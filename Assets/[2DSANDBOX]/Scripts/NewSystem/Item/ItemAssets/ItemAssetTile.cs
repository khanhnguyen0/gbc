using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Block Item", menuName = "Items/Tile Item")]
public class ItemAssetTile : ItemAsset
{
    [SerializeField]
    private TileAsset _tileAsset;

    public TileAsset tileAsset { get { return _tileAsset; } }

    public override Item getItem()
    {
        return new ItemTile(id, _tileAsset);
    }
}
