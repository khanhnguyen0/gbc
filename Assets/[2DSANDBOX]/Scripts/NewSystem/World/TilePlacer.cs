using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField]
    private InventoryController _inventory;
    /// <summary>
    /// Whether or not the placer can edit the terrain
    /// </summary>
    [SerializeField]
    private bool _canEdit;

    /// <summary>
    /// Set the state of the placement
    /// </summary>
    /// <param name="state"></param>
    public void toggleState(bool state)
    {
        _canEdit = state;
    }

    void Awake()
    {

    }
    
    void Start()
    {

    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        if (_canEdit && Input.GetMouseButtonDown(0)) // left click, hit with item
        {
            removeTile(pos);
        }
        else if(_canEdit && Input.GetMouseButtonDown(1)) // right click, use the item
        {
            // Send the right click 'event'
            World.instance.rightClick(pos);

            Stack stack = _inventory.getActiveItem();

            if(stack != null) // Is an item active?
            {
                // Check if the placement was successful, if not, refund the item back to the inventory
                if(stack.item.activateItem(stack))
                {
                    _inventory.takeItem();
                }
            }
        }
    }

    private void placeTile(Vector3 position)
    {
        World.instance.setBlock(new Tile(1, 0), position);
    }

    private void removeTile(Vector3 position)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            World.instance.setBackground(new Tile(0, 0), position);
        }
        else
        {
            World.instance.setBlock(new Tile(0, 0), position);
        }
    }

}