using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Stack : ISaveable
{
    private Item _item;
    private int _count;

    public Item item { get { return _item; } }
    public int count { get { return _count; } }

    public Stack()
    {
        _count = 0;
        _item = null;
    }

    public Stack(Item item, int count)
    {
        _item = item;
        _count = count;
    }

    public void removeItem(int amount)
    {
        _count -= amount;
    }

    public void addItem(int amount)
    {
        _count += amount;
    }

    public void saveData(StreamWriter writer)
    {
        writer.WriteLine(_count);
        item.saveData(writer);
    }

    public void loadData(StreamReader reader)
    {
        _count = int.Parse(reader.ReadLine());
        int id = int.Parse(reader.ReadLine());
        _item = ItemStore.createItem(id);
        //item.loadData(reader);
    }
}