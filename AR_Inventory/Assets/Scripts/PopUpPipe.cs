using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PopUpPipe
{
    public static string info;
    //add a field for product image and a list of possible stores

    private static bool loaded = false;

    public static void LoadPopUp()
    {
        if (!loaded)
        {
            SceneManager.LoadScene("Scenes/PopUp", LoadSceneMode.Additive);
            loaded = true;
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
            SceneManager.UnloadScene("Scenes/PopUp");
            loaded = false;
        }
        else
        {
            Debug.Log("No Window Is Open");
        }
    }
}
