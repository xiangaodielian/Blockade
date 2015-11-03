using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LoseCollider : MonoBehaviour {

	private GameMaster gameMaster;
	private AudioSource audioSouce;
	
	void Start(){
		gameMaster = GameObject.FindObjectOfType<GameMaster>();
		audioSouce = GetComponent<AudioSource>();
		audioSouce.volume = PrefsManager.GetMasterSFXVolume();
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Ball[] ballArray = FindObjectsOfType<Ball>();
		
		if(collider.tag == "Ball"){
			audioSouce.Play();
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
}
