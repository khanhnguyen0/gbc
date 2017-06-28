using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

[System.Serializable]
public class Inventory : ISaveable
{
    private List<Stack> _items;
    private int _activeIndex;

    public List<Stack> items { get { return _items; } }
    public Stack activeStack
    {
        get
        {
            if (_items.Count > 0)
            {
                try
                {
                    return _items[_activeIndex];
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
    public int activeIndex { get { return _activeIndex; } }

    public Inventory()
    {
        _items = new List<Stack>();
    }

    public void setActiveItem(int index)
    {
        _activeIndex = index;
    }

    public void removeItem(Stack stack)
    {
        items.Remove(stack);
    }

    /*public void addItem(Stack stack)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].item.id == stack.item.id) // Does the inventory contain this itemstack already?
            {
                _items[i].addItem(stack.count);
            }
            else
            {
                _items.Add(stack);
                break;
            }
        }
    }*/

    public void addItem(Item item, int count)
    {
        // Iterate to find a stack which has this item in it
        for(int i = 0; i < _items.Count; i++)
        {
            if(_items[i].item.id == item.id) // check for same stack
            {
                _items[i].addItem(count);
                return;
            }
        }

        Stack stack = new Stack(item, count);
        _items.Add(stack);
    }

    public void removeItem(Item item, int count)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].item.id == item.id)
            {
                _items[i].removeItem(count);

                if (_items[i].count == 0)
                    _items.RemoveAt(i);

                return;    
            }
        }
    }

    public void saveData(StreamWriter writer)
    {
        writer.WriteLine(_items.Count);
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].saveData(writer);
        }
    }

    public void loadData(StreamReader reader)
    {
        _activeIndex = 0;
        int size = int.Parse(reader.ReadLine());
        for(int i = 0; i < size; i++)
        {
            Stack stack = new Stack();
            stack.loadData(reader);
            _items.Add(stack);
        }
    }

}