//@MCGameStudios 2016 
//Please do not remove this lines :)

using UnityEngine;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{

    public Camera mainCamera;

    public Block grassBlockPrefab;
    public Block dirtBlockPrefab;
    public Block cobbleBlockPrefab;
    public Block woodBlockPrefab;
    public Block leafBlockPrefab;
    public Block sandBlockPrefab;
    public Block brickBlockPrefab;
    public Block iceBlockPrefab;
    public Block flintBlockPrefab;
    public Block clayBlockPrefab;
    public Block snowBlockPrefab;
    public Block skyrockBlockPrefab;
    public Block flowerBlockPrefab;
    public Block grassitemBlockPrefab;
    public Block brickredBlockPrefab;
    public Block plankBlockPrefab;
    public Block glassBlockPrefab;
    public Block campBlockPrefab;
    public Block camprockBlockPrefab;



    public BlockLoader woodsContainer;
    public BlockLoader grassContainer;
    public BlockLoader dirtContainer;
    public BlockLoader cobbleContainer;
    public BlockLoader leafContainer;
    public BlockLoader sandContainer;
    public BlockLoader brickContainer;
    public BlockLoader iceContainer;
    public BlockLoader flintContainer;
    public BlockLoader clayContainer;
    public BlockLoader snowContainer;
    public BlockLoader skyrockContainer;
    public BlockLoader flowerContainer;
    public BlockLoader grassitemContainer;
    public BlockLoader brickredContainer;
    public BlockLoader plankContainer;
    public BlockLoader glassContainer;
    public BlockLoader campContainer;
    public BlockLoader camprockContainer;

    private BlockType selectedBlock;

    private List<Block> allblocks = new List<Block>() {};

    private bool isOn = false;
    // Use this for initialization
    void Start()
    {
        // Make the block type None at first (empty handed)
        selectedBlock = BlockType.None;
        // Load all blocks into an array (This isn't the best approach but works with your method so far)
        addBlocks();

        // GetComponent<Camera>();
    }

    /// <summary>
    /// Add all the blocks which are listed into the 'allblocks' array
    /// </summary>
    void addBlocks()
    {
        allblocks.Add(grassBlockPrefab);
        allblocks.Add(dirtBlockPrefab);
        allblocks.Add(cobbleBlockPrefab);
        allblocks.Add(woodBlockPrefab);
        allblocks.Add(leafBlockPrefab);
        allblocks.Add(sandBlockPrefab);
        allblocks.Add(brickBlockPrefab);
        allblocks.Add(iceBlockPrefab);
        allblocks.Add(flintBlockPrefab);
        allblocks.Add(clayBlockPrefab);
        allblocks.Add(snowBlockPrefab);
        allblocks.Add(skyrockBlockPrefab);
        allblocks.Add(flowerBlockPrefab);
        allblocks.Add(grassitemBlockPrefab);

        // Crafting blocks

        //allblocks.Add(brickredBlockPrefab);
        //allblocks.Add(plankBlockPrefab);
        //allblocks.Add(glassBlockPrefab);
        //allblocks.Add(campBlockPrefab);
        //allblocks.Add(camprockBlockPrefab);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.I))
        {
            isOn = !isOn;
        }

        if (!isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {

                Vector3 click_pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                // click_pos.z = 1;//-1
                Collider2D hit = Physics2D.OverlapPoint(click_pos);
                if (hit)
                {
                    Block b = hit.gameObject.GetComponent<Block>();
                    if (b)
                    {
                        b.OnHit();
                    }
                    // Debug.Log("pos not null: "+hit.gameObject.name+"\npos: "+click_pos);
                    return;
                }
                Vector3 pos = click_pos + new Vector3(0, 0, 10);
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 3);
                if (selectedBlock == BlockType.Dirt)
                {
                    if (SaveGame.Instance.data.dirtAmount > 0)
                    {
                        CreateBlock(dirtContainer, dirtBlockPrefab, pos);
                        SaveGame.Instance.data.placedDirts.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.dirtAmount--;
                    }
                }


                if (selectedBlock == BlockType.Grass)
                {
                    if (SaveGame.Instance.data.grassAmount > 0)
                    {
                        CreateBlock(grassContainer, grassBlockPrefab, pos);
                        SaveGame.Instance.data.placedGrass.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.grassAmount--;
                    }
                }
                if (selectedBlock == BlockType.Cobble)
                {
                    if (SaveGame.Instance.data.cobbleAmount > 0)
                    {
                        CreateBlock(cobbleContainer, cobbleBlockPrefab, pos);
                        SaveGame.Instance.data.placedCobbles.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.cobbleAmount--;
                    }
                    if (SaveGame.Instance.data.camprockAmount > 0)
                    {
                        SaveGame.Instance.data.camprockAmount--;
                    }
                }
                if (selectedBlock == BlockType.Wood)
                {
                    if (SaveGame.Instance.data.woodAmount > 0)
                    {
                        CreateBlock(woodsContainer, woodBlockPrefab, pos);
                        SaveGame.Instance.data.placedWoods.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.woodAmount--;
                    }
                    if (SaveGame.Instance.data.plankAmount > 0)
                    {
                        SaveGame.Instance.data.plankAmount--;
                    }

                    if (SaveGame.Instance.data.campAmount > 0)
                    {
                        SaveGame.Instance.data.campAmount--;
                    }
                }
                if (selectedBlock == BlockType.Leaf)
                {
                    if (SaveGame.Instance.data.leafAmount > 0)
                    {
                        CreateBlock(leafContainer, leafBlockPrefab, pos);
                        SaveGame.Instance.data.placedLeafs.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.leafAmount--;
                    }
                }
                //Crafting Glass
                if (selectedBlock == BlockType.Sand)
                {
                    if (SaveGame.Instance.data.sandAmount > 0)
                    {
                        CreateBlock(sandContainer, sandBlockPrefab, pos);
                        SaveGame.Instance.data.placedSands.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.sandAmount--;
                    }
                    if (SaveGame.Instance.data.glassAmount > 0)
                    {
                        SaveGame.Instance.data.glassAmount--;
                    }
                }
                if (selectedBlock == BlockType.Brick)
                {
                    if (SaveGame.Instance.data.brickAmount > 0)
                    {
                        CreateBlock(brickContainer, brickBlockPrefab, pos);
                        SaveGame.Instance.data.placedBricks.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.brickAmount--;
                    }
                }
                if (selectedBlock == BlockType.Ice)
                {
                    if (SaveGame.Instance.data.iceAmount > 0)
                    {
                        CreateBlock(iceContainer, iceBlockPrefab, pos);
                        SaveGame.Instance.data.placedIces.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.iceAmount--;
                    }
                }
                if (selectedBlock == BlockType.Flint)
                {
                    if (SaveGame.Instance.data.flintAmount > 0)
                    {
                        CreateBlock(flintContainer, flintBlockPrefab, pos);
                        SaveGame.Instance.data.placedFlints.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.flintAmount--;
                    }
                    if (SaveGame.Instance.data.camprockAmount > 0)
                    {
                        SaveGame.Instance.data.camprockAmount--;
                    }
                    if (SaveGame.Instance.data.campAmount > 0)
                    {
                        SaveGame.Instance.data.campAmount--;
                    }
                }
                if (selectedBlock == BlockType.Clay)
                {
                    if (SaveGame.Instance.data.clayAmount > 0)
                    {
                        CreateBlock(clayContainer, clayBlockPrefab, pos);
                        SaveGame.Instance.data.placedClays.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.clayAmount--;
                    }
                    if (SaveGame.Instance.data.brickredAmount > 0)
                    {
                        SaveGame.Instance.data.brickredAmount--;
                    }
                }
                if (selectedBlock == BlockType.Snow)
                {
                    if (SaveGame.Instance.data.snowAmount > 0)
                    {
                        CreateBlock(snowContainer, snowBlockPrefab, pos);
                        SaveGame.Instance.data.placedSnows.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.snowAmount--;
                    }
                }
                if (selectedBlock == BlockType.Skyrock)
                {
                    if (SaveGame.Instance.data.skyrockAmount > 0)
                    {
                        CreateBlock(skyrockContainer, skyrockBlockPrefab, pos);
                        SaveGame.Instance.data.placedSkyrocks.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.skyrockAmount--;
                    }
                }
                if (selectedBlock == BlockType.Flower)
                {
                    if (SaveGame.Instance.data.flowerAmount > 0)
                    {
                        CreateBlock(flowerContainer, flowerBlockPrefab, pos);
                        SaveGame.Instance.data.placedFlowers.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.flowerAmount--;
                    }
                }
                if (selectedBlock == BlockType.Grassitem)
                {
                    if (SaveGame.Instance.data.grassitemAmount > 0)
                    {
                        CreateBlock(grassitemContainer, grassitemBlockPrefab, pos);
                        SaveGame.Instance.data.placedGrassitems.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.grassitemAmount--;
                    }
                }
                //Brickred
                if (selectedBlock == BlockType.Brickred)
                {
                    if (SaveGame.Instance.data.clayAmount > 0)
                    {
                        SaveGame.Instance.data.clayAmount--;
                    }
                    if (SaveGame.Instance.data.brickredAmount > 0)
                    {
                        CreateBlock(brickredContainer, brickredBlockPrefab, pos);
                        SaveGame.Instance.data.placedBrickreds.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.brickredAmount--;
                    }
                }
                //Crafting of Plank.
                if (selectedBlock == BlockType.Plank)
                {
                    if (SaveGame.Instance.data.woodAmount > 0)
                    {
                        SaveGame.Instance.data.woodAmount--;
                    }
                    if (SaveGame.Instance.data.plankAmount > 0)
                    {
                        CreateBlock(plankContainer, plankBlockPrefab, pos);
                        SaveGame.Instance.data.placedPlanks.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.plankAmount--;
                    }
                }
                //Crafting of Glass. 
                if (selectedBlock == BlockType.Glass)
                {
                    if (SaveGame.Instance.data.sandAmount > 0)
                    {
                        SaveGame.Instance.data.sandAmount--;
                    }
                    if (SaveGame.Instance.data.glassAmount > 0)
                    {
                        CreateBlock(glassContainer, glassBlockPrefab, pos);
                        SaveGame.Instance.data.placedGlasss.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.glassAmount--;
                    }
                }
                if (selectedBlock == BlockType.Camp)
                {
                    if (SaveGame.Instance.data.woodAmount > 0)
                    {
                        SaveGame.Instance.data.woodAmount--;
                    }
                    if (SaveGame.Instance.data.flintAmount > 0)
                    {
                        SaveGame.Instance.data.flintAmount--;
                    }
                    if (SaveGame.Instance.data.campAmount > 0)
                    {
                        CreateBlock(campContainer, campBlockPrefab, pos);
                        SaveGame.Instance.data.placedCamps.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.campAmount--;
                    }
                }
                if (selectedBlock == BlockType.Camprock)
                {
                    if (SaveGame.Instance.data.cobbleAmount > 0)
                    {
                        SaveGame.Instance.data.cobbleAmount--;
                    }
                    if (SaveGame.Instance.data.flintAmount > 0)
                    {
                        SaveGame.Instance.data.flintAmount--;
                    }
                    if (SaveGame.Instance.data.camprockAmount > 0)
                    {
                        CreateBlock(campContainer, campBlockPrefab, pos);
                        SaveGame.Instance.data.placedCamps.Add(new SaveVector3(pos));
                        SaveGame.Instance.data.camprockAmount--;
                    }
                }

            }
        }
    }

    private void CreateBlock(BlockLoader container, Block prefab, Vector3 pos)
    {
        Block instanceBlock = (Block)Instantiate(prefab, pos, Quaternion.identity);
        instanceBlock.id = container.blocks.Count;
        container.blocks.Add(instanceBlock);
        instanceBlock.SetBlock(container.transform, prefab.name);
    }

    //Inventory GUI
    public Vector2 WindowPosition = Vector2.zero;
    public Vector2 WindowSize = new Vector2(100, 100);
    //Texture Inventory
    public Texture InventoryWindow;
    public Texture SelectedBlock;

    public Texture DirtText;
    public Texture GrassText;
    public Texture CobbleText;
    public Texture WoodText;
    public Texture LeafText;
    public Texture SandText;
    public Texture BrickText;
    public Texture IceText;
    public Texture FlintText;
    public Texture ClayText;
    public Texture SnowText;
    public Texture SkyrockText;
    public Texture FlowerText;
    public Texture GrassitemText;
    public Texture BrickredText;
    public Texture PlankText;
    public Texture GlassText;
    public Texture CampText;
    public Texture CamprockText;

    /// <summary>
    /// Because blocks are stored individually, for now we have data in different places, and we need to access each part
    /// </summary>
    /// <returns></returns>
    private object[] getBlockData(BlockType type)
    {
        object[] data = new object[2];

        data[0] = GlassText;
        data[1] = 5;

        switch (type)
        {
            case BlockType.Brick:
                data[0] = BrickText;
                data[1] = SaveGame.Instance.data.brickAmount;
                break;
            case BlockType.Clay:
                data[0] = ClayText;
                data[1] = SaveGame.Instance.data.clayAmount;
                break;
            case BlockType.Cobble:
                data[0] = CobbleText;
                data[1] = SaveGame.Instance.data.cobbleAmount;
                break;
            case BlockType.Dirt:
                data[0] = DirtText;
                data[1] = SaveGame.Instance.data.dirtAmount;
                break;
            case BlockType.Flint:
                data[0] = FlintText;
                data[1] = SaveGame.Instance.data.flintAmount;
                break;
            case BlockType.Flower:
                data[0] = FlowerText;
                data[1] = SaveGame.Instance.data.flowerAmount;
                break;
            case BlockType.Glass:
                data[0] = GlassText;
                data[1] = SaveGame.Instance.data.glassAmount;
                break;
            case BlockType.Grass:
                data[0] = GrassText;
                data[1] = SaveGame.Instance.data.grassAmount;
                break;
            case BlockType.Ice:
                data[0] = IceText;
                data[1] = SaveGame.Instance.data.iceAmount;
                break;
            case BlockType.Leaf:
                data[0] = LeafText;
                data[1] = SaveGame.Instance.data.leafAmount;
                break;
            case BlockType.Grassitem:
                data[0] = GrassitemText;
                data[1] = SaveGame.Instance.data.grassitemAmount;
                break;
            case BlockType.Wood:
                data[0] = WoodText;
                data[1] = SaveGame.Instance.data.woodAmount;
                break;
            case BlockType.Snow:
                data[0] = SnowText;
                data[1] = SaveGame.Instance.data.snowAmount;
                break;
            case BlockType.Sand:
                data[0] = SandText;
                data[1] = SaveGame.Instance.data.sandAmount;
                break;
            case BlockType.Skyrock:
                data[0] = SkyrockText;
                data[1] = SaveGame.Instance.data.skyrockAmount;
                break;
            case BlockType.None:

                break;
            default:
                throw new System.Exception("Failed to get block type " + type);
        }

        return data;
    }

    void OnGUI()
    {
        if (isOn)
        {
            GUI.BeginGroup(new Rect(WindowPosition.x, WindowPosition.y, WindowSize.x, WindowSize.y), InventoryWindow);

            GUI.EndGroup();

            // Dynamic block stuff here:

            // how many buttons are shown in a row
            int buttonRowLength = 5;
            int curX = 0;
            int curY = 0;

            // Dynamic button drawing
            for (int i = 0; i < allblocks.Count; i++)
            {
                BlockType type = allblocks[i].blockType;

                // Get the data from the block, its texture, how many are in the inventory, etc.
                object[] data = getBlockData(type);

                Texture texture = (Texture)data[0];
                int count = (int)data[1];

                if (count > 0)
                {
                    // Check for end of row, increment down further
                    if (curX == buttonRowLength)
                    {
                        curY++;
                        curX = 0;
                    }

                    curX++;

                    // Dynamically draw the block box
                    // The values in this are dynamic, so if you want to change them around you will be able to find out yourself which 
                    // look you like best
                    if (GUI.Button(new Rect(290 + (curX * 50.0f), 205 + (curY * 50.0f), 50, 50), new GUIContent("" + count, texture)))
                    {
                        selectedBlock = type;
                    }

                    // Is this block selected?
                    if (selectedBlock == type)
                    {
                        GUI.BeginGroup(new Rect(290 + (curX * 50.0f), 205 + (curY * 50.0f), 50, 50), SelectedBlock);
                        GUI.EndGroup();
                    }
                }
            }


            //if (selectedBlock == BlockType.Dirt)
            //{
            //    GUI.BeginGroup(new Rect(325, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Grass)
            //{
            //    GUI.BeginGroup(new Rect(375, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Cobble)
            //{
            //    GUI.BeginGroup(new Rect(425, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Wood)
            //{
            //    GUI.BeginGroup(new Rect(475, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Skyrock)
            //{
            //    GUI.BeginGroup(new Rect(525, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Leaf)
            //{
            //    GUI.BeginGroup(new Rect(325, 255, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Sand)
            //{
            //    GUI.BeginGroup(new Rect(375, 255, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Brick)
            //{
            //    GUI.BeginGroup(new Rect(425, 255, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Ice)
            //{
            //    GUI.BeginGroup(new Rect(475, 255, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Flint)
            //{
            //    GUI.BeginGroup(new Rect(525, 255, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Clay)
            //{
            //    GUI.BeginGroup(new Rect(325, 305, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Snow)
            //{
            //    GUI.BeginGroup(new Rect(375, 305, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}

            //if (selectedBlock == BlockType.Flower)
            //{
            //    GUI.BeginGroup(new Rect(425, 305, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Grassitem)
            //{
            //    GUI.BeginGroup(new Rect(475, 305, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Brickred)
            //{
            //    GUI.BeginGroup(new Rect(60, 255, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Plank)
            //{
            //    GUI.BeginGroup(new Rect(210, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Glass)
            //{
            //    GUI.BeginGroup(new Rect(160, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Camp)
            //{
            //    GUI.BeginGroup(new Rect(60, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}
            //if (selectedBlock == BlockType.Camprock)
            //{
            //    GUI.BeginGroup(new Rect(110, 205, 50, 50), SelectedBlock);

            //    GUI.EndGroup();
            //}

            // Inventory Items below ---
/*
            if (SaveGame.Instance.data.dirtAmount > 0 && GUI.Button(new Rect(325, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.dirtAmount, DirtText)))
            {
                selectedBlock = BlockType.Dirt;
            }

            if (SaveGame.Instance.data.grassAmount > 0 && GUI.Button(new Rect(375, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.grassAmount, GrassText)))
            {
                selectedBlock = BlockType.Grass;
            }
            if (SaveGame.Instance.data.cobbleAmount > 0 && GUI.Button(new Rect(425, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.cobbleAmount, CobbleText)))
            {
                selectedBlock = BlockType.Cobble;
            }
            if (SaveGame.Instance.data.woodAmount > 0 && GUI.Button(new Rect(475, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.woodAmount, WoodText)))
            {
                selectedBlock = BlockType.Wood;
            }
            if (SaveGame.Instance.data.skyrockAmount > 0 && GUI.Button(new Rect(525, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.skyrockAmount, SkyrockText)))
            {
                selectedBlock = BlockType.Skyrock;
            }

            if (SaveGame.Instance.data.leafAmount > 0 && GUI.Button(new Rect(325, 255, 50, 50), new GUIContent("" + SaveGame.Instance.data.leafAmount, LeafText)))
            {
                selectedBlock = BlockType.Leaf;
            }
            if (SaveGame.Instance.data.sandAmount > 0 && GUI.Button(new Rect(375, 255, 50, 50), new GUIContent("" + SaveGame.Instance.data.sandAmount, SandText)))
            {
                selectedBlock = BlockType.Sand;
            }

            if (SaveGame.Instance.data.brickAmount > 0 && GUI.Button(new Rect(425, 255, 50, 50), new GUIContent("" + SaveGame.Instance.data.brickAmount, BrickText)))
            {
                selectedBlock = BlockType.Brick;
            }

            if (SaveGame.Instance.data.iceAmount > 0 && GUI.Button(new Rect(475, 255, 50, 50), new GUIContent("" + SaveGame.Instance.data.iceAmount, IceText)))
            {
                selectedBlock = BlockType.Ice;
            }

            if (SaveGame.Instance.data.flintAmount > 0 && GUI.Button(new Rect(525, 255, 50, 50), new GUIContent("" + SaveGame.Instance.data.flintAmount, FlintText)))
            {
                selectedBlock = BlockType.Flint;
            }

            if (SaveGame.Instance.data.clayAmount > 0 && GUI.Button(new Rect(325, 305, 50, 50), new GUIContent("" + SaveGame.Instance.data.clayAmount, ClayText)))
            {
                selectedBlock = BlockType.Clay;
            }

            if (SaveGame.Instance.data.snowAmount > 0 && GUI.Button(new Rect(375, 305, 50, 50), new GUIContent("" + SaveGame.Instance.data.snowAmount, SnowText)))
            {
                selectedBlock = BlockType.Snow;
            }

            if (SaveGame.Instance.data.flowerAmount > 0 && GUI.Button(new Rect(425, 305, 50, 50), new GUIContent("" + SaveGame.Instance.data.flowerAmount, FlowerText)))
            {
                selectedBlock = BlockType.Flower;
            }

            if (SaveGame.Instance.data.grassitemAmount > 0 && GUI.Button(new Rect(475, 305, 50, 50), new GUIContent("" + SaveGame.Instance.data.grassitemAmount, GrassitemText)))
            {
                selectedBlock = BlockType.Grassitem;
            }*/

            // Crafting items below ---

            if (GUI.Button(new Rect(60, 255, 50, 50), new GUIContent("" + SaveGame.Instance.data.brickredAmount, BrickredText)))
            {
                selectedBlock = BlockType.Brickred;
            }

            if (GUI.Button(new Rect(210, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.plankAmount, PlankText)))
            {
                selectedBlock = BlockType.Plank;
            }

            if (GUI.Button(new Rect(160, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.glassAmount, GlassText)))
            {
                selectedBlock = BlockType.Glass;
            }

            if (GUI.Button(new Rect(60, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.campAmount, CampText)))
            {
                selectedBlock = BlockType.Camp;
            }

            if (GUI.Button(new Rect(110, 205, 50, 50), new GUIContent("" + SaveGame.Instance.data.camprockAmount, CamprockText)))
            {
                selectedBlock = BlockType.Camprock;
            }




        }
    }

    // private Block GetBlock()

}
