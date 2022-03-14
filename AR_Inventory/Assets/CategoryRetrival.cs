using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryRetrival : MonoBehaviour
{
    public GameObject[] storage = new GameObject[3];
    // Start is called before the first frame update
    public GameObject GetObject(int index)
    {
        if (index < storage.Length)
        {
            return storage[index];
        }
        else return null;
    }
}
