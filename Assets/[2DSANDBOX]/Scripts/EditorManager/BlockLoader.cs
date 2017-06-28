using System.Collections.Generic;
using UnityEngine;

public class BlockLoader : MonoBehaviour {
    public BlockType blockType = BlockType.None;
    public Block blockPrefab;
    [HideInInspector]
    public List<Block> blocks;
    private bool is_started = false;
    void Start(){
        if(!is_started){
            ForceStart();
        }
    }

    private void ForceStart(){
        blocks = new List<Block>(GetComponentsInChildren<Block>());
        blocks.Sort();
        for (int i = 0; i < blocks.Count; i++){
            blocks[i].id = i;
        }
        is_started = true;
        // Debug.Log("BlockLoader "+blockType+" force started, blocks length: "+blocks.Count);
    }

    public void ClearClickedBlocks(){
        if(!is_started){
            ForceStart();
        }
        // Debug.Log("try clear "+blockType);
        switch (blockType){
            case BlockType.Dirt:
                ClearBlocks(SaveGame.Instance.data.deletedDirt);
            break;
            case BlockType.Grass:
                ClearBlocks(SaveGame.Instance.data.deletedGrass);
            break;
            case BlockType.Cobble:
                ClearBlocks(SaveGame.Instance.data.deletedCobble);
            break;
            case BlockType.Wood:
                // Debug.Log("wood savedata count: "+SaveGame.Instance.data.deletedWood.Count);
                ClearBlocks(SaveGame.Instance.data.deletedWood);
            break;
            case BlockType.Leaf:
                ClearBlocks(SaveGame.Instance.data.deletedLeaf);
            break;
            case BlockType.Sand:
                ClearBlocks(SaveGame.Instance.data.deletedSand);
            break;
            case BlockType.Brick:
                ClearBlocks(SaveGame.Instance.data.deletedBrick);
                break;
            case BlockType.Ice:
                ClearBlocks(SaveGame.Instance.data.deletedIce);
                break;
            case BlockType.Flint:
                ClearBlocks(SaveGame.Instance.data.deletedFlint);
                break;
            case BlockType.Clay:
                ClearBlocks(SaveGame.Instance.data.deletedClay);
                break;
            case BlockType.Snow:
                ClearBlocks(SaveGame.Instance.data.deletedSnow);
                break;
            case BlockType.Skyrock:
                ClearBlocks(SaveGame.Instance.data.deletedSkyrock);
                break;
            case BlockType.Flower:
                ClearBlocks(SaveGame.Instance.data.deletedFlower);
                break;
            case BlockType.Grassitem:
                ClearBlocks(SaveGame.Instance.data.deletedGrassitem);
                break;
            case BlockType.Brickred:
                ClearBlocks(SaveGame.Instance.data.deletedBrickred);
                break;
            case BlockType.Plank:
                ClearBlocks(SaveGame.Instance.data.deletedPlank);
                break;
            case BlockType.Glass:
                ClearBlocks(SaveGame.Instance.data.deletedGlass);
                break;
            case BlockType.Camp:
                ClearBlocks(SaveGame.Instance.data.deletedCamp);
                break;
            case BlockType.Camprock:
                ClearBlocks(SaveGame.Instance.data.deletedCamprock);
                break;
            default:
            break;
        }
    }

    private void ClearBlocks(List<int> saved_data){
        List<GameObject> to_be_destroyed = new List<GameObject>();
        for (int j = 0; j < blocks.Count; j++){
            if(saved_data.Contains(blocks[j].id)){
                to_be_destroyed.Add(blocks[j].gameObject);
            }
        }
        to_be_destroyed.ForEach(go=>Destroy(go));
        // Debug.Log("total "+blockType+" to be destroyed: "+to_be_destroyed.Count);
    }

    public void CreateBlocks(){
        switch (blockType){
            case BlockType.Dirt:
                CreatePlacedBlocks(SaveGame.Instance.data.placedDirts);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedDirts);
            break;
            case BlockType.Grass:
                CreatePlacedBlocks(SaveGame.Instance.data.placedGrass);                
                CreatePickableBlocks(SaveGame.Instance.data.unpickedGrass);
            break;
            case BlockType.Cobble:
                CreatePlacedBlocks(SaveGame.Instance.data.placedCobbles);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedCobbles);
            break;
            case BlockType.Wood:
                CreatePlacedBlocks(SaveGame.Instance.data.placedWoods);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedWoods);
            break;
            case BlockType.Leaf:
                CreatePlacedBlocks(SaveGame.Instance.data.placedLeafs);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedLeafs);
            break;
            case BlockType.Sand:
                CreatePlacedBlocks(SaveGame.Instance.data.placedSands);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedSands);
             break;
            case BlockType.Brick:
               CreatePlacedBlocks(SaveGame.Instance.data.placedBricks);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedBricks);
                break;
            case BlockType.Ice:
                CreatePlacedBlocks(SaveGame.Instance.data.placedIces);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedIces);
                break;
            case BlockType.Flint:
                CreatePlacedBlocks(SaveGame.Instance.data.placedFlints);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedFlints);
                break;
            case BlockType.Clay:
                CreatePlacedBlocks(SaveGame.Instance.data.placedClays);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedClays);
                break;
            case BlockType.Snow:
                CreatePlacedBlocks(SaveGame.Instance.data.placedSnows);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedSnows);
                break;
            case BlockType.Skyrock:
                CreatePlacedBlocks(SaveGame.Instance.data.placedSkyrocks);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedSkyrocks);
                break;
            case BlockType.Flower:
                CreatePlacedBlocks(SaveGame.Instance.data.placedFlowers);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedFlowers);
                break;
            case BlockType.Grassitem:
                CreatePlacedBlocks(SaveGame.Instance.data.placedGrassitems);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedGrassitems);
                break;
            case BlockType.Brickred:
                CreatePlacedBlocks(SaveGame.Instance.data.placedBrickreds);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedBrickreds);
                break;
            case BlockType.Plank:
                CreatePlacedBlocks(SaveGame.Instance.data.placedPlanks);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedPlanks);
                break;
            case BlockType.Glass:
                CreatePlacedBlocks(SaveGame.Instance.data.placedGlasss);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedGlasss);
                break;
            case BlockType.Camp:
                CreatePlacedBlocks(SaveGame.Instance.data.placedCamps);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedCamps);
                break;
            case BlockType.Camprock:
                CreatePlacedBlocks(SaveGame.Instance.data.placedCamprocks);
                CreatePickableBlocks(SaveGame.Instance.data.unpickedCamprocks);
                break;
               
            default:
            break;
        }
    }

    private void CreatePickableBlocks(List<SaveVector3> block_pos){
        for (int i = 0; i < block_pos.Count; i++){
            Block block = (Block)Instantiate(blockPrefab, block_pos[i].Vector3(), Quaternion.identity);
            block.ReadyForPickupable();
        }
    }

    private void CreatePlacedBlocks(List<SaveVector3> block_pos){
        for (int i = 0; i < block_pos.Count; i++){
            Block instanceBlock = (Block) Instantiate(blockPrefab, block_pos[i].Vector3(), Quaternion.identity);
            instanceBlock.id = blocks.Count;
            blocks.Add(instanceBlock);
            instanceBlock.SetBlock(transform, blockPrefab.name);
        }
    }
}