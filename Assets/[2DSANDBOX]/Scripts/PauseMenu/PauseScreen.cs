using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class PauseScreen : MonoBehaviour
{
    public BlockManager blockManager;
    public GameObject pauseMenu, options;
    public GameObject startPanel, recepiesPanel, profilePanel, optionsPanel, quitPanel;
    public Image playerImage;
    public Text playerName;
    public Image playerLife;
    public Button prevRecipe, nextRecipe;
    public Button createRecipeButton;
    public Image[] recipeRequirements;
    public EquipmentSlot[] equipmentSlots;
    public Image mouseImage;
    public Text mouseText;
    int mouseId;
    int mouseCount;
    bool mouseSelected;

    bool pauseEnabled;
    int currentRecipe;

    EquipmentSlot selectedSlot;

    public Button load;
    public Button save;

    // Hook into world reference
    [SerializeField]
    private World _world;
    private EntityPlayer _player;

    void Start()
    {
        _player = _world.playerObj.GetComponent<EntityPlayer>();
        _player.canOpenInventory = true;

        if (SaveGame.Instance.IsSaveGame)
            loadGame();

        save.onClick.AddListener(() => { saveGame(); });
        //load.onClick.AddListener(()=>{SaveGame.Instance.Load();});
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.I))
        {
            if (!pauseEnabled)
            {
                //blockManager.enabled = false;

                Time.timeScale = 0;
                _player.canOpenInventory = false;

                startPanel.SetActive(true);
                recepiesPanel.SetActive(false);
                profilePanel.SetActive(false);
                optionsPanel.SetActive(false);
                quitPanel.SetActive(false);

                UpdateEquipment();
                pauseMenu.SetActive(true);
                pauseEnabled = true;
            }
            else
            {
                ResumeGame();
            }
        }

        if (mouseImage.enabled)
        {
            mouseImage.transform.position = Input.mousePosition;
        }
    }

    public void ResumeGame()
    {
        //blockManager.enabled = true;

        if (mouseImage.enabled)
            return;

        _player.canOpenInventory = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pauseEnabled = false;
    }

    public void PauseOptions()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        options.SetActive(!options.activeSelf);
    }

    public void ExitGame()
    {
        //saveGame();
        Application.Quit();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

        //blockManager.enabled = true;

        _player.canOpenInventory = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void saveGame()
    {
        SaveEquipment();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerPrefs.SetFloat("x", player.transform.position.x);
        PlayerPrefs.SetFloat("y", player.transform.position.y);
        PlayerPrefs.SetFloat("z", player.transform.position.z);

        // Tell the map to save
        _world.saveMap();

        StreamWriter writer = null;

        // Saving inventory
        try
        {
            writer = new StreamWriter(Application.dataPath + "/inventory.inv");
            _player.saveData(writer);
        }
        catch
        {
            Debug.Log("Failed to save inventory!");
        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
            }
        }
    }

    public void loadGame()
    {
        LoadEquipment();

        StreamReader reader = null;
        string path = Application.dataPath + "/inventory.inv";

        // Loading inventory
        if (File.Exists(path))
        {
            try
            {
                reader = new StreamReader(path);
                _player.loadData(reader);
            }
            catch
            {
                Debug.Log("Failed to save inventory!");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        else // Inventory file not found, save new one after loading
        {

        }
    }

    public void RecepiesButton()
    {
        startPanel.SetActive(false);
        recepiesPanel.SetActive(true);
        profilePanel.SetActive(false);
        optionsPanel.SetActive(false);
        quitPanel.SetActive(false);
        RefreshRecipe();
    }

    public void ProfileButton()
    {
        startPanel.SetActive(false);
        recepiesPanel.SetActive(false);
        profilePanel.SetActive(true);
        optionsPanel.SetActive(false);
        quitPanel.SetActive(false);

        SpriteRenderer playerSpriteRenderer = _world.playerObj.GetComponent<SpriteRenderer>();
        playerImage.sprite = playerSpriteRenderer.sprite;
        playerImage.color = playerSpriteRenderer.color;
        playerName.text = PlayerPrefs.GetString("PlayerName");
        Heart heart = GameObject.FindObjectOfType<Heart>(); //_world.playerObj.GetComponent<Character>().heart;
        playerLife.sprite = heart.heart_life.sprite;
    }

    public void OptionsButton()
    {
        startPanel.SetActive(false);
        recepiesPanel.SetActive(false);
        profilePanel.SetActive(false);
        optionsPanel.SetActive(true);
        quitPanel.SetActive(false);
    }

    public void QuitButton()
    {
        startPanel.SetActive(false);
        recepiesPanel.SetActive(false);
        profilePanel.SetActive(false);
        optionsPanel.SetActive(false);
        quitPanel.SetActive(true);
    }

    public void RecepiesPrevButton()
    {
        currentRecipe--;
        RefreshRecipe();
    }

    public void RecepiesNextButton()
    {
        currentRecipe++;
        RefreshRecipe();
    }

    void RefreshRecipe()
    {
        prevRecipe.interactable = currentRecipe > 0;
        nextRecipe.interactable = currentRecipe < RecipeManager.Recipies.Count - 1;

        if (currentRecipe >= 0 && currentRecipe < RecipeManager.Recipies.Count)
        {
            RecipeAsset recipe = RecipeManager.Recipies[currentRecipe];
            Inventory inventory = _world.playerObj.GetComponent<InventoryController>().inventory;
            bool canCreate = true;

            for (int i = 0; i < recipe.Requirements.Length; i++)
            {
                bool haveItems = false;

                for (int j = 0; j < inventory.items.Count; j++)
                {
                    if (inventory.items[j].item.id == recipe.Requirements[i].itemAsset.id)
                    {
                        if (inventory.items[j].count >= recipe.Requirements[i].quantity)
                            haveItems = true;

                        break;
                    }
                }

                if (!haveItems)
                    canCreate = false;
            }

            Texture2D tex = recipe.Result.texture;
            createRecipeButton.image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            createRecipeButton.gameObject.SetActive(canCreate);

            for (int i = 0; i < recipeRequirements.Length; i++)
            {
                if (i < recipe.Requirements.Length)
                {
                    Texture2D requirementTexture = recipe.Requirements[i].itemAsset.texture;
                    recipeRequirements[i].gameObject.SetActive(true);
                    recipeRequirements[i].sprite = Sprite.Create(requirementTexture, new Rect(0, 0, requirementTexture.width, requirementTexture.height), Vector2.zero);

                    int quantity = recipe.Requirements[i].quantity;
                    string quantityText;
                    quantityText = quantity > 1 ? quantity.ToString() : "";
                    recipeRequirements[i].transform.FindChild("Quantity").GetComponent<Text>().text = quantityText;
                }
                else
                    recipeRequirements[i].gameObject.SetActive(false);
            }
        }
        else
        {
            createRecipeButton.gameObject.SetActive(false);

            for (int i = 0; i < recipeRequirements.Length; i++)
                recipeRequirements[i].gameObject.SetActive(false);
        }
    }

    public void CreateRecipeButton()
    {
        RecipeAsset recipe = RecipeManager.Recipies[currentRecipe];
        Inventory inventory = _world.playerObj.GetComponent<InventoryController>().inventory;

        for (int i = 0; i < recipe.Requirements.Length; i++)
            inventory.removeItem(new Item(recipe.Requirements[i].itemAsset.id), recipe.Requirements[i].quantity);

        inventory.addItem(new ItemTile(recipe.Result.id, recipe.Result.tileAsset), 1);

        RefreshRecipe();
        UpdateEquipment();
    }

    public void UpdateEquipment()
    {
        Inventory inventory = _world.playerObj.GetComponent<InventoryController>().inventory;

        for (int i = 0; i < inventory.items.Count; i++)
        {
            bool haveSlot = false;

            for (int j = 0; j < equipmentSlots.Length; j++)
            {
                EquipmentSlot slot = equipmentSlots[j];

                if (inventory.items[i].item.id == slot.GetId() && slot.GetCount() > 0)
                {
                    haveSlot = true;
                    slot.SetCount(inventory.items[i].count);

                    if (inventory.activeIndex == i)
                        slot.Select();

                    break;
                }
            }

            if (!haveSlot)
            {
                for (int j = 0; j < equipmentSlots.Length; j++)
                {
                    EquipmentSlot slot = equipmentSlots[j];

                    if (slot.GetCount() == 0)
                    {
                        slot.SetId(inventory.items[i].item.id);
                        slot.SetCount(inventory.items[i].count);

                        if (inventory.activeIndex == i)
                            slot.Select();

                        break;
                    }
                }
            }
        }

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            EquipmentSlot slot = equipmentSlots[i];
            bool resetSlot = true;

            for (int j = 0; j < inventory.items.Count; j++)
            {
                if (inventory.items[j].item.id == slot.GetId() && inventory.items[j].count > 0 && slot.GetCount() > 0)
                {
                    resetSlot = false;
                    break;
                }
            }

            if (resetSlot)
            {
                slot.SetCount(0);
                slot.Deselect();
            }
        }      
    }

    public void SlotClick()
    {
        EquipmentSlot slot = EventSystem.current.currentSelectedGameObject.GetComponent<EquipmentSlot>();

        if (slot.GetCount() > 0 && !mouseImage.enabled)
        {
            Texture2D tex = ItemStore.getItem(slot.GetId()).texture;
            mouseImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            mouseImage.enabled = true;
            mouseId = slot.GetId();
            mouseCount = slot.GetCount();
            mouseText.text = mouseCount > 1 ? mouseCount.ToString() : "";
            mouseSelected = selectedSlot == slot;
            slot.SetCount(0);
            slot.Deselect();
        }
        else if(mouseImage.enabled && slot.GetCount() == 0)
        {
            mouseImage.enabled = false;
            slot.SetId(mouseId);
            slot.SetCount(mouseCount);
            mouseText.text = "";

            if (mouseSelected)
                slot.Select();
        }
    }

    public void SetActiveItem(EquipmentSlot slot)
    {
        Inventory inventory = _world.playerObj.GetComponent<InventoryController>().inventory;

        for (int i = 0; i < inventory.items.Count; i++)
            if (inventory.items[i].item.id == slot.GetId())
            {
                inventory.setActiveItem(i);
                break;
            }

        if (selectedSlot != null)
            selectedSlot.Deselect();

        selectedSlot = slot;
    }

    void SaveEquipment()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            EquipmentSlot slot = equipmentSlots[i];
            PlayerPrefs.SetInt("eqcount" + i, slot.GetCount());
            PlayerPrefs.SetInt("eqid" + i, slot.GetId());
        }
    }

    void LoadEquipment()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            EquipmentSlot slot = equipmentSlots[i];
            slot.SetCount(PlayerPrefs.GetInt("eqcount" + i, 0));
            slot.SetId(PlayerPrefs.GetInt("eqid" + i, 0));
        }
    }
}