using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {
	
	private AudioSource audioSource;
	
	void Awake(){
		GameObject.DontDestroyOnLoad(gameObject);
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterMusicVolume();
	}
	
	public void SetVolume(float value){
		audioSource.volume = value;
	}
}
