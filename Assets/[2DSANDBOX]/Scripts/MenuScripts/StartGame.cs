using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

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
		SceneManager.LoadScene("SelectPlayer");
	}

	// Use this for initialization
	void Start ()
    {
        Application.targetFrameRate = 60;
	}
}
