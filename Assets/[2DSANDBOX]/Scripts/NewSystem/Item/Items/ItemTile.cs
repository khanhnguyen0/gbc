using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class ItemTile : Item
{
    private TileAsset _tileAsset;

    public ItemTile(int id, TileAsset tileAsset) : base(id)
    {
        _tileAsset = tileAsset;
    }

    public override bool activateItem(Stack stack)
    {
        Tile tile = TileStore.createTile(_tileAsset.id);
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        // Background tiles are placed in the background
        if (_tileAsset.backgroundTile)
        {
            if (World.instance.backgroundEmpty(pos))
            {
                World.instance.setBackground(tile, pos);
                return true;
            }
        }
        else // Foreground tile
        {
            if (World.instance.blockEmpty(pos))
            {
                World.instance.setBlock(tile, pos);
                return true;
            }
        }

        return false;
    }
}