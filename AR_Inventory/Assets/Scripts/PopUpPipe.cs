using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class PopUpPipe
{
    public static string info { get; private set; }
    public static string id { get; private set; }
    static int locks = 0;
    static int locks_max = 3;
    static bool data_aquired = false;

    public static List<Dictionary<string, string>> stores { get; private set; }

    //add a field for product image and a list of possible stores

    private static bool loaded = false;

    public static void LoadPopUp()
    {
        if (!loaded)
        {
            locks++;
            if (locks >= locks_max)
            {
                SceneManager.LoadScene("Scenes/PopUp", LoadSceneMode.Additive);
                loaded = true;
                data_aquired = true;
            }
        }
        else
        {
            Debug.Log("Window is loaded");
        }
    }

    public static void ClosePopUp()
    {
        if (loaded)
        {
            loaded = false;
            SceneManager.UnloadSceneAsync("Scenes/PopUp");
        }
        else
        {
            Debug.Log("No Window Is Open");
        }
    }

    public static void SetInfo(string information, string ID)
    {
        info = information;
        id = ID;
    }

    public static void SetLock(int max_num)
    {
        locks_max = max_num;
        if (data_aquired == false)
            locks = 0;
    }

    public static void SetStores(List<Dictionary<string, string>> storeIn)
    {
        stores = storeIn;
    }
}
