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
    public GameObject AROB;
    public GameObject TObj;
    string baseStr = "Vuforia/";
    string databaseName = "";
    string fileExt = ".xml";
    private string[] names;
    Stack kImages = new Stack();

    Stack databases = new Stack();
    Stack collections = new Stack();

    AutoList<string> categories = new AutoList<string>();
    string category = "";
    string targetType = "";
    int max = 0;
    private int place = 0;
    int counter = 0;
    bool found = false;
    bool search = false;
    bool searchReady = false;
    
    XmlDocument dataBase = new XmlDocument();
    
    delegate void Stage(); //delegate for stages that occur one after another.
    Stage[] stages = new Stage[4];

    delegate void CreateImageTarget(float delay = .1f);
    CreateImageTarget CIT = null;

    // Start is called before the first frame update
    void Start()
    {
        debugBox.text = "Start";
        stages[0] = new Stage(loadCategories);
        stages[1] = new Stage(loadCollections);
        stages[2] = new Stage(loadDatabasePaths);
        stages[3] = new Stage(configDatabase);

        stages[0]();
    }

    private void loadCategories()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("Common/CommonInfo").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
                Assert.IsNull(task.Exception);
                var result = task.Result.ToDictionary();
                debugBox.text = "Stage 0.1";
                List<object> tem = (List<object>) result["Categories"];
                debugBox.text = "Stage 0.2";
                for (int i = 0; i < tem.Count; i++)
                {
                    string x = (string) tem[i];
                    categories.AddItem(x);
                }
                stages[1]();
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
            stages[2]();
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
            stages[3]();
        });
    }

    private void configDatabase()
    {
        var data = (Dictionary<string, string>) databases.Pop();
        databaseName = category + "/" + data["path"];
        targetType = data["type"];

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

    public void Search()
    {
        if (searchReady)
        {
            if (search)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }

            if (targetType == "Music")
                CIT = createMIT;
            else
                CIT = createImageTarget;
            search = true;
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
                    KImageTarget kIT = new KImageTarget(baseStr + databaseName + fileExt, names[place], delay, AROB, TObj);
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
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    KImageTarget kIT = new KImageTarget(baseStr + databaseName + fileExt, names[place], delay, AROB, TObj);
                    kIT.addEvent(OnTargetStatusChanged);
                    kIT.addEvent(IncrementCount);
                    kImages.Push(kIT);
                    place++;
                    textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                }
            }
        }
    }

    public void SearchFail()
    {
        textBox.text = "Search Failed.";
    }

    void IncrementCount()
    {
        counter++;
        if (place >= max && found == false) SearchFail();
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
