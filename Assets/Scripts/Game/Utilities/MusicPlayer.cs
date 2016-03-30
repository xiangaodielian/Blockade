﻿/*----------------------------/
  MusicPlayer Class - Blockade
  Manages MusicPlayer object
  and its functions
  Writen by Joe Arthur
  Latest Revision - 29 Mar, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {
	
	#region Variables
	
	//Singleton instance for MusicPlayer
	public static MusicPlayer instance {get; private set;}
	
	[HideInInspector] public bool isPlaying = false;
	
	private AudioSource audioSource;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
		
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterMusicVolume();
		audioSource.clip = ResourceManager.LoadAudioClip(true, "bg_music");
	}
	
	#endregion
	#region Music Utlity Functions
	
	public void StartMusic(){
		audioSource.Play();
		isPlaying = true;
	}
	
	public void SetVolume(float value){
		audioSource.volume = value;
	}
	
	#endregion
}
