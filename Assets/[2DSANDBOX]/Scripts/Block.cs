
using System;
using UnityEngine;

public class Block : MonoBehaviour, System.IComparable<Block>
{
    public BlockType blockType = BlockType.None;
    public int hitPoints = 1;
    private bool is_clicked = false;
    [HideInInspector]
    public int id = -1;
    private SaveVector3 pickableData = null;
    void Start(){
        
    }

    public void OnHit(){
        if(!is_clicked){
            SetAsPickupable();
        }
    }

    public void SetAsPickupable(){
        ReadyForPickupable();
        AddClickedBlock(blockType, id);
    }

    public void ReadyForPickupable(){
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        if(blockType!=BlockType.Sand){
            gameObject.AddComponent<Rigidbody2D>();
        }
        is_clicked = true;
        pickableData = new SaveVector3(transform.position);
    }

    private void AddClickedBlock(BlockType btype, int block_id){
        switch (btype){
            case BlockType.Dirt:
                SaveGame.Instance.data.deletedDirt.Add(block_id);
                SaveGame.Instance.data.placedDirts.Remove(pickableData);
                SaveGame.Instance.data.unpickedDirts.Add(pickableData);                
            break;
            case BlockType.Grass:
                SaveGame.Instance.data.deletedGrass.Add(block_id);
                SaveGame.Instance.data.placedGrass.Remove(pickableData);
                SaveGame.Instance.data.unpickedGrass.Add(pickableData);               
            break;
            case BlockType.Cobble:
                SaveGame.Instance.data.deletedCobble.Add(block_id);
                SaveGame.Instance.data.placedCobbles.Remove(pickableData);
                SaveGame.Instance.data.unpickedCobbles.Add(pickableData);                       
            break;
            case BlockType.Wood:
                // Debug.Log("add deleted wood");
                SaveGame.Instance.data.deletedWood.Add(block_id);
                SaveGame.Instance.data.placedWoods.Remove(pickableData);
                SaveGame.Instance.data.unpickedWoods.Add(pickableData);           
            break;
            case BlockType.Leaf: 
                SaveGame.Instance.data.deletedLeaf.Add(block_id);
                SaveGame.Instance.data.placedLeafs.Remove(pickableData);
                SaveGame.Instance.data.unpickedLeafs.Add(pickableData);
            break;
            case BlockType.Sand:
                SaveGame.Instance.data.deletedSand.Add(block_id);
                SaveGame.Instance.data.placedSands.Remove(pickableData);
                SaveGame.Instance.data.unpickedSands.Add(pickableData);
                break;
            case BlockType.Brick:
               SaveGame.Instance.data.deletedBrick.Add(block_id);
               SaveGame.Instance.data.placedBricks.Remove(pickableData);
               SaveGame.Instance.data.unpickedBricks.Add(pickableData);
                break;
                case BlockType.Ice:
                SaveGame.Instance.data.deletedIce.Add(block_id);
                SaveGame.Instance.data.placedIces.Remove(pickableData);
                SaveGame.Instance.data.unpickedIces.Add(pickableData);
                break;
            case BlockType.Flint:
                SaveGame.Instance.data.deletedFlint.Add(block_id);
                SaveGame.Instance.data.placedFlints.Remove(pickableData);
                SaveGame.Instance.data.unpickedFlints.Add(pickableData);
                break;
            case BlockType.Clay:
                SaveGame.Instance.data.deletedClay.Add(block_id);
                SaveGame.Instance.data.placedClays.Remove(pickableData);
                SaveGame.Instance.data.unpickedClays.Add(pickableData);
                break;
            case BlockType.Snow:
                SaveGame.Instance.data.deletedSnow.Add(block_id);
                SaveGame.Instance.data.placedSnows.Remove(pickableData);
                SaveGame.Instance.data.unpickedSnows.Add(pickableData);
                break;
            case BlockType.Skyrock:
                SaveGame.Instance.data.deletedSkyrock.Add(block_id);
                SaveGame.Instance.data.placedSkyrocks.Remove(pickableData);
                SaveGame.Instance.data.unpickedSkyrocks.Add(pickableData);
                break;
            case BlockType.Flower:
                SaveGame.Instance.data.deletedFlower.Add(block_id);
                SaveGame.Instance.data.placedFlowers.Remove(pickableData);
                SaveGame.Instance.data.unpickedFlowers.Add(pickableData);
                break;
            case BlockType.Grassitem:
                SaveGame.Instance.data.deletedGrassitem.Add(block_id);
                SaveGame.Instance.data.placedGrassitems.Remove(pickableData);
                SaveGame.Instance.data.unpickedGrassitems.Add(pickableData);
                break;
            case BlockType.Brickred:
                SaveGame.Instance.data.deletedBrickred.Add(block_id);
                SaveGame.Instance.data.placedBrickreds.Remove(pickableData);
                SaveGame.Instance.data.unpickedBrickreds.Add(pickableData);
                break;
            case BlockType.Plank:
                SaveGame.Instance.data.deletedPlank.Add(block_id);
                SaveGame.Instance.data.placedPlanks.Remove(pickableData);
                SaveGame.Instance.data.unpickedPlanks.Add(pickableData);
                break;
            case BlockType.Glass:
                SaveGame.Instance.data.deletedGlass.Add(block_id);
                SaveGame.Instance.data.placedGlasss.Remove(pickableData);
                SaveGame.Instance.data.unpickedGlasss.Add(pickableData);
                break;
            case BlockType.Camp:
                SaveGame.Instance.data.deletedCamp.Add(block_id);
                SaveGame.Instance.data.placedCamps.Remove(pickableData);
                SaveGame.Instance.data.unpickedCamps.Add(pickableData);
                break;
            case BlockType.Camprock:
                SaveGame.Instance.data.deletedCamprock.Add(block_id);
                SaveGame.Instance.data.placedCamprocks.Remove(pickableData);
                SaveGame.Instance.data.unpickedCamprocks.Add(pickableData);
                break;
                break;
                break;


            default:
            break;
        }
    }

    void OnCollisionEnter2D(Collision2D colenter) {
        if (is_clicked && colenter.gameObject.tag == "Player"){
            switch (blockType){
                case BlockType.Dirt:
                    SaveGame.Instance.data.dirtAmount++;
                    SaveGame.Instance.data.unpickedDirts.Remove(pickableData);
                break;
                case BlockType.Grass:
                    SaveGame.Instance.data.grassAmount++;
                    SaveGame.Instance.data.unpickedGrass.Remove(pickableData);
                break;
                case BlockType.Cobble:
                    SaveGame.Instance.data.cobbleAmount++;
                    if (SaveGame.Instance.data.flintAmount > 0)
                    {
                        SaveGame.Instance.data.camprockAmount++;
                    }
                    SaveGame.Instance.data.unpickedCobbles.Remove(pickableData);
                break;
                case BlockType.Wood:
                    SaveGame.Instance.data.woodAmount++;
                    SaveGame.Instance.data.plankAmount++;
                    if (SaveGame.Instance.data.flintAmount > 0)
                    {
                        SaveGame.Instance.data.campAmount++;
                    }

                    // SaveGame.Instance.data.unpickedWoods.ForEach(sv3=>{
                    //     Debug.Log(sv3.ToString());
                    // });
                    // Debug.Log("pickable data: "+pickableData.ToString());
                    SaveGame.Instance.data.unpickedWoods.Remove(pickableData);
                    // SaveGame.Instance.data.unpickedWoods.ForEach(sv3=>{
                    //     Debug.Log(sv3.ToString());
                    // });
                break;
                case BlockType.Leaf:
                    SaveGame.Instance.data.leafAmount++;
                    SaveGame.Instance.data.unpickedLeafs.Remove(pickableData);
                break;
                    //Crafting of Glass
                case BlockType.Sand:
                    SaveGame.Instance.data.sandAmount++;
                    SaveGame.Instance.data.glassAmount++;
                    SaveGame.Instance.data.unpickedSands.Remove(pickableData);
                    break;
                    case BlockType.Brick:
                    SaveGame.Instance.data.brickAmount++;
                    SaveGame.Instance.data.unpickedBricks.Remove(pickableData);
                    break;
                    case BlockType.Ice:
                    SaveGame.Instance.data.iceAmount++;
                    SaveGame.Instance.data.unpickedIces.Remove(pickableData);
                    break;
                case BlockType.Flint:
                    SaveGame.Instance.data.flintAmount++;
                    if (SaveGame.Instance.data.cobbleAmount > 0)
                    {
                        SaveGame.Instance.data.camprockAmount++;
                    }
                    if (SaveGame.Instance.data.woodAmount > 0)
                    {
                        SaveGame.Instance.data.campAmount++;
                    }
                    SaveGame.Instance.data.unpickedFlints.Remove(pickableData);
                    break;
                case BlockType.Clay:
                    SaveGame.Instance.data.clayAmount++;
                    SaveGame.Instance.data.brickredAmount++;
                    SaveGame.Instance.data.unpickedClays.Remove(pickableData);
                    break;
                case BlockType.Snow:
                    SaveGame.Instance.data.snowAmount++;
                    SaveGame.Instance.data.unpickedSnows.Remove(pickableData);
                    break;
                case BlockType.Skyrock:
                    SaveGame.Instance.data.skyrockAmount++;
                    SaveGame.Instance.data.unpickedSkyrocks.Remove(pickableData);
                    break;
                case BlockType.Flower:
                    SaveGame.Instance.data.flowerAmount++;
                    SaveGame.Instance.data.unpickedFlowers.Remove(pickableData);
                    break;
                case BlockType.Grassitem:
                    SaveGame.Instance.data.grassitemAmount++;
                    SaveGame.Instance.data.unpickedGrassitems.Remove(pickableData);
                    break;
                case BlockType.Brickred:
                    SaveGame.Instance.data.brickredAmount++;
                    SaveGame.Instance.data.unpickedBrickreds.Remove(pickableData);
                    break;
                case BlockType.Plank:
                    SaveGame.Instance.data.plankAmount++;
                    SaveGame.Instance.data.unpickedPlanks.Remove(pickableData);
                    break;
                case BlockType.Glass:
                    SaveGame.Instance.data.glassAmount++;
                    SaveGame.Instance.data.unpickedGlasss.Remove(pickableData);
                    break;
                case BlockType.Camp:
                    SaveGame.Instance.data.campAmount++;
                    SaveGame.Instance.data.unpickedCamps.Remove(pickableData);
                    break;
                case BlockType.Camprock:
                    SaveGame.Instance.data.camprockAmount++;
                    SaveGame.Instance.data.unpickedCamprocks.Remove(pickableData);
                    break;
                default:
                break;
            }
            pickableData = null;
            is_clicked = false;
            Destroy(gameObject);
        }
    }

    public void PlayerPickClickedBlock(BlockType btype){
        
    }

    int IComparable<Block>.CompareTo(Block other){
        return name.CompareTo(other.name);
    }

    public void SetBlock(Transform parent, string base_name){
        name = id > 0 ? base_name + " ("+id+")" : base_name;
        transform.SetParent(parent);
    }
}


public enum BlockType{
    Dirt,
    Grass,
    Cobble,
    Wood,
    Leaf,
    Sand,
    Brick,
    Ice,
    Flint,
    Clay,
    Snow,
    Skyrock,
    Flower,
    Grassitem,
    Brickred,
    Plank,
    Glass,
    Camp,
    Camprock,
    None
}