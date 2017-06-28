using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Pool<T>
{
    private Queue<T> pool;

    public Pool()
    {
        pool = new Queue<T>();
    }

    public T get()
    {
        if(pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            return create();
        }
    }

    public void store(T item)
    {
        pool.Enqueue(item);
    }

    private T create()
    {
        T item = Activator.CreateInstance<T>();
        return item;
    }
}