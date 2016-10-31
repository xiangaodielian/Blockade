﻿using UnityEngine;
using ApplicationManagement;
using ApplicationManagement.ResourceControl;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlaySpace : MonoBehaviour {
	
	#region Variables
	
	//Singleton instance of Playspace
	public static PlaySpace instance {get; private set;}
	
	[Tooltip("Reference to the Shields childed to the Playspace.")]
	[SerializeField] private GameObject[] shields = new GameObject[25];				//Array of all Shields (children of Playspace)
	[Tooltip("The Long Colliders that are active when Shields aren't rotating.")]
	[SerializeField] private GameObject[] longColliders = new GameObject[3];		//Array of Long Box Colliders
	
	private float timer = 0f;
	private Animator animator;
	private AudioSource audioSource;
	private bool movingShields = false;
	
	#endregion
	#region Mono Functionc
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}
	
	void Start(){
		audioSource.volume = PrefsManager.GetMasterSFXVolume();
		audioSource.clip = ResourceManager.LoadAudioClip("ShieldDeploy");
		
		foreach(GameObject shield in shields)
			shield.AddComponent<Shield>();

		ResourceManager.SetMaterialTextures(this.gameObject);
	}
	
	void Update(){
		//Deploy ShieldProjectors when Time is reached
		if(timer > 0f){
			if(Time.time - timer > 1f){
				animator.SetTrigger("deployTrigger");
				timer = 0f;
			}
		}

		LevelCheck();
	}
	
	#endregion
	#region Utility Functions
	
	public void StartTimer(){	
		timer = Time.time;
	}

	public void PlaySound(){
		audioSource.Play();
	}
	
	//Dissolve Shields into scene
	public void ShowShields(){
		foreach(GameObject shield in shields)
			shield.GetComponent<Shield>().DissolveIn();
			
		LevelCheck();
	}

	void LevelCheck(){
		if(LevelManager.GetLevelNum() >= 5){
			if(!movingShields)
				SetMovingShields(true);
		} else
			SetMovingShields(false);
	}
	
	//Set Shields as capable of rotation on hit
	void SetMovingShields(bool b){
		movingShields = b;

		foreach(GameObject longCollider in longColliders)
			longCollider.SetActive(!b);
			
		foreach(GameObject shield in shields)
			shield.GetComponent<Shield>().ToggleMovingShields(b);
	}
	
	#endregion
}
