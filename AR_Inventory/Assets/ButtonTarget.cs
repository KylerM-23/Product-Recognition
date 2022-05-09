using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.Assertions;
using System;
using UnityEngine.SceneManagement;

public class ButtonTarget : MonoBehaviour
{
    const int n = 4;
    public GameObject ARobject;
    public GameObject TObj;

    public Button restartButton;

    public GameObject CategoryManager;

    string baseStr = "Vuforia/";
    string databaseName = "";
    string fileExt = ".xml";

    XML_Loader xml_loader = new XML_Loader();

    Stack kImages = new Stack();

    Stack databases = new Stack();
    Stack collections = new Stack();

    Dictionary<string, object> databaseData;

    List<string> Categories = new List<string>();
    Stack categories = new Stack();

    string[] names;
    string category = "";
    string targetType = "";
    int max = 0;
    private int place = 0;
    int counter = 0;
    bool found = false;
    bool done = false;
    bool loading = false;
    bool searchReady = false;

    void Start()
    {
        restartButton.gameObject.SetActive(false);
        loadCategories();
    }

    private void DatabaseEmpty()
    {
        if (categories.Count == 0) SearchFail();

        if (databases.Count == 0)
        {
            if (collections.Count == 0) loadCollections();
            else loadDatabasePaths();
        }
        else configDatabase();
    }

    private void loadCategories()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("Databases/Categories").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result.ToDictionary();
            List<object> tem = (List<object>) result["categories"];
            for (int i = 0; i < tem.Count; i++)
            {
                string x = (string) tem[i];
                Categories.Add(x);
                categories.Push(x);
            }
            loadCollections();
        });
    }
    private void loadCollections()
    {
        category = (string) categories.Pop();
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("Databases/DatabaseRefrences").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            var result = task.Result.ToDictionary();
            List<object> tem = (List<object>)result[category];
            for (int i = 0; i < tem.Count; i++)
            {
                var x = tem[i];
                collections.Push(x);
            }
            loadDatabasePaths();
        });
    }

    private void loadDatabasePaths()
    {
        var doc = (DocumentReference)collections.Pop();
        doc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            var result = task.Result.ToDictionary();
            databases.Push(result);
            configDatabase();
        });
    }

    private void configDatabase()
    {
        databaseData = (Dictionary<string, object>)databases.Pop();
        databaseName = category + "/" + (string) databaseData["path"];
        targetType = (string) databaseData["type"];
        xml_loader.LoadXMLDatabase(baseStr + databaseName, targetType);
        names = xml_loader.GetItemNames();
        max = names.Length;
        searchReady = true;
        Search();
    }

    public void RestartSearch()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void Search()
    {
        if (searchReady)
        {
            ClearImages();
            place = 0;
            found = false;
            counter = 0;
            createImageTarget(category, 3f);
        }
    }

    public void createImageTarget(string cate, float delay = .1f)
    {
        if (!found)
        {
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    var ARobjClone = CategoryManager.GetComponent<CategoryRetrival>().GetObject(cate);
                    var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);

                    KImageTarget kIT;

                    switch (cate)
                    {
                        case "Music":
                            kIT = new MusicImageTarget(databaseName, names[place], (string)databaseData["Firestore"], ARobjClone, TimerClone, delay); break;
                        case "Video_Games":
                            kIT = new VGImageTarget(databaseName, names[place], (string)databaseData["Firestore"], ARobjClone, TimerClone, delay); break;
                        case "Books":
                            kIT = new BookImageTarget(databaseName, names[place], (string)databaseData["Firestore"], ARobjClone, TimerClone, delay); break;
                        case "Food":
                            kIT = new FImageTarget(databaseName, names[place], (string)databaseData["Firestore"], ARobjClone, TimerClone, delay); break;
                        case "Home&Office":
                            kIT = new H_OImageTarget(databaseName, names[place], (string)databaseData["Firestore"], ARobjClone, TimerClone, delay); break;
                        default:
                            kIT = new KImageTarget(baseStr + databaseName + fileExt, names[place], delay, ARobjClone, TimerClone); break;
                    }

                    kIT.addEvent(OnTargetStatusChanged);
                    kIT.addEvent(IncrementCount);
                    kImages.Push(kIT);
                    place++;
                }
            }
        }
    }

    public void SearchFail()
    {
        done = true;
        restartButton.gameObject.SetActive(true);
    }

    void IncrementCount()
    {
        counter++;
        if (place >= max && found == false) DatabaseEmpty();
        if (counter >= n && found != true) IncrementRestart();
    }

    void IncrementRestart()
    {
        counter = 0;
        ClearImages();
        createImageTarget(category);
    }

    void OnTargetStatusChanged(ObserverBehaviour observerbehavour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED && status.StatusInfo == StatusInfo.NORMAL)
        {
            restartButton.gameObject.SetActive(true);
            found = true;
        }
    }

    void ClearImages()
    {
        for (int j = 0; j < n && j < kImages.Count; j++)
        {
            KImageTarget kIT = (KImageTarget)kImages.Pop();
            if (kIT.active) kIT.kill();
            kIT = null;
        }
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("Scenes/Start");
    }
}
