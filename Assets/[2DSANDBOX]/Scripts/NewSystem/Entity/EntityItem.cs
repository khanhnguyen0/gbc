using UnityEngine;
using System.Collections;
using System.IO;

public class EntityItem : Entity
{
    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private int _itemID;

    [SerializeField]
    private float _life;

    public int itemID
    {
        get
        {
            return _itemID;
        }
        set
        {
            _itemID = value;
            updateVisuals();
        }
    }

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update the visual texture to comply with the texture of the item contained within this entity
    void updateVisuals()
    {
        ItemAsset asset = ItemStore.getItem(_itemID);
        Texture2D texture = asset.texture;

        Debug.Log("Asset " + asset.id + asset.name);

        _renderer.material.mainTexture = texture;
    }

    void Update()
    {
        _life -= Time.deltaTime;

        // Lifetime management
        if(_life < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EntityPlayer entity = other.gameObject.GetComponent<EntityPlayer>();

        // Is the entity we collided with a player?
        if (entity != null)
        {
            Debug.Log("Player");

            // Store the item in the inventory (TEMPORARILY SET TO 99999)
            ItemAsset item = ItemStore.getItem(_itemID);
            entity.addItem(item.getItem(), 1);

            // TODO:
            // Pooling behaviour, NEVER destroy like this
            Destroy(gameObject);
        }
    }

    public override void saveData(StreamWriter writer)
    {
        base.saveData(writer);

        writer.WriteLine(_itemID);
    }

    public override void loadData(StreamReader reader)
    {
        base.loadData(reader);

        _itemID = int.Parse(reader.ReadLine());
    }
}