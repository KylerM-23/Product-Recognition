using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryRetrival : MonoBehaviour
{
    public string[] keys;
    public GameObject[] items;
    Dictionary<string, GameObject> storage = new Dictionary<string, GameObject>();

    void Start()
    {
        for (int i = 0; i < keys.Length; i++)
            storage.Add(keys[i], items[i]);
    }

    public GameObject GetObject(string category)
    {
        if (storage.ContainsKey(category))
        {
            return Instantiate(storage[category], new Vector3(0, 0, 0), Quaternion.identity);
        }
        else return null;
    }

}
