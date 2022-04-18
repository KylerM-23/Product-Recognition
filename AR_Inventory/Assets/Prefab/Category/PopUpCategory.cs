using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCategory : MonoBehaviour
{
    bool click = false;

    void Start()
    {
        click = false;    
    }
    void OnMouseDown()
    {
        if (!click)
            PopUpPipe.LoadPopUp();
    }
}
