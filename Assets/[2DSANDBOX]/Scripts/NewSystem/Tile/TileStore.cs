using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class TileStore
{
    private const string _path = "Tiles/";
    private static Dictionary<int,TileAsset> _tiles;
    private static bool _initialized;

    public static Dictionary<int,TileAsset> tiles { get { return _tiles; } }

    public static void initialize()
    {
        if (_initialized)
        {
            //throw new System.Exception("Already initialized!");
        }
        else
        {
            _initialized = true;
            _tiles = new Dictionary<int, TileAsset>();

            TileAsset[] data = Resources.LoadAll<TileAsset>(_path);
            Debug.Log("Loading " + data.Length + " tiles.");

            // Index the tiles into the dictionary
            for(int i = 0; i < data.Length; i++)
            {
                _tiles.Add(data[i].id, data[i]);
            }
        }
    }

    public static Tile createTile(int id)
    {
        return _tiles[id].getTile();
    }

    public static TileAsset getTile(int id)
    {
        return _tiles[id];
    }
}