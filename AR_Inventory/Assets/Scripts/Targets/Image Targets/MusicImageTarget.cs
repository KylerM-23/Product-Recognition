using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;
using System;

public class MusicImageTarget : KImageTarget
{

    public MusicImageTarget(string p, string n, string FS, GameObject ARObj, GameObject TObj, float delay = .1f) :
        base("Vuforia/" + p + ".xml", n, delay, ARObj, TObj)
    {
        Database = "Music/Artists/" + FS + "/" + IT.TargetName;
        this.addEvent(AcquireData);

        this.Info = new Dictionary<string, string>()
        {
            {"Artist", "" },
            { "Album Name" , ""},
            { "ID" , ""}
        };
    }
}
