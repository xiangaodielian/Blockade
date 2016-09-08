using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LoseCollider : MonoBehaviour {

	private AudioSource audioSouce;
	
	void Start(){
		audioSouce = GetComponent<AudioSource>();
		audioSouce.volume = PrefsManager.GetMasterSFXVolume();
		audioSouce.clip = ResourceManager.LoadAudioClip("LostBall");
	}
	
	//Reset Level or Go To Lose Screen if Applicable
	//Destroy Ball if more than one Ball on screen (Multiball Case)
	void OnTriggerEnter(Collider collider){
		Ball[] ballArray = FindObjectsOfType<Ball>();
		
		if(collider.tag == "Ball"){
			audioSouce.Play();
			if(ballArray.Length == 1){
				if(GameMaster.GMInstance.playerManager.GetPlayerLives() > 0){
					GameMaster.GMInstance.playerManager.AddToPlayerLives(-1);
					LevelManager.instance.ResetCurrentLevel();
				} else{
					GUIManager.instance.DestroyInGameGUI(GUIManager.TargetMenuOptions.loseMenu);
					LevelManager.instance.SetPreviousLevel();
					LevelManager.instance.ChangeToLevel("EndGame");
				}
			}

			Destroy(collider.gameObject);
		}
	}
}
