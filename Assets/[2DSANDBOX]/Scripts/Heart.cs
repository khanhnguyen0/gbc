using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour {

	public Sprite[] health;
	public Image heart_life;

	private int curr_sprite_index = -1;

	void Start(){
		if(curr_sprite_index==-1){
			curr_sprite_index = health.Length-1;
		}
	}

	public void SetSpriteIdx(int idx){
		if(idx > -1){
			curr_sprite_index = idx;
			heart_life.sprite = health[curr_sprite_index];
		}
	}

	public void Hit(){
		if(curr_sprite_index>0){
			curr_sprite_index--;
			heart_life.sprite = health[curr_sprite_index];
			SaveGame.Instance.data.health_index = curr_sprite_index;
		} else {
			//todo player dead
			Debug.Log("Player dead");
		}
	}
}
