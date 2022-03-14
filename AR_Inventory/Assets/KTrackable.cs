using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KTrackable : MonoBehaviour
{
    protected string name, path;
    protected Stack EventsTSC = new Stack();
    protected Stack EventsOB = new Stack();
    public bool active { get; protected set; }

    abstract public void kill();
}
