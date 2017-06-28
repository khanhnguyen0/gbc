using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {

	public Texture2D playNormal;
	public Texture2D playHover;

	void OnMouseEnter(){
		GetComponent<GUITexture>().texture = playHover;
	}

	void OnMouseExit(){
		GetComponent<GUITexture>().texture = playNormal;
	}

	void OnMouseDown()
	{
		//Nilupul
		//SaveGame.Instance.Load();
		SaveGame.Instance.IsSaveGame = true;
		SceneManager.LoadScene ("World");
	}

	// Use this for initialization
	void Start () {
		
	}
}
