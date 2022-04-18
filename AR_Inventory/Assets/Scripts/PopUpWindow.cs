using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PopUpWindow : MonoBehaviour
{
    public Text data;
    string info = "";
    
    void Start()
    {
        info = PopUpPipe.info;
        string storesString = "Stores:\n";
        for (int i = 0; i < PopUpPipe.stores.Count; i++)
        {
            Dictionary<string, string>  temp = PopUpPipe.stores[i];
            storesString = storesString + String.Format("{0}:\n\tPrice: ${1}\n\tStock: {2}\n", temp["Name"], temp["Price"], temp["Stock"]);
        }
        var infoText = String.Format("{0}\n{1}", info, storesString);
        data.text = infoText;
    }
    
    public void CloseWindow()
    {
        PopUpPipe.ClosePopUp();
    }
}
