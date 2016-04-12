/*------------------------------/
  LevelManager Class - Universal
  Manages Level loading and
  saving functions
  Writen by Joe Arthur
  Latest Revision - 9 Apr, 2016
  Compatible with Unity 5.3.3
/-----------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager {
	
	public static void LoadLevel(string name){
		if(name.Contains("Level")){
			bool scenePathExists = AssetBundleManager.instance.GetScenePath(name);

			if(scenePathExists)
				SceneManager.LoadScene(name);
		} else
			SceneManager.LoadScene(name);
	}

	public static AsyncOperation LoadLevelAsync(string name){
		bool scenePathExists = AssetBundleManager.instance.GetScenePath(name);

		if(scenePathExists)
			return SceneManager.LoadSceneAsync(name);
		else
			return null;
	}
	
	public static void LoadNextLevel(){
		string nextLevel = "Level_";
		int curLevelNum = GetLevelNum();
		curLevelNum++;

		if(curLevelNum < 10)
			nextLevel += "0";

		nextLevel += curLevelNum.ToString();

		bool scenePathExists = AssetBundleManager.instance.GetScenePath(nextLevel);

		if(scenePathExists)
			SceneManager.LoadSceneAsync(nextLevel);
	}
	
	public static void ReloadLevel(){
		SceneManager.LoadScene(GetCurrentLevel());
	}
	
	public static string GetCurrentLevel(){
		return SceneManager.GetActiveScene().name;
	}
	
	public static int GetLevelNum(){
		string curLevel = GetCurrentLevel();

		if(curLevel.Contains("Level_0"))
			curLevel = curLevel.Replace("Level_0", "");
		else
			curLevel = curLevel.Replace("Level_", "");

		return int.Parse(curLevel);
	}
	
	public static void QuitApplication(){
		Application.Quit();
	}
}
