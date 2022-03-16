using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.Assertions;
using System.Xml;
using System;
using UnityEngine.SceneManagement;


public class ButtonTarget : MonoBehaviour
{
    const int n = 4;
    public Text textBox;
    public Text debugBox;
    public GameObject ARobject;
    public GameObject TObj;

    public GameObject CategoryManager;

    string baseStr = "Vuforia/";
    string databaseName = "";
    string fileExt = ".xml";
    private string[] names;
    Stack kImages = new Stack();

    Stack databases = new Stack();
    Stack collections = new Stack();

    Dictionary<string, object> databaseData;

    AutoList<string> categories = new AutoList<string>();
    string category = "";
    string targetType = "";
    int max = 0;
    private int place = 0;
    int counter = 0;
    bool found = false;
    bool done = false;
    bool loading = false;
    bool searchReady = false;
    
    XmlDocument dataBase = new XmlDocument();

    delegate void CreateImageTarget(float delay = .1f);
    CreateImageTarget CIT = null;

    // Start is called before the first frame update
    void Start()
    {
        debugBox.text = "Start";
        loadCategories();
    }

    private void DatabaseEmpty()
    {
        searchReady = false;
        if (databases.Count == 0)
        {
            if (collections.Count == 0)
            {
                if (categories.GetLooped())
                    SearchFail();
                else loadCollections();
            }
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
            debugBox.text = "Stage 0.1";
            List<object> tem = (List<object>) result["categories"];
            debugBox.text = "Stage 0.2";
            for (int i = 0; i < tem.Count; i++)
            {
                string x = (string) tem[i];
                categories.AddItem(x);
            }
            loadCollections();
        });
    }
    private void loadCollections()
    {
        category = categories.GetItem();
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(category + "/DatabaseRefrences").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            var result = task.Result.ToDictionary();
            debugBox.text = "Stage 1.1";
            List<object> tem = (List<object>)result["Ref"];
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
        if (Application.platform == RuntimePlatform.Android)
        {
            var webrequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + baseStr + databaseName + fileExt);
            webrequest.SendWebRequest();
            while (!webrequest.isDone) ;
            loadData(webrequest.downloadHandler.text, true);
        }

        else loadData("Assets/StreamingAssets/" + baseStr + databaseName + fileExt);

        textBox.text = "Press Search When Ready.";
        searchReady = true;
        Search();
    }

    private void loadData(string p, bool mode = false)
    {
        if (mode) dataBase.LoadXml(p);
        else dataBase.Load(p);
        XmlElement root = dataBase.DocumentElement;
        XmlNodeList nodes = root.SelectNodes("Tracking/ImageTarget");
        max = nodes.Count;
        names = new string[max];
        int temp = 0;
        foreach (XmlNode node in nodes)
        {
            names[temp] = node.Attributes["name"].InnerText;
            temp++;
        }
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
            if (targetType == "Image" && category == "Music")
                CIT = createMIT;
            else
                CIT = createImageTarget;
            
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

    public void SearchFail()
    {
        done = true;
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
}
