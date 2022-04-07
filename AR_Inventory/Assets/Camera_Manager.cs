using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Camera_Manager
{
    static public Camera ARCam;
    static public Camera LessonCam;
    static bool AR = true;

    public static void SetCamera()
    {
        ARCam.enabled = AR;
        LessonCam.enabled = !AR;
    }

    public static void SwitchCamera()
    {
        AR = !AR;
        SetCamera();
    }

}
