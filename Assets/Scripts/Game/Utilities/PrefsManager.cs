using UnityEngine;
using System.Collections;

public class PrefsManager{
	
	const string MASTER_VOLUME_KEY = "master_volume";
	const string MASTER_SFX_KEY = "master_sfx_volume";
	const string LEVEL_KEY = "level_unlocked_";
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
	
	public static int GetHighScore(int rank){
		int highScore = 0;
		switch(rank){
			case 1:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_1,10000);
				break;
			case 2:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_2,8000);
				break;
			case 3:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_3,6000);
				break;
			case 4:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_4,4000);
				break;
			case 5:
				highScore = PlayerPrefs.GetInt(HIGH_SCORE_5,2000);
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
}