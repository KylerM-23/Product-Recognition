using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;

public class MusicImageTarget : KImageTarget
{

    public string Artist = "";
    public string Album = "";
    bool dataLoaded = false;

    public MusicImageTarget(string p, string n, GameObject ARObj, GameObject TObj, string a, float delay = .1f) :
        base(p, n, delay, ARObj, TObj)
    {
        Artist = a;
        this.addEvent(AquireData);
    }

    void AquireData(ObserverBehaviour observerbehavour, TargetStatus status)
    {
        if (!dataLoaded)
        {
            dataLoaded = true;
            var firestore = FirebaseFirestore.DefaultInstance;
            string path = "Music/Artists/" + Artist + "/" + IT.TargetName;
            firestore.Document(path).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                var result = task.Result.ToDictionary();
                Album = (string)result["Name"];
                var temp = string.Format("Album: {0}\n Artist {1}\n", Album, Artist);
                PopUpPipe.info = temp;
            });
        }
    }
}
