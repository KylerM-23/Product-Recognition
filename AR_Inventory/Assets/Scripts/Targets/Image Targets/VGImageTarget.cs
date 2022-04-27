using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;
using System;

public class VGImageTarget : KImageTarget
{
    public VGImageTarget(string p, string n, string FS, GameObject ARObj, GameObject TObj, float delay = .1f) :
        base("Vuforia/" + p + ".xml", n, delay, ARObj, TObj)
    {
        Database = "Video_Games/Companies/" + FS + "/" + IT.TargetName;
        this.addEvent(AcquireData);
        
        this.Info = new Dictionary<string, string>()
        {
            {"Name", "" },
            { "Studio" , ""},
            { "Genre" , ""},
            { "Publisher", ""},
            { "ID", ""}
        };
    }
}
