using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class ComponentPool<T> where T : Component
{
    private Queue<T> _pool;
    private GameObject _parent;

    public ComponentPool(GameObject parent)
    {
        _pool = new Queue<T>();
        _parent = parent;
    }

    public T get()
    {
        T item;
        if(_pool.Count > 0)
        {
            item = _pool.Dequeue();
        }
        else
        {
            item = create();
        }

        item.gameObject.SetActive(true);

        return item;
    }

    public void store(T item)
    {
        item.gameObject.SetActive(false);

        _pool.Enqueue(item);
    }

    private T create()
    {
        GameObject obj = new GameObject();

        obj.name = "Pooled " + typeof(T).Name;
        obj.transform.parent = _parent.transform;

        T item = obj.AddComponent<T>();

        return item;
    }
}