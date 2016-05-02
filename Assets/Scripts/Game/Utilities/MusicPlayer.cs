/*----------------------------/
  MusicPlayer Class - Blockade
  Manages MusicPlayer object
  and its functions
  Writen by Joe Arthur
  Latest Revision - 2 May, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {
	
	#region Variables
	
	//Singleton instance for MusicPlayer
	public static MusicPlayer instance {get; private set;}
	
	[System.Serializable] private class TrackNames{
		public string menuMusic = "";
		public string inGame = "";
	}

	[HideInInspector] public bool isPlaying = false;
	
	[SerializeField] private TrackNames trackNames = null;
	private Queue<AudioClip> clipQueue = new Queue<AudioClip>();

	private AudioSource audioSource;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
		
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterMusicVolume();
	}
	
	#endregion
	#region Music Utlity Functions
	
	public void MenuMusicSet(){
		audioSource.clip = ResourceManager.LoadAudioClip(true, trackNames.menuMusic);

		//Clear Queue and add inGame track
		if(clipQueue.Count != 0)
			clipQueue.Clear();

		clipQueue.Enqueue(ResourceManager.LoadAudioClip(true, trackNames.inGame));
	}

	public void NextTrack(){
		if(clipQueue.Count != 0){
			StopMusic();
			audioSource.clip = clipQueue.Dequeue();
			StartMusic();
		} else
			Debug.LogError("No AudioClips in Queue!");
	}

	public void StartMusic(){
		audioSource.Play();
		isPlaying = true;
	}

	public void StopMusic(){
		audioSource.Stop();
		isPlaying = false;
	}
	
	public void SetVolume(float value){
		audioSource.volume = value;
	}
	
	#endregion
}
