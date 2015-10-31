using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class OptionsController : MonoBehaviour {
	
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;
	public Slider ballColorRedSlider;
	public Slider ballColorGreenSlider;
	public Slider ballColorBlueSlider;
	public Image ballImage;
	
	private MusicPlayer musicPlayer;
	private AudioSource sfxTestPlayer;
	private Color ballColor;
	private float sfxVolumeOld;
	
	void Start(){
		musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
		musicVolumeSlider.value = PrefsManager.GetMasterMusicVolume();
		
		sfxTestPlayer = GetComponent<AudioSource>();
		sfxVolumeSlider.value = PrefsManager.GetMasterSFXVolume();
		sfxTestPlayer.volume = sfxVolumeSlider.value;
		sfxVolumeOld = sfxTestPlayer.volume;
		
		if(ballColorRedSlider != null){
			ballColor = PrefsManager.GetBallColor();
			ballColorRedSlider.value = ballColor.r;
			ballColorGreenSlider.value = ballColor.g;
			ballColorBlueSlider.value = ballColor.b;
			ballImage.color = ballColor;
		}
	}
	
	void Update(){
		if(sfxVolumeOld != sfxTestPlayer.volume && Input.GetMouseButtonUp(0)){
			sfxTestPlayer.Play();
			sfxVolumeOld = sfxTestPlayer.volume;
		}
	}
	
	public void SetSliders(){
		sfxVolumeSlider.value = PrefsManager.GetMasterSFXVolume();
		musicVolumeSlider.value = PrefsManager.GetMasterMusicVolume();
	}
	
	public void SetSFXVolume(){
		if(sfxTestPlayer)
			sfxTestPlayer.volume = sfxVolumeSlider.value;
	}
	
	public void SetMusicVolume(){
		if(musicPlayer)
			musicPlayer.SetVolume(musicVolumeSlider.value);
	}
	
	// Sets Color of Ball (Red = 0, Green = 1, Blue = 2)
	public void SetBallColor(int color){
		if(color == 0)
			ballColor.r = ballColorRedSlider.value;
		if(color == 1)
			ballColor.g = ballColorGreenSlider.value;
		if(color == 2)
			ballColor.b = ballColorBlueSlider.value;
			
		ballImage.color = ballColor;
	}
	
	public void SaveAndExit(){
		PrefsManager.SetMasterMusicVolume(musicVolumeSlider.value);
		PrefsManager.SetMasterSFXVolume(sfxVolumeSlider.value);
		if(ballColorRedSlider != null)
			PrefsManager.SetBallColor(ballColorRedSlider.value,ballColorGreenSlider.value,ballColorBlueSlider.value);
	}
	
	public void ResetDefaults(){
		musicVolumeSlider.value = musicVolumeSlider.maxValue;
		PrefsManager.SetMasterMusicVolume(musicVolumeSlider.value);
		
		sfxVolumeSlider.value = sfxVolumeSlider.maxValue;
		PrefsManager.SetMasterSFXVolume(sfxVolumeSlider.value);
		
		ballColorRedSlider.value = ballColorRedSlider.maxValue;
		ballColorGreenSlider.value = ballColorGreenSlider.minValue;
		ballColorBlueSlider.value = ballColorBlueSlider.minValue;
		PrefsManager.SetBallColor(ballColorRedSlider.value,ballColorGreenSlider.value,ballColorBlueSlider.value);
	}
}
