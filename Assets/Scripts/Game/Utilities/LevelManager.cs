/*------------------------------/
  LevelManager Class - Universal
  Manages Level loading and
  saving functions
  Writen by Joe Arthur
  Latest Revision - 25 Mar, 2016
  Compatible with Unity 5.3.3
/-----------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager {
	
	public static void LoadLevel(string name){
		SceneManager.LoadScene(name);
	}

	public static AsyncOperation LoadLevelAsync(string name){
		return SceneManager.LoadSceneAsync(name);
	}
	
	public static void LoadNextLevel(){
		SceneManager.LoadScene(GetLevelNum()+1);
	}
	
	public static void ReloadLevel(){
		SceneManager.LoadScene(GetCurrentLevel());
	}
	
	public static string GetCurrentLevel(){
		return SceneManager.GetActiveScene().name;
	}
	
	public static int GetLevelNum(){
		return SceneManager.GetActiveScene().buildIndex;
	}
	
	public static void QuitApplication(){
		Application.Quit();
	}
}
