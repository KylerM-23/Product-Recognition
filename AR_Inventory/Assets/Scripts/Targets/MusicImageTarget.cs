using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicImageTarget : KImageTarget
{

    public string Artist = "";

    public MusicImageTarget(string p, string n, GameObject ARObj, GameObject TObj, string a, float delay = .1f) :
        base(p, n, delay, ARObj, TObj)
    {
        Artist = a;
    }
}
