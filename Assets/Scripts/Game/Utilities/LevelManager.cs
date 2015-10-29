using UnityEngine;
using System.Collections;

public class LevelManager {
	
	public static void LoadLevel(string name){
		Application.LoadLevel(name);
	}
	
	public static void LoadNextLevel(){
		Application.LoadLevel(Application.loadedLevel+1);
	}
	
	public static void ReloadLevel(){
		Application.LoadLevel(GetCurrentLevel());
	}
	
	public static string GetCurrentLevel(){
		return Application.loadedLevelName;
	}
	
	public static int GetLevelNum(){
		return Application.loadedLevel;
	}
	
	public static void QuitApplication(){
		Application.Quit();
	}
}
