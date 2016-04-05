/*------------------------------/
  PrefsManager Class - Blockade
  Manages all Player Prefs calls
  and applies settings
  Writen by Joe Arthur
  Latest Revision - 4 Mar, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

public class PrefsManager{
	
	#region Pref Keys
	
	const string MASTER_VOLUME_KEY = "master_volume";
	const string MASTER_SFX_KEY = "master_sfx_volume";
	const string LATEST_CHECKPOINT = "latest_checkpoint";
	const string LEVEL_UNLOCKED = "level_unlocked";
	const string CURRENT_LEVEL = "current_level";
	const string BALL_COLOR_RED = "ball_color_red";
	const string BALL_COLOR_GREEN = "ball_color_green";
	const string BALL_COLOR_BLUE = "ball_color_blue";
	const string HIGH_SCORE_NAME_1 = "high_score_name_1";
	const string HIGH_SCORE_1 = "high_score_1";
	const string HIGH_SCORE_NAME_2 = "high_score_name_2";
	const string HIGH_SCORE_2 = "high_score_2";
	const string HIGH_SCORE_NAME_3 = "high_score_name_3";
	const string HIGH_SCORE_3 = "high_score_3";
	const string HIGH_SCORE_NAME_4 = "high_score_name_4";
	const string HIGH_SCORE_4 = "high_score_4";
	const string HIGH_SCORE_NAME_5 = "high_score_name_5";
	const string HIGH_SCORE_5 = "high_score_5";
	
	#endregion
	#region Volume Functions
	
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
	
	#endregion
	#region Level Functions
	
	public static void SetLatestCheckpoint(int levelNum){
		PlayerPrefs.SetInt(LATEST_CHECKPOINT,levelNum);
	}
	
	public static int GetLatestCheckpoint(){
		return PlayerPrefs.GetInt(LATEST_CHECKPOINT,1);
	}
	
	public static void SetLevelUnlocked(int level){
		PlayerPrefs.SetInt(LEVEL_UNLOCKED,level);
	}
	
	public static int GetLevelUnlocked(){
		return PlayerPrefs.GetInt(LEVEL_UNLOCKED,1);
	}
	
	public static void SetCurrentLevel(int level){
		PlayerPrefs.SetInt(CURRENT_LEVEL,level);
	}
	
	public static int GetCurrentLevel(){
		return PlayerPrefs.GetInt(CURRENT_LEVEL,1);
	}
	
	public static int GetLevelNumber(){
		return LevelManager.GetLevelNum();
	}
	
	#endregion
	#region Other Pref Functions
	
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
	
	#endregion
	#region High Score Functions
	
	public static int GetHighScore(int rank){
		int highScore = 0;
		switch(rank){
			case 1:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_1,50000);
				break;
			case 2:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_2,30000);
				break;
			case 3:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_3,20000);
				break;
			case 4:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_4,10000);
				break;
			case 5:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_5,5000);
				break;
			default:
				Debug.LogError("High Score Rank out of range!");
				break;
		}
		
		return highScore;
	}
	
	public static void SetHighScore(int rank, int score){
		int replacedScore = 0;
		switch(rank){
			case 1:
				replacedScore = GetHighScore(1);
				PlayerPrefs.SetInt(HIGH_SCORE_1,score);
				SetHighScore(2,replacedScore);
				break;
			case 2:
				replacedScore = GetHighScore(2);
				PlayerPrefs.SetInt(HIGH_SCORE_2,score);
				SetHighScore(3,replacedScore);
				break;
			case 3:
				replacedScore = GetHighScore(3);
				PlayerPrefs.SetInt(HIGH_SCORE_3,score);
				SetHighScore(4,replacedScore);
				break;
			case 4:
				replacedScore = GetHighScore(4);
				PlayerPrefs.SetInt(HIGH_SCORE_4,score);
				SetHighScore(5,replacedScore);
				break;
			case 5:
				PlayerPrefs.SetInt(HIGH_SCORE_5,score);
				break;
			default:
				Debug.LogError("High Score Rank out of range!");
				break;
		}
	}
	
	public static string GetHighScoreName(int rank){
		string highScorename = "null";
		switch(rank){
			case 1:
				highScorename = PlayerPrefs.GetString(HIGH_SCORE_NAME_1,"JEA");
				break;
			case 2:
				highScorename = PlayerPrefs.GetString(HIGH_SCORE_NAME_2,"JEA");
				break;
			case 3:
				highScorename = PlayerPrefs.GetString(HIGH_SCORE_NAME_3,"JEA");
				break;
			case 4:
				highScorename = PlayerPrefs.GetString(HIGH_SCORE_NAME_4,"JEA");
				break;
			case 5:
				highScorename = PlayerPrefs.GetString(HIGH_SCORE_NAME_5,"JEA");
				break;
			default:
				Debug.LogError("High Score Rank out of range!");
				break;
		}
		
		return highScorename;
	}
	
	public static void SetHighScoreName(int rank, string name){
		string replacedName = "null";
		switch(rank){
			case 1:
				replacedName = GetHighScoreName(1);
				PlayerPrefs.SetString(HIGH_SCORE_NAME_1,name);
				SetHighScoreName(2,replacedName);
				break;
			case 2:
				replacedName = GetHighScoreName(2);
				PlayerPrefs.SetString(HIGH_SCORE_NAME_2,name);
				SetHighScoreName(3,replacedName);
				break;
			case 3:
				replacedName = GetHighScoreName(3);
				PlayerPrefs.SetString(HIGH_SCORE_NAME_3,name);
				SetHighScoreName(4,replacedName);
				break;
			case 4:
				replacedName = GetHighScoreName(4);
				PlayerPrefs.SetString(HIGH_SCORE_NAME_4,name);
				SetHighScoreName(5,replacedName);
				break;
			case 5:
				PlayerPrefs.SetString(HIGH_SCORE_NAME_5,name);
				break;
			default:
				Debug.LogError("High Score Rank out of range!");
				break;
		}
	}
	
	#endregion
}