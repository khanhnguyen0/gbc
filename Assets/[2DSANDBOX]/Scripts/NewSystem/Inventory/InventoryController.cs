using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class InventoryController : MonoBehaviour,  ISaveable
{
    [SerializeField]
    private Inventory _inventory;
    [SerializeField]
    private bool _open;

    [SerializeField]
    private Rect _windowRect;

    [SerializeField]
    private Vector2 _windowSize;
    [SerializeField]
    private Vector2 _inventoryOffset;
    [SerializeField]
    private Vector2 _craftingOffset;

    [SerializeField]
    private Vector2 _windowBounds;

    [SerializeField]
    private Texture2D _background;
    [SerializeField]
    private Texture2D _selectionSquare;
    [SerializeField]
    private int _selectedItemIndex;

    [SerializeField]
    private int _inventoryRowLength;
    [SerializeField]
    private int _craftingRowLength;
    [SerializeField]
    private int _spacingX, _spacingY;
    [SerializeField]
    private int _buttonSize;

    [SerializeField]
    private GUISkin style;


    private List<ItemAsset> availableRecipies;
    public Inventory inventory { get { return _inventory; } }
    public bool isOpen { get { return _open; } }

    /// <summary>
    /// Toggle the state of the inventory
    /// </summary>
    public void toggleState()
    {
        _open = !_open;

        // Hacky but a way to determine inventory and the recipes currently available
        if (_open == true)
        {
            rebuildRecipes();
        }
    }

    private void rebuildRecipes()
    {
        List<RecipeAsset> recipies = RecipeManager.Recipies;
        List<Stack> items = _inventory.items;
		//Debug.Log("aaaaaaaaaaaaaaa"+ items.Count);
        availableRecipies.Clear();

        Debug.Log(recipies[0].Requirements.Length);

        Debug.Log(recipies.Count + "," + items.Count);
        //return;

        // Iterate and find the matching recipies in this inventory
        for (int i = 0; i < recipies.Count; i++)
        {
            RecipeAsset recipe = recipies[i];
            RecipeAsset.Require[] elements = recipe.Requirements;
            bool recipeSuccess = false;

            // Iterate over all required items in the recipe
            for (int j = 0; j < elements.Length; j++)
            {
                bool success = false;

                // Compare for items in the recipe requirements
                for (int k = 0; k < items.Count; k++)
                {
                    int id = items[k].item.id;

                    // The recipe has a match to this item, break out
                    if (elements[j].itemAsset.id != id)
                    {
                        success = true;
                        break;
                    }
                }

                // Did the specified item fail to match up?
                if (!success)
                {
                    recipeSuccess = false;
                    break;
                }
                else
                {
                    recipeSuccess = true;
                }
            }

            // Recipe has succeeded in matching all its elements, 
            if(recipeSuccess)
            {
				if(availableRecipies.Count<7)
                availableRecipies.Add(recipe.Result);
            }
        }
    }

    public void addItem(Stack stack, int count)
    {
        if(stack != null)
        {
            inventory.addItem(stack.item, count);
        }
    }

    public void addItem(Item item, int count)
    {
        if(item != null)
        {
            inventory.addItem(item, count);
        }
    }

    public Stack getActiveItem()
    {
        return inventory.activeStack;
    }

    public Stack takeItem()
    {
        Stack stack = inventory.activeStack;
        if(stack != null)
        {
            // Check for a valid item count
            if(stack.count > 0)
            {
                //Debug.Log("Before: " + stack.count);
                stack.removeItem(1);
                //Debug.Log("After: " + stack.count);

                // Catch the situation where the stack goes empty
                if (stack.count == 0)
                {
                    inventory.removeItem(inventory.activeStack);
                }

                return stack;
            }
            else // The stack is empty for some reason, Nullify return and delete the stack
            {
                inventory.removeItem(inventory.activeStack);
                return null;
            }
        }
        else // The stack was NULL for some reason
        {
            return null;
        }
    }

    void Awake()
    {
        availableRecipies = new List<ItemAsset>();
        rebuildRecipes();
        updateGUIDimensions();
    }

    void updateGUIDimensions()
    {
        Vector2 screenDim = new Vector2(Screen.width, Screen.height);

        float midX = Screen.width * 0.5f;
        float midY = Screen.height * 0.5f;

        // Get the size in pixels of the GUI window as it's represented in 0.0-1.0 normalized screen space.
        float mX = screenDim.x * _windowBounds.x;
        float mY = screenDim.y * _windowBounds.y;

        float halfX = mX * 0.5f;
        float halfY = mY * 0.5f;

        Vector2 corn = new Vector2(midX - halfX, midY - halfY);
        _windowRect = new Rect(corn.x, corn.y, mX, mY);
    }

    void Start()
    {
        // TEMPORARILY ADD DEFAULT ITEMS
		List<Stack> items = _inventory.items;

		if (!SaveGame.Instance.IsSaveGame)
		{			
			items.Clear ();

		}

        int count = ItemStore.storeCount;
        for(int i = 0; i < count; i++)
        {
           // _inventory.addItem(ItemStore.getItem(i).getItem(), 999);
        }
        
        /*_inventory.addItem(ItemStore.getItem(0).getItem(), 99999);
        _inventory.addItem(ItemStore.getItem(1).getItem(), 99999);
        _inventory.addItem(ItemStore.getItem(2).getItem(), 99999);
        _inventory.addItem(ItemStore.getItem(3).getItem(), 99999);
        _inventory.addItem(ItemStore.getItem(4).getItem(), 99999);
        _inventory.addItem(ItemStore.getItem(5).getItem(), 99999);
        _inventory.addItem(ItemStore.getItem(6).getItem(), 99999);
        _inventory.addItem(ItemStore.getItem(7).getItem(), 99999);*/
    }




    void Update()
    {
        updateGUIDimensions();

        if (Input.GetKeyDown(KeyCode.I))
        {
            //_open = !_open;
        }
        if (_open && Input.GetKeyDown(KeyCode.Escape)){

            _open = !_open;

        }
    }

    void OnGUI()
    {
        if (_open)
        {

            
            GUI.skin = style;
            List<Stack> items = _inventory.items;
			//Nilupul

            // Draw the outlining window for the inventory
            GUI.BeginGroup(_windowRect, _background);

            GUI.EndGroup();

            int x = 0;
            int y = 0;
            
            // Draw Main inventory window
            for (int i = 0; i < items.Count; i++)
            {
                if (x == _inventoryRowLength)
                {
                    x = 0;
                    y++;
                }

                Rect rect = new Rect(new Vector2(_windowRect.x + ((_spacingX + _buttonSize) * x), _windowRect.y + ((_spacingY + _buttonSize) * y)) + _inventoryOffset, new Vector2(_buttonSize, _buttonSize));
                ItemAsset item = ItemStore.getItem(items[i].item.id);

                // Draw the blocks texture
                GUI.DrawTexture(rect, item.texture);

                // Draw the UI element
                if (GUI.Button(rect, ""))
                {
                    _inventory.setActiveItem(i);
                }

                // Draw the selection square
                if (i == _inventory.activeIndex)
                {
                    GUI.DrawTexture(rect, _selectionSquare);
                }

                // Draw the quantity
                GUI.Label(rect, ""+ items[i].count);

                x++;
            }

            x = 0;
            y = 0;

            // Draw Crafting inventory window
            for (int i = 0; i < availableRecipies.Count; i++)
            {
                if (x == _craftingRowLength)
                {
                    x = 0;
                    y++;
                }

                Rect rect = new Rect(new Vector2(_windowRect.x + ((_spacingX + _buttonSize) * x), _windowRect.y + ((_spacingY + _buttonSize) * y)) + _craftingOffset, new Vector2(_buttonSize, _buttonSize));
                ItemAsset item = availableRecipies[i];

                // Draw the blocks texture
                GUI.DrawTexture(rect, item.texture);

                // Draw the UI element
                if (GUI.Button(rect, ""))
                {
                    // Temporarily add 99999 of that specified item to the inventory

                    _inventory.addItem(item.getItem(), 1);
                    // Re-build any recipes which can be now created.
                    rebuildRecipes();
                }

                x++;
            }

        }
    }

    public void saveData(StreamWriter writer)
    {
        _inventory.saveData(writer);
    }

    public void loadData(StreamReader reader)
    {
        _inventory.loadData(reader);
    }
}