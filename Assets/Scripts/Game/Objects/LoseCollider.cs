/*-----------------------------------/
  LoseCollider Class - Blockade
  Controlling class for LoseCollider
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 7 Apr, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LoseCollider : MonoBehaviour {

	private AudioSource audioSouce;
	
	void Start(){
		audioSouce = GetComponent<AudioSource>();
		audioSouce.volume = PrefsManager.GetMasterSFXVolume();
		audioSouce.clip = ResourceManager.LoadAudioClip(false, "LostBall");
	}
	
	//Reset Level or Go To Lose Screen if Applicable
	//Destroy Ball if more than one Ball on screen (Multiball Case)
	void OnTriggerEnter(Collider collider){
		Ball[] ballArray = FindObjectsOfType<Ball>();
		
		if(collider.tag == "Ball"){
			audioSouce.Play();
			if(ballArray.Length == 1){
				if(GameMaster.instance.gameValues.playerLives > 0){
					GameMaster.instance.gameValues.playerLives--;
					GameMaster.instance.ResetCurrentLevel();
				} else
					GameMaster.instance.ChangeToLevel("Lose");
			}

			Destroy(collider.gameObject);
		}
	}
}
