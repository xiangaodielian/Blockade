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
				if(GameMaster.Instance.PlayerManager.GetPlayerLives() > 0){
					GameMaster.Instance.PlayerManager.AddToPlayerLives(-1);
					LevelManager.Instance.ResetCurrentLevel();
				} else{
					GUIManager.Instance.DestroyInGameGUI(GUIManager.TargetMenuOptions.LoseMenu);
					LevelManager.Instance.SetPreviousLevel();
					LevelManager.Instance.ChangeToLevel("EndGame");
				}
			}

			Destroy(collider.gameObject);
		}
	}
}
