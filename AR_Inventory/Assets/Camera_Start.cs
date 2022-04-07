using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour
{
    public Camera ARCam;
    public Camera LessonCam;

    // Start is called before the first frame update
    void Start()
    {
        Camera_Manager.ARCam = ARCam;
        Camera_Manager.LessonCam = LessonCam;
        Camera_Manager.SetCamera();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
