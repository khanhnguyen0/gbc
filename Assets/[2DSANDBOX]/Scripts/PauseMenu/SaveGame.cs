using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveGame : MonoBehaviour {

    [HideInInspector]
    public SaveData data = new SaveData();

    private const string save_file_name = "data";
	//Nilupul
	public bool IsSaveGame;
    private static SaveGame instance = null;
    private bool is_will_load = false;

    public static SaveGame Instance{
        get {
            if(instance==null){
                GameObject saveGame = new GameObject("SaveGame");
                instance = saveGame.AddComponent<SaveGame>();
                DontDestroyOnLoad(saveGame);
            }
        return instance; 
        }
    }

    void Start(){
        Debug.Log("path: "+Application.persistentDataPath);
    }

    public void Save()
    {
        //mark all deleted gameobject in scene when player click it
        //save all unpicked pickup object transform and type (need factory class)
        //save all blocks placed by player
        //save player inventory
        //save player posistion
        // Debug.Log("save attemp");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        PauseScreen pauseScreen = GameObject.Find("PauseMenu Canvas").GetComponent<PauseScreen>();
        data.sceneIdx = SceneManager.GetActiveScene().buildIndex;
        data.cameraPos = new SaveVector3(camera.transform.position);
        SaveData(data, save_file_name);
        if(pauseScreen!=null){
            pauseScreen.ResumeGame();
        }
    }

    public void Load()
	{
        //get all childrens, loop per block type, delete marked deleted gameobject in scene
        //
       
        object data_obj = ReadData(save_file_name);
        if(data_obj!=null){
            data = (SaveData) data_obj;
        }
        if(SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex ==data.sceneIdx){
            //RefreshScene();
            PauseScreen pauseScreen = GameObject.Find("PauseMenu Canvas").GetComponent<PauseScreen>();
            if(pauseScreen!=null){
                pauseScreen.ResumeGame();
            }
        } else if(data_obj!=null){
            SceneManager.LoadScene (data.sceneIdx);
            is_will_load = true;
        } else {
            Debug.Log("no load data");
        }
        

    }
//	void OnLevelWasLoaded()
//	{
//		print ("fgfgfgfg");
//		RefreshScene ();
//	}

//    void OnLevelWasLoaded(int level){
//        if(is_will_load){
//            is_will_load = false;
//            object data_obj = ReadData(save_file_name);
//            if(data_obj!=null){
//                data = (SaveData) data_obj;
//            }
//            RefreshScene();
//			print ("elaaaaa");
//        }
//    }

    public void RefreshScene()
	{
        GameObject[] containers = GameObject.FindGameObjectsWithTag("block_container");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Heart heart = GameObject.FindGameObjectWithTag("life").GetComponentInChildren<Heart>();
        // Debug.Log("block loader length: "+containers.Length);
        BlockLoader[] loaders = new BlockLoader[containers.Length];
        for (int i = 0; i < loaders.Length; i++){
            loaders[i] = containers[i].GetComponent<BlockLoader>();
        }
        loaders.ForEach(loader=>loader.ClearClickedBlocks());
        loaders.ForEach(loader=>loader.CreateBlocks());
        player.transform.position = data.characterPosition.Vector3();
        camera.transform.position = data.cameraPos.Vector3();
        heart.SetSpriteIdx(data.health_index);
    }

    public void SaveData(object data, string fileName)
	{
		using(FileStream fs = File.Open(Application.persistentDataPath+"/"+ fileName, FileMode.OpenOrCreate)){
			BinaryFormatter m_bF = new BinaryFormatter();
			m_bF.Serialize(fs, data);
			fs.Close();
		}
	}

    public object ReadData(string fileName){
		object obj;
		string path = Application.persistentDataPath + "/" + fileName;
        
		if(File.Exists(path)){
			using(FileStream fs = File.Open(path, FileMode.Open)){
				BinaryFormatter m_bF = new BinaryFormatter();
				obj = (object)m_bF.Deserialize(fs);
				fs.Close();
			}
		} else {
			obj = null;
		}
		return obj;
	}
}

[Serializable]
public class SaveData{
    public List<int> deletedDirt = new List<int>();
    public List<int> deletedGrass = new List<int>();
    public List<int> deletedCobble= new List<int>();
    public List<int> deletedWood= new List<int>();
    public List<int> deletedLeaf= new List<int>();
    public List<int> deletedSand= new List<int>();
   public List<int> deletedBrick = new List<int>();
    public List<int> deletedIce = new List<int>();
    public List<int> deletedFlint = new List<int>();
    public List<int> deletedClay = new List<int>();
    public List<int> deletedSnow = new List<int>();
    public List<int> deletedSkyrock = new List<int>();
    public List<int> deletedFlower = new List<int>();
    public List<int> deletedGrassitem = new List<int>();
    public List<int> deletedBrickred = new List<int>();
    public List<int> deletedPlank = new List<int>();
    public List<int> deletedGlass = new List<int>();
    public List<int> deletedCamp = new List<int>();
    public List<int> deletedCamprock = new List<int>();

    public List<SaveVector3> unpickedWoods = new List<SaveVector3>();
    public List<SaveVector3> unpickedDirts = new List<SaveVector3>();
    public List<SaveVector3> unpickedGrass = new List<SaveVector3>();
    public List<SaveVector3> unpickedCobbles = new List<SaveVector3>();
    public List<SaveVector3> unpickedLeafs = new List<SaveVector3>();
    public List<SaveVector3> unpickedSands = new List<SaveVector3>();
   public List<SaveVector3> unpickedBricks = new List<SaveVector3>();
    public List<SaveVector3> unpickedIces = new List<SaveVector3>();
    public List<SaveVector3> unpickedFlints = new List<SaveVector3>();
    public List<SaveVector3> unpickedClays = new List<SaveVector3>();
    public List<SaveVector3> unpickedSnows = new List<SaveVector3>();
    public List<SaveVector3> unpickedSkyrocks = new List<SaveVector3>();
    public List<SaveVector3> unpickedFlowers = new List<SaveVector3>();
    public List<SaveVector3> unpickedGrassitems = new List<SaveVector3>();
    public List<SaveVector3> unpickedBrickreds = new List<SaveVector3>();
    public List<SaveVector3> unpickedPlanks = new List<SaveVector3>();
    public List<SaveVector3> unpickedGlasss = new List<SaveVector3>();
    public List<SaveVector3> unpickedCamps = new List<SaveVector3>();
    public List<SaveVector3> unpickedCamprocks = new List<SaveVector3>();

    public List<SaveVector3> placedWoods = new List<SaveVector3>();
    public List<SaveVector3> placedDirts = new List<SaveVector3>();
    public List<SaveVector3> placedGrass = new List<SaveVector3>();
    public List<SaveVector3> placedCobbles = new List<SaveVector3>();
    public List<SaveVector3> placedLeafs = new List<SaveVector3>();
    public List<SaveVector3> placedSands = new List<SaveVector3>();
   public List<SaveVector3> placedBricks = new List<SaveVector3>();
    public List<SaveVector3> placedIces = new List<SaveVector3>();
    public List<SaveVector3> placedFlints = new List<SaveVector3>();
    public List<SaveVector3> placedClays = new List<SaveVector3>();
    public List<SaveVector3> placedSnows = new List<SaveVector3>();
    public List<SaveVector3> placedSkyrocks = new List<SaveVector3>();
    public List<SaveVector3> placedFlowers = new List<SaveVector3>();
    public List<SaveVector3> placedGrassitems = new List<SaveVector3>();
    public List<SaveVector3> placedBrickreds = new List<SaveVector3>();
    public List<SaveVector3> placedPlanks = new List<SaveVector3>();
    public List<SaveVector3> placedGlasss = new List<SaveVector3>();
    public List<SaveVector3> placedCamps = new List<SaveVector3>();
    public List<SaveVector3> placedCamprocks = new List<SaveVector3>();

    public int dirtAmount = 0;
	public int grassAmount = 0;
	public int cobbleAmount = 0;
	public int woodAmount = 0;
	public int leafAmount = 0;
	public int sandAmount = 0;
   public int brickAmount = 0;
    public int iceAmount = 0;
    public int flintAmount = 0;
    public int clayAmount = 0;
    public int snowAmount = 0;
    public int skyrockAmount = 0;
    public int flowerAmount = 0;
    public int grassitemAmount = 0;
    public int brickredAmount = 0;
    public int plankAmount = 0;
    public int glassAmount = 0;
    public int campAmount = 0;
    public int camprockAmount = 0;

    public int sceneIdx;
    public int health_index = -1;
    public SaveVector3 cameraPos;
    public SaveVector3 characterPosition;
}

[Serializable]
public class SaveVector3{
    public SaveVector3(Vector3 v3){
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    public Vector3 Vector3(){
        return new Vector3(x,y,z);
    }
    // public string ToString(){
    //     return "x:"+x+" y:"+y+" z:"+z;
    // }
    // public void IsIdentical
    public float x;
    public float y;
    public float z;
}
