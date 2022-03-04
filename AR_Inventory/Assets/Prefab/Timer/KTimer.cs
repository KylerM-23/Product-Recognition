using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTimer : MonoBehaviour
{

    bool active = false;
    float timeRemaing = 0f;

    public delegate void TimerEnd();
    public event TimerEnd TimerStop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timeRemaing -= Time.deltaTime;
            if (timeRemaing < 0)
            {
                active = false;
                this.TimerStop();
            }
        }
    }

    public void StartTimer(float time)
    {
        active = true;
        timeRemaing = time;
    }

    public void StopTimer()
    {
        active = false;
    }
}
