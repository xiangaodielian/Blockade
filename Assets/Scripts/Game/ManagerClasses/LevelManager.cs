using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

	#region Variables

	public static LevelManager instance {get; private set;}

	[HideInInspector] public AsyncOperation asyncOp;

	private string previousLevel;

	#endregion
	#region Mono Functions

	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);

		instance = this;
	}

	void OnEnable(){
		EnableDelegates();
	}

	void OnDisable(){
		DisableDelegates();
	}

	#endregion
	#region Level Loading

	public static void LoadLevel(string name){
		if(name != "Splash"){
			bool scenePathExists = GameMaster.GMInstance.abManager.GetScenePath(name);

			if(scenePathExists)
				SceneManager.LoadScene(name);
		}
	}

	public static AsyncOperation LoadLevelAsync(string name){
		bool scenePathExists = GameMaster.GMInstance.abManager.GetScenePath(name);

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

		bool scenePathExists = GameMaster.GMInstance.abManager.GetScenePath(nextLevel);

		if(scenePathExists)
			SceneManager.LoadSceneAsync(nextLevel);
	}
	
	public static void ReloadLevel(){
		SceneManager.LoadScene(GetCurrentLevel());
	}

	#endregion
	#region Level Management

	public void ChangeToLevel(string level){
		if(GameMaster.GMInstance.playerManager.activePlayer != null)
			GameMaster.GMInstance.playerManager.activePlayer.firstBall = true;

		if(level == "Next"){
			if(GetLevelNum() == 20){
				GUIManager.instance.DestroyInGameGUI(GUIManager.TargetMenuOptions.winMenu);
				LoadLevel("EndGame");
			} else
				LoadNextLevel();
		} else{
			if(level == "LatestCheckpoint"){
				int latestCheckpoint = PrefsManager.GetLatestCheckpoint();

				if(latestCheckpoint < 10)
					level = "Level_0" + latestCheckpoint;
				else
					level = "Level_" + latestCheckpoint;
			}

			LoadLevel(level);
		}
	}

	public IEnumerator ChangeToLevelAsync(string level){
		if(GameMaster.GMInstance.playerManager.activePlayer != null)
			GameMaster.GMInstance.playerManager.activePlayer.firstBall = true;

		if(level == "LatestCheckpoint"){
			int latestCheckpoint = PrefsManager.GetLatestCheckpoint();

			if(latestCheckpoint < 10)
				level = "Level_0" + latestCheckpoint;
			else
				level = "Level_" + latestCheckpoint;
		}

		asyncOp = LoadLevelAsync(level);

		while(!asyncOp.isDone)
			yield return null;

		asyncOp = null;
	}

	public void ResetCurrentLevel(){
		EventManager.TriggerEvent(EventManager.EventNames.levelReset);
		GUIManager.instance.inGameGUI.TogglePrompt(true);
	}

	public void RestartLevel(){
		GameMaster.GMInstance.playerManager.SetPlayerScore(0);
		GameMaster.GMInstance.playerManager.SetPlayerLives(3);

		ChangeToLevel(previousLevel);
	}

	#endregion
	#region Application Management
	
	public static void QuitApplication(){
		Application.Quit();
	}

	#endregion
	#region Utility
	
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

	public void SetPreviousLevel(){
		previousLevel = GetCurrentLevel();
	}

	#endregion
	#region Delegates

	void EnableDelegates(){
		SceneManager.sceneLoaded += SceneChange;
		SceneManager.sceneLoaded += GUIManager.instance.SceneChange;
		SceneManager.sceneLoaded += GameMaster.GMInstance.gameObjectManager.SceneChange;
		SceneManager.sceneLoaded += GameMaster.GMInstance.playerManager.SceneChange;
		SceneManager.sceneLoaded += GameMaster.GMInstance.abManager.SceneChange;
	}

	void DisableDelegates(){
		SceneManager.sceneLoaded -= SceneChange;
		SceneManager.sceneLoaded -= GUIManager.instance.SceneChange;
		SceneManager.sceneLoaded -= GameMaster.GMInstance.gameObjectManager.SceneChange;
		SceneManager.sceneLoaded -= GameMaster.GMInstance.playerManager.SceneChange;
		SceneManager.sceneLoaded -= GameMaster.GMInstance.abManager.SceneChange;
	}

	void SceneChange(Scene scene, LoadSceneMode mode){
		if(scene.name == "MainMenu"){
			EventManager.TriggerEvent(EventManager.EventNames.levelFinish);

			if(MusicPlayer.instance.isPlaying)
				MusicPlayer.instance.StopMusic();

			MusicPlayer.instance.MenuMusicSet();
			MusicPlayer.instance.StartMusic();

			OptionsManager.instance.SetAudioClip();
		} else if(scene.name.Contains("Level")){
			PrefsManager.SetCurrentLevel(LevelManager.GetLevelNum());

			if(LevelManager.GetLevelNum() > PrefsManager.GetLatestCheckpoint()){
				PrefsManager.SetLatestCheckpoint(LevelManager.GetLevelNum());
				PrefsManager.SetLevelUnlocked(LevelManager.GetLevelNum());
			}
		} else if(scene.name != "Splash"){
			EventManager.TriggerEvent(EventManager.EventNames.levelFinish);
			StartCoroutine(ResourceManager.UnloadUnusedResources());
		}
	}

	#endregion
}
