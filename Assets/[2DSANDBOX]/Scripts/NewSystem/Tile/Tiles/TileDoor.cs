using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class TileDoor : Tile
{
    /// <summary>
    /// The state of the door,
    /// 0: Closed
    /// 1: Open
    /// </summary>
    private int _state;
    /// <summary>
    /// Re-casted asset reference
    /// </summary>
    private TileAssetDoor _assetN;

    public TileDoor(int id, int health, TileAsset asset) : base(id, health, asset)
    {
        _state = 1;
        _assetN = (TileAssetDoor)asset;
    }

    public override void saveData(StreamWriter writer)
    {
        base.saveData(writer);
        writer.WriteLine(_state);
    }

    public override void loadData(StreamReader reader)
    {
        base.loadData(reader);
        _state = int.Parse(reader.ReadLine());
    }

    public override void onRightClicked()
    {
        _state = _state == 1 ? 0 : 1;
    }

    public override bool isBlockSolid()
    {
        // Doors will not be solid once they are opened
        return _state == 0 ? true : false;
    }

    public override string getTexture()
    {
        // Differentiate between CLOSED and OPEN texture
        return _state == 0 ? _assetN.doorClosed.name : _assetN.texture.name;
    }
}
