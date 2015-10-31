using UnityEngine;
using System.Collections;

public class PrefsManager{
	
	const string MASTER_VOLUME_KEY = "master_volume";
	const string MASTER_SFX_KEY = "master_sfx_volume";
	const string LEVEL_KEY = "level_unlocked_";
	const string BALL_COLOR_RED = "ball_color_red";
	const string BALL_COLOR_GREEN = "ball_color_green";
	const string BALL_COLOR_BLUE = "ball_color_blue";
	
	public static void SetMasterMusicVolume(float volume){
		if(volume >= 0f && volume <= 1f){
			PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Master Volume Out of Range");
		}
	}
	
	public static float GetMasterMusicVolume(){
		return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY,1f);
	}
	
	public static void SetMasterSFXVolume(float volume){
		if(volume >= 0f && volume <= 1f){
			PlayerPrefs.SetFloat(MASTER_SFX_KEY, volume);
		} else {
			Debug.LogError("SFX Volume Out of Range");
		}
	}
	
	public static float GetMasterSFXVolume(){
		return PlayerPrefs.GetFloat(MASTER_SFX_KEY,1f);
	}
	
	// Set Level Unlock State (1 = unlocked)
	public static void UnlockLevel(int level){
		if(level <= Application.levelCount-1){
			PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(),1);
		} else{
			Debug.LogError("Level Not in Build Order");
		}
	}
	
	public static bool IsLevelUnlocked(int level){
		int levelValue = PlayerPrefs.GetInt(LEVEL_KEY + level.ToString());
		bool isLevelUnlocked = (levelValue == 1);
		
		if(level <= Application.levelCount-1){
			return isLevelUnlocked;
		} else{
			Debug.LogError("Level Not in Build Order");
			return false;
		}
	}
	
	public static Color GetBallColor(){
		float red = PlayerPrefs.GetFloat(BALL_COLOR_RED,1f);
		float green = PlayerPrefs.GetFloat(BALL_COLOR_GREEN,0f);
		float blue = PlayerPrefs.GetFloat(BALL_COLOR_BLUE,0f);
		
		return new Color(red,green,blue,1f);
	}
	
	public static void SetBallColor(float red, float green, float blue){
		PlayerPrefs.SetFloat(BALL_COLOR_RED,red);
		PlayerPrefs.SetFloat(BALL_COLOR_GREEN,green);
		PlayerPrefs.SetFloat(BALL_COLOR_BLUE,blue);
	}
}
