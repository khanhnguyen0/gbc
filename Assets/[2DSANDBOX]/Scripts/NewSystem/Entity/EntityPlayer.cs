using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class EntityPlayer : Entity
{
    // Inventory container reference
    [SerializeField]
    private InventoryController _inventory;
    [SerializeField]
    private TilePlacer _placer;
    private bool _canOpenInventory;

    public bool canOpenInventory
    {
        get { return _canOpenInventory; }
        set
        {
            _canOpenInventory = value;
            _placer.toggleState(value);
        }
    }

    public void addItem(Item item, int count)
    {
        _inventory.addItem(item, count);
    }

    public virtual void Awake()
    {
        _inventory = GetComponent<InventoryController>();
        _placer = GetComponent<TilePlacer>();

        //load();
    }

    public virtual void Start()
    {
        // Tell the placer it can place blocks.
        _placer.toggleState(true);
    }

    public virtual void Update()
    {
        // Inventory Management ---

        if (Input.GetKeyDown(KeyCode.O) && _canOpenInventory)
        {
            _inventory.toggleState();
            bool state = _inventory.isOpen;
            _placer.toggleState(!state);
        }

        // End of Inventory Management ---
    }

    public override void loadData(StreamReader reader)
    {
        base.loadData(reader);

        _inventory.loadData(reader);
    }

    public override void saveData(StreamWriter writer)
    {
        base.saveData(writer);

        _inventory.saveData(writer);
    }
}