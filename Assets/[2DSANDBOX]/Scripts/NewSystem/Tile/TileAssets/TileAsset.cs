using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tiles/Tile")]
public class TileAsset : ScriptableObject
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private Texture2D _texture;
    [SerializeField]
    private bool _specialTile;
    [SerializeField]
    private bool _backgroundTile;
    [SerializeField]
    private string _displayName;
    [SerializeField]
    private bool _visible;
    [SerializeField]
    private bool _solid;
    [SerializeField]
    private bool _opaque;
    [SerializeField]
    private bool _dropItem;
    [SerializeField]
    private ItemAssetTile _droppedItem;

    public int id { get { return _id; } }
    public int maxHealth { get { return _maxHealth; } }
    public Texture2D texture { get { return _texture; } }
    public bool specialTile { get { return _specialTile; } }
    public bool backgroundTile { get { return _backgroundTile; } }
    public string displayName { get { return _displayName; } }
    public bool visible { get { return _visible; } }
    public bool solid { get { return _solid; } }
    public bool opaque { get { return _opaque; } }
    public bool dropItem { get { return _dropItem; } }
    public ItemAssetTile droppedItem { get { return _droppedItem; } }

    public virtual Tile getTile()
    {
        return new Tile(id, maxHealth, this);
    }
}
