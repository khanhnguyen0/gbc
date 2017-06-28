using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemAsset : ScriptableObject
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _displayName;
    [SerializeField]
    private Texture2D _texture;
    [SerializeField]
    private Item _item;

    public int id { get { return _id; } }
    public string displayName { get { return _displayName; } }
    public Texture2D texture { get { return _texture; } }

    public virtual Item getItem()
    {
        return new Item(_id);
    }
}