using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Firebase.Firestore;
using Firebase;
using Firebase.Extensions;
using System;

public class FImageTarget : KImageTarget
{
    public FImageTarget(string p, string n, string FS, GameObject ARObj, GameObject TObj, float delay = .1f) :
        base("Vuforia/" + p + ".xml", n, delay, ARObj, TObj)
    {
        Database = "Food/Brands/" + FS + "/" + IT.TargetName;
        this.addEvent(AcquireData);

        this.Info = new Dictionary<string, string>()
        {
            {"Name", "" },
            { "Company" , ""},
            { "Type" , ""},
            {"Calories", "" },
            { "ID", ""}
        };
    }
}
