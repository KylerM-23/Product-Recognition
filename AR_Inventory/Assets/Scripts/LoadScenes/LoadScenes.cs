using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadScenes
{
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("Scenes/Start");
    }
}
