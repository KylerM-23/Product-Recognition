using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;


public class KImageTarget : KTrackable
{
    protected ImageTargetBehaviour IT;

    protected Dictionary<string, string> Info = new Dictionary<string, string>()
    {
        { "ID" , ""}
    };

    protected GameObject ARObj;
    protected GameObject Timer;

    bool loaded = false;
    public event Action ITDone;
    protected string Database = "";
    protected bool dataLoaded = false;
    bool press = false;

    protected DocumentReference FirestoreRef;

    public KImageTarget(string p, string n, float delay = .1f, GameObject ARoj = null, GameObject TObj = null)
    {
        active = false;
        path = p;
        name = n;
        IT = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(path, name);
        ARObj = ARoj;
        ARObj.SetActive(false);
        Timer = TObj;
        activate_IT(delay);
        addEvent(OnTSC);
    }

    public void activate_IT(float delay = .1f)
    {
        active = true;
        IT.enabled = true;
        var timer = Timer.GetComponent<KTimer>();
        timer.TimerStop += kill;
        timer.StartTimer(delay);
    }

    public void load_AROB()
    {
        loaded = true;
        ARObj.transform.position = new Vector3(-0.5f, -7, 0);
        ARObj.transform.parent = IT.transform;
        ARObj.SetActive(true);
        ARObj.transform.Rotate(new Vector3(0, 180, 0));
    }


    public override void kill()
    {
        active = false;
        IT.enabled = false;
        ITDone();
        
        for (int i = 0; i < EventsOB.Count; i++)
        { 
            var event1 = (Action)EventsOB.Pop();
            ITDone -= event1;
        }

        for (int i = 0; i < EventsTSC.Count; i++)
        {
            var event2 = (Action<ObserverBehaviour, TargetStatus>)EventsTSC.Pop();
            IT.OnTargetStatusChanged -= event2;
        }

        DestroyImmediate(IT);
        IT = null; 
        Destroy(Timer);
        Destroy(ARObj);
    }

    public void addEvent(Action<ObserverBehaviour, TargetStatus> OnTargetStatusChanged)
    {
        IT.OnTargetStatusChanged += OnTargetStatusChanged;
        EventsTSC.Push(OnTargetStatusChanged);
    }

    public void addEvent(Action ITDoneFunc)
    {
        ITDone += ITDoneFunc;
        EventsOB.Push(ITDoneFunc);
    }


    private void OnTSC(ObserverBehaviour observerbehavour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED && status.StatusInfo == StatusInfo.NORMAL)
        {
            var timer = Timer.GetComponent<KTimer>();
            timer.StopTimer();
            if (!loaded) 
                load_AROB();
        }
        else 
        {
            if (loaded && !press)
            {
                loaded = false;
                ARObj.SetActive(false);
            }
        }
    }

    protected void GetStores(int limit = 5)
    {
        List<Dictionary<string, string>> storeList = new List<Dictionary<string, string>>();

        var firestore = FirebaseFirestore.DefaultInstance;
        CollectionReference storeCollection = firestore.Collection("Stores");
        Query possibleStores = storeCollection.WhereArrayContains("AllProducts", Info["ID"]);
        possibleStores.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot storeSnapshots = task.Result;
            foreach (DocumentSnapshot documentSnapshot in storeSnapshots.Documents)
            {
                if (storeList.Count >= limit)
                    break;
                Dictionary<string, object> storeDict = documentSnapshot.ToDictionary();
                Dictionary<string, string> storeInfo = new Dictionary<string, string>();
                storeInfo.Add("DocumentID", (string) documentSnapshot.Id);
                storeInfo.Add("Name", (string) storeDict["Name"]);
                storeInfo.Add("Price", "N/A");
                storeInfo.Add("Stock", "N/A");
                storeList.Add(storeInfo);
            }
            if (storeList.Count == limit)
                GetStoreInfo(storeList, 0, limit);
            else
                GetStoreInfo(storeList, 0, storeList.Count);
        });
    }

    protected void GetStoreInfo(List<Dictionary<string, string>> storeList, int i, int limit = 5)
    {
        int next = i + 1;
        Debug.Log(i);
        Debug.Log(next);
        if (i >= limit)
        {
            Debug.Log("Done");
            PopUpPipe.SetStores(storeList);
            PopUpPipe.LoadPopUp();
            return;
        }
        else
        {
            Dictionary<string, string> current_store = storeList[i];
            string path = String.Format("Stores/{0}/ProductInformation/IDs", current_store["DocumentID"]);
            
            try
            {
                var firestore = FirebaseFirestore.DefaultInstance;
                firestore.Document(path).GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    var result = (Dictionary<string, object>)task.Result.ToDictionary();
                    var ID_Dict = (Dictionary<string, object>)result["ID"];
                    var doc = (DocumentReference)ID_Dict[Info["ID"]];
                    doc.GetSnapshotAsync().ContinueWithOnMainThread(task2 =>
                    {
                        var product_result = task2.Result.ToDictionary();
                        current_store["Price"] = (string)product_result["Price"];
                        current_store["Stock"] = (string)product_result["Stock"];
                        GetStoreInfo(storeList, next, limit);
                    });

                });
            }
            catch
            {
                Debug.Log("Catch");
                this.GetStoreInfo(storeList, next, limit);
            }
        }

    }

    protected void AcquireData(ObserverBehaviour observerbehavour, TargetStatus status)
    {
        PopUpPipe.SetLock(3);
        if (!dataLoaded)
        {
            dataLoaded = true;
            var firestore = FirebaseFirestore.DefaultInstance;
            firestore.Document(Database).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                SetData(task.Result.ToDictionary());
            });
        }
    }

    public virtual void SetData(Dictionary<string, object> result)
    {
        string output = "";
        Debug.Log("test");

        List<string> keyList = new List<string>(Info.Keys);
        
        foreach (string key in keyList)
        {
            Debug.Log(key);
            Info[key] = (string)result[key];
            Debug.Log(Info[key]);
            if (key != "ID")
                output = output + key + ": " + Info[key] + "\n";
        }
        
        PopUpPipe.SetInfo(output, Info["ID"]);
        PopUpPipe.LoadPopUp();
        GetStores();
    }


}
