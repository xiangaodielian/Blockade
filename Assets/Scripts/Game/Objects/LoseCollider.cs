using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour {

	private GameMaster gameMaster;
	
	void Start(){
		gameMaster = GameObject.FindObjectOfType<GameMaster>();
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Ball[] ballArray = FindObjectsOfType<Ball>();
		
		if(collider.tag == "Ball")
			if(ballArray.Length == 1){
				if(gameMaster.playerLives > 0){
					gameMaster.playerLives--;
					gameMaster.ResetCurrentLevel();
				} else
					gameMaster.ChangeToLevel("Lose");
			} else
				Destroy(collider.gameObject);
	}
}
