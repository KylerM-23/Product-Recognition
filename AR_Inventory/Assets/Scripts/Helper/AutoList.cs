using System.Collections;
using System.Collections.Generic;
using System;

public class AutoList<T>
{
    private List<T> list = new List<T>();
    private int index = 0;
    private bool looped = false;

    public void AddItem(T item)
    {
        list.Add(item);
    }

    public T GetItem()
    {
        T result = default(T);
        if (list.Count > 0)
        {
            if (index >= list.Count)
            {
                looped = true;
                index = 0;
            }

            result = list[index];
            index++;
            
        }
        return result;
        
    }

    public void Reset()
    {
        index = 0;
        looped = false;
    }

    public bool GetLooped()
    {
        return looped;
    }
}
