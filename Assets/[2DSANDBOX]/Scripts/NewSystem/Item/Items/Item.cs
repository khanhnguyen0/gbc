using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class Item : ISaveable
{
    private int _id;

    public int id { get { return _id; } }

    public Item()
    {
        _id = 0;
    }

    public Item(int id)
    {
        _id = id;
    }

    public virtual bool activateItem(Stack stack)
    {
        return true;
    }

    public void saveData(StreamWriter writer)
    {
        writer.WriteLine(_id);
    }

    public void loadData(StreamReader reader)
    {
        _id = int.Parse(reader.ReadLine());
    }
}
