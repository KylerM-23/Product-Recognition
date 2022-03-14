using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicImageTarget : KImageTarget
{

    public string Artist = "";

    public MusicImageTarget(string p, string n, GameObject ARObj, string a, float delay = .1f, GameObject TObj = null) :
        base(p, n, delay, ARObj, TObj)
    {
        Artist = a;
    }
}
