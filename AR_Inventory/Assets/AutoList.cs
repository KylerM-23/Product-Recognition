using System.Collections;
using System.Collections.Generic;
using System;

public class AutoList<T>
{
    private List<T> list = new List<T>();
    private int index = 0;
    
    public void AddItem(T item)
    {
        list.Add(item);
    }

    public T GetItem()
    {
        T result = default(T);
        if (list.Count > 0)
        {
            result = list[index];
            index++;
            if (index >= list.Count) index = 0;
        }
        return result;
        
    }

    public void Reset()
    {
        index = 0;
    }
}
