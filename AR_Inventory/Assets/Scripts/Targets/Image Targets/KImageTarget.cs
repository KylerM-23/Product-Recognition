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
    protected string ID;
    protected GameObject ARObj;
    protected GameObject Timer;

    bool loaded = false;
    public event Action ITDone;

    bool press = false;

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
            {
                load_AROB();
            }
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

    protected void GetStores()
    {
        List<string> storeNames = new List<string>();
        var firestore = FirebaseFirestore.DefaultInstance;
        CollectionReference storeCollection = firestore.Collection("Stores");
        Query possibleStores = storeCollection.WhereArrayContains("AllProducts", ID);
        possibleStores.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot storeSnapshots = task.Result;

            foreach (DocumentSnapshot documentSnapshot in storeSnapshots.Documents)
                storeNames.Add(documentSnapshot.Id);
            PopUpPipe.SetStores(storeNames);
            PopUpPipe.LoadPopUp();
        });
    }


}
