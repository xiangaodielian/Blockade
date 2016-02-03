/*----------------------------/
  MusicPlayer Class - Blockade
  Manages MusicPlayer object
  and its functions
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {
	
	//Singleton instance for MusicPlayer
	public static MusicPlayer instance {get; private set;}
	
	public bool isPlaying = false;
	
	private AudioSource audioSource;
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
		
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterMusicVolume();
	}
	
	public void StartMusic(){
		audioSource.Play();
		isPlaying = true;
	}
	
	public void SetVolume(float value){
		audioSource.volume = value;
	}
}
