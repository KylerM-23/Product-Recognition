using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Display : MonoBehaviour
{
    public TextMesh output;

    public void SetText(string message)
    {
        output.text = message;
    }

   
}
