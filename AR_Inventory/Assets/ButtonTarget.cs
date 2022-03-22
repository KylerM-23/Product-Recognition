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
    public Text textBox;
    public Text debugBox;
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
    
    

    delegate void CreateImageTarget(float delay = .1f);
    CreateImageTarget CIT = null;

    // Start is called before the first frame update
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
        if (categories.Count == 0) { SearchFail(); return; }

        category = (string) categories.Pop();
        Debug.Log(category);
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("Databases/DatabaseRefrences").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            var result = task.Result.ToDictionary();
            debugBox.text = "Stage 1.1";
            List<object> tem = (List<object>)result[category];
            debugBox.text = "Stage 1.2";
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
        debugBox.text = "Stage 2";
        var doc = (DocumentReference)collections.Pop();
        doc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            var result = task.Result.ToDictionary();
            Debug.Log(result["path"]);
            databases.Push(result);
            configDatabase();
        });
    }

    private void configDatabase()
    {
        debugBox.text = "Stage 3";
        
        databaseData = (Dictionary<string, object>)databases.Pop();

        debugBox.text = "Stage 3.1"; 
        databaseName = category + "/" + (string) databaseData["path"];
        debugBox.text = "Stage 3.2";
        targetType = (string) databaseData["type"];
        debugBox.text = databaseName;
        xml_loader.LoadXMLDatabase(baseStr + databaseName, targetType);
        names = xml_loader.GetItemNames();
        max = names.Length;
        textBox.text = "Press Search When Ready.";
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
            if (targetType == "Image")
                switch (category)
                {
                    case "Music":
                        CIT = createMIT; break;
                    case "Video_Games":
                        CIT = createVGIT; break;
                    default:
                        CIT = createImageTarget; break;
                }

            ClearImages();
            place = 0;
            found = false;
            textBox.text = "Prepping Search";
            counter = 0;
            textBox.text = "Searching...";
            CIT(3f);
        }
    }

    public void createImageTarget(float delay = .1f)
    {
        if (!found)
        {
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    var ARobjClone = Instantiate(ARobject, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                    var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                    KImageTarget kIT = new KImageTarget(baseStr + databaseName + fileExt, names[place], delay, ARobjClone, TimerClone);
                    kIT.addEvent(OnTargetStatusChanged);
                    kIT.addEvent(IncrementCount);
                    kImages.Push(kIT);
                    place++;
                    textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                }
            }
        }
    }

    public void createMIT(float delay = .1f)
    {
        if (!found)
        {
            string artist = (string) databaseData["Artist"];
            debugBox.text = artist;
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    var ARobjClone = CategoryManager.GetComponent<CategoryRetrival>().GetObject("Music");
                    var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                    MusicImageTarget MIT = new MusicImageTarget(baseStr + databaseName + fileExt, names[place], ARobjClone, TimerClone, artist, delay);
                    MIT.addEvent(OnTargetStatusChanged);
                    MIT.addEvent(IncrementCount);
                    kImages.Push(MIT);
                    place++;
                    textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                }
            }
        }
    }

    public void createVGIT(float delay = .1f)
    {
        if (!found)
        {
            string studio = (string)databaseData["Studio"];
            debugBox.text = studio;
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    var ARobjClone = CategoryManager.GetComponent<CategoryRetrival>().GetObject("Video_Games");
                    var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                    VGImageTarget VGIT = new VGImageTarget(baseStr + databaseName + fileExt, names[place], ARobjClone, TimerClone, studio, delay);
                    VGIT.addEvent(OnTargetStatusChanged);
                    VGIT.addEvent(IncrementCount);
                    kImages.Push(VGIT);
                    place++;

                    textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                }
            }
        }
    }

    public void SearchFail()
    {
        done = true;
        restartButton.gameObject.SetActive(true);
        textBox.text = "Search Failed.";
    }

    void IncrementCount()
    {
        counter++;
        if (place >= max && found == false) DatabaseEmpty();
        if (counter >= n && found != true) IncrementRestart();
    }

    void IncrementRestart()
    {
        textBox.text = "Searching...";
        counter = 0;
        ClearImages();
        CIT();
    }

    void OnTargetStatusChanged(ObserverBehaviour observerbehavour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED && status.StatusInfo == StatusInfo.NORMAL)
        {
            restartButton.gameObject.SetActive(true);
            textBox.text = "Found " + observerbehavour.TargetName;
            found = true;
        }
    }

    void ClearImages()
    {
        for (int j = 0; j < n && j < kImages.Count; j++)
        {
            KImageTarget kIT = (KImageTarget)kImages.Pop();
            if (kIT.active) kIT.kill();
            DestroyImmediate(kIT);
            kIT = null;
        }
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("Scenes/Start");
    }
}
