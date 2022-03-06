using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;
using UnityEngine.Assertions;


public class KImageTarget : MonoBehaviour
{
    public ImageTargetBehaviour IT;
    public GameObject ARobject;
    public GameObject CloneARObj;
    public GameObject Timer;
    public GameObject TimerClone;
    string name, path;
    bool loaded = false;
    Stack EventsTSC = new Stack();
    Stack EventsOB = new Stack();
    Stack VirtualButtons = new Stack();
    public event Action ITDone;
    public bool active = false;

    private string basePath = "music/";

    public KImageTarget(string p, string n, float delay = .1f, GameObject ARoj = null, GameObject TObj = null)
    {
        path = p;
        name = n;
        IT = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(path, name);
        ARobject = ARoj;
        Timer = TObj;
        activate_IT(delay);
        addEvent(OnTSC);  
    }

    public void activate_IT(float delay = .1f)
    {
        active = true;
        IT.enabled = true;
        TimerClone = Instantiate(Timer);
        var timer = TimerClone.GetComponent<KTimer>();
        timer.TimerStop += kill;
        timer.StartTimer(delay);
    }

    public void load_AROB(string message = "")
    {
        loaded = true;
        CloneARObj = Instantiate(ARobject, new Vector3(-0.5f,-7,0), Quaternion.identity);
        CloneARObj.transform.parent = IT.transform;
        CloneARObj.transform.Rotate(new Vector3(0, 90, 0));
        var display = CloneARObj.GetComponent<Display>();
        display.SetText(message);
    }

    public void updateScreen(string message = "")
    {
        var display = CloneARObj.GetComponent<Display>();
        display.SetText(message);
    }

    private void VirtualButton(string VBName)
    {
        VirtualButtons.Push(IT.CreateVirtualButton(VBName, IT.transform.position, IT.GetSize()));
    }

    public void kill()
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

        for (int i = 0; i < VirtualButtons.Count; i++)
        {
            var vb = (VirtualButtonBehaviour)VirtualButtons.Pop();
            IT.DestroyVirtualButton(vb.VirtualButtonName);
        }

        DestroyImmediate(IT);
        IT = null;
        Destroy(TimerClone);
        if (loaded) Destroy(CloneARObj);
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
            var timer = TimerClone.GetComponent<KTimer>();
            timer.StopTimer();
            if (!loaded)
            {
                var firestore = FirebaseFirestore.DefaultInstance;
                firestore.Document(basePath + observerbehavour.TargetName).GetSnapshotAsync().ContinueWithOnMainThread(task => 
                {
                    Assert.IsNull(task.Exception);
                    var result = task.Result.ConvertTo<Album>();
                    string resultStr = result.Name + "\n" + result.Artist;
                    updateScreen(resultStr);
                });
                load_AROB("Loading");

            }
        }
        else 
        {
            if (loaded)
            {
                loaded = false;
                Destroy(CloneARObj);
            }
        }
    }
    
    
}
