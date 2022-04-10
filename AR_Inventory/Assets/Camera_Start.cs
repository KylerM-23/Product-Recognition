using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera_Start : MonoBehaviour
{
    //public Camera ARCam;
    //public Camera PopUpCam;

    // Start is called before the first frame update
    void Start()
    {
        //Camera_Manager.ARCam = ARCam;
        //Camera_Manager.SecondCam = PopUpCam;
        //Camera_Manager.SetCamera();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadSearch()
    {
        SceneManager.LoadScene("Scenes/Search");
    }

}
