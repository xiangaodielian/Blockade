/*-----------------------------------/
  OptionsController Class - Blockade
  Controlling class for setting and
  loading game preferences
  Writen by Joe Arthur
  Latest Revision - 10 Apr, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class OptionsController : MonoBehaviour {
	
	#region Variables
	
	//Singleton Instance of OptionsController
	public static OptionsController instance {get; private set;}
	
	private float musicVolume;
	private float sfxVolume;
	private float sfxVolumeOld;
	private Color ballColor;
	private AudioSource audioSource;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
	}

	void Start(){
		musicVolume = PrefsManager.GetMasterMusicVolume();
		sfxVolume = PrefsManager.GetMasterSFXVolume();
		sfxVolumeOld = sfxVolume;
		ballColor = PrefsManager.GetBallColor();
		audioSource = GetComponent<AudioSource>();
	}

	void Update(){
		if(sfxVolume != sfxVolumeOld && Input.GetMouseButtonUp(0)){
			audioSource.Play();
			sfxVolumeOld = sfxVolume;
		}
	}
	
	#endregion
	#region Set Option Functions and Checks
	
	public void SetUseCursor(bool useCursor){
		InputManager.instance.useCursorMovement = useCursor;
		PrefsManager.SetMouseControl(useCursor);
	}

	public void SetAudioClip(){
		audioSource.clip = ResourceManager.LoadAudioClip(false, "Bounce");
	}

	public void SetSFXVolume(float value){
		sfxVolume = value;
		audioSource.volume = sfxVolume;
	}
	
	public void SetMusicVolume(float value){
		musicVolume = value;
		if(MusicPlayer.instance)
			MusicPlayer.instance.SetVolume(value);
	}
	
	// Sets Color of Ball (Red = 0, Green = 1, Blue = 2)
	public void SetBallColor(int color, float value){
		if(color == 0)
			ballColor.r = value;
		if(color == 1)
			ballColor.g = value;
		if(color == 2)
			ballColor.b = value;
	}

	public bool SliderCheck(float musCheck, float sfxCheck){

		if(musCheck == musicVolume && sfxCheck == sfxVolume)
			return true;

		return false;
	}
	
	#endregion
	#region Save and Reset Functions
	
	public void SaveOptions(){
		PrefsManager.SetMasterMusicVolume(musicVolume);
		PrefsManager.SetMasterSFXVolume(sfxVolume);
		PrefsManager.SetBallColor(ballColor.r,ballColor.g,ballColor.b);
	}
	
	public void ResetDefaults(){
		musicVolume = 1f;
		PrefsManager.SetMasterMusicVolume(musicVolume);
		
		sfxVolume = 1f;
		PrefsManager.SetMasterSFXVolume(sfxVolume);
		
		ballColor.r = 1f;
		ballColor.g = 0f;
		ballColor.b = 0f;
		PrefsManager.SetBallColor(ballColor.r,ballColor.g,ballColor.b);

		PrefsManager.SetMouseControl(false);
	}
	
	#endregion
}
