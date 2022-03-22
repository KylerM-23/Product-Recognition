using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VGImageTarget : KImageTarget
{
    public string Studio = "";

    public VGImageTarget(string p, string n, GameObject ARObj, GameObject TObj, string studio, float delay = .1f) :
        base(p, n, delay, ARObj, TObj)
    {
        Studio = studio;
    }
}
