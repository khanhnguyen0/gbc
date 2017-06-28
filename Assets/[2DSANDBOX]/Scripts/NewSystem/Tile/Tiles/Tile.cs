using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class Tile : ISaveable
{
    private int _id;
    private int _health;
    private TileAsset _asset;

    public int id { get { return _id; } }
    public int health { get { return _health; } }
    public TileAsset asset { get { return _asset; } }

    public Tile(int id, int health)
    {
        this._id = id;
        this._health = health;
        this._asset = TileStore.getTile(id);
    }

    public Tile(int id, int health, TileAsset asset)
    {
        this._id = id;
        this._health = health;
        this._asset = asset;
    }

    public virtual void saveData(StreamWriter writer)
    {
        // ID is parsed to the save data file
        writer.WriteLine(_id);
    }

    public virtual void loadData(StreamReader reader)
    {
        // ID value does not need to be re-loaded as it is used for initialization.
        // TODO : use default contstructor to assign ID in here instead.
    }

    public virtual bool isBlockSolid()
    {
        return _asset.solid;
    }

    public virtual string getTexture()
    {
        return _asset.texture.name;
    }

    public virtual void onRightClicked()
    {

    }

    public virtual void onPlaced()
    {

    }

    public virtual void onDestroyed(Vector2 position)
    {
        TileAsset tile = TileStore.getTile(id);

        if(tile.dropItem)
        {
            Debug.Log(tile);

            EntityItem item = (EntityItem)EntityStore.createEntity(0);

            Debug.Log(item);
            item.itemID = tile.droppedItem.id;

            item.transform.position = new Vector3(position.x - 0.5f, position.y - 0.5f, 1.0f);
        }
    }

    public virtual void decreaseHealth()
    {
        _health--;
    }
}