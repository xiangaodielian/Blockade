/*-----------------------------------/
  LoseCollider Class - Blockade
  Controlling class for LoseCollider
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LoseCollider : MonoBehaviour {

	private AudioSource audioSouce;
	
	void Start(){
		audioSouce = GetComponent<AudioSource>();
		audioSouce.volume = PrefsManager.GetMasterSFXVolume();
	}
	
	//Reset Level or Go To Lose Screen if Applicable
	//Destroy Ball if more than one Ball on screen (Multiball Case)
	void OnTriggerEnter2D(Collider2D collider){
		Ball[] ballArray = FindObjectsOfType<Ball>();
		
		if(collider.tag == "Ball"){
			audioSouce.Play();
			if(ballArray.Length == 1){
				if(GameMaster.instance.playerLives > 0){
					GameMaster.instance.playerLives--;
					GameMaster.instance.ResetCurrentLevel();
				} else
					GameMaster.instance.ChangeToLevel("Lose");
			} else
				Destroy(collider.gameObject);
		}
	}
}
