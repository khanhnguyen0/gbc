using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class ItemStore
{
    private const string _path = "Items/";
    private static Dictionary<int, ItemAsset> _items;
    private static bool _initialized;

    public static int storeCount { get { return _items.Count; } }

    public static void initialize()
    {
        if(_initialized)
        {
            //throw new Exception("Already initialized!");
        }
        else
        {
            _initialized = true;
            _items = new Dictionary<int, ItemAsset>();
            loadItems();
        }
    }

    private static void loadItems()
    {
        UnityEngine.Object[] data = Resources.LoadAll(_path);
        Debug.Log("Loading " + data.Length + " items.");

        // Index the tiles into the dictionary
        for (int i = 0; i < data.Length; i++)
        {
            ItemAsset asset = (ItemAsset)data[i];
            _items.Add(asset.id, asset);
            
           // _items.Add(data[i].id, data[i]);
        }
    }

    public static Item createItem(int id)
    {
        return _items[id].getItem();
    }

    public static ItemAsset getItem(int id)
    {
        return _items[id];
    }

}