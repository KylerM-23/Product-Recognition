using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWindow : MonoBehaviour
{
    public Text data;
    string info = "";
    
    void Start()
    {
        Debug.Log(PopUpPipe.info);
        info = PopUpPipe.info;
        data.text = info;
    }
    
    public void CloseWindow()
    {
        PopUpPipe.ClosePopUp();
    }
}
