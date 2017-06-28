using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

[CreateAssetMenu(fileName = "New Door", menuName = "Tiles/Door")]
public class TileAssetDoor : TileAsset
{
    [SerializeField]
    private Texture2D _doorClosed;

    public Texture2D doorClosed { get { return _doorClosed; } }

    public override Tile getTile()
    {
        return new TileDoor(id, maxHealth, this);
    }
}