using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    #region Variables

    public static LevelManager Instance { get; private set; }

    [HideInInspector] public AsyncOperation asyncOp;

    private string previousLevel;

    #endregion

    #region Mono Functions

    private void Awake() {
        if(Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void OnEnable() {
        EnableDelegates();
    }

    private void OnDisable() {
        DisableDelegates();
    }

    #endregion

    #region Level Loading

    public static void LoadLevel(string name) {
        if(name == "Splash")
            return;

        try {
            AssetBundleManager.Instance.GetScenePath(name);
            SceneManager.LoadScene(name);
        } catch(ArgumentException e) {
            Debug.LogError(string.Format("ERROR in {0}; {1}", e.Source, e.Message));
        }
    }

    public static AsyncOperation LoadLevelAsync(string name) {
        try {
            AssetBundleManager.Instance.GetScenePath(name);
            return SceneManager.LoadSceneAsync(name);
        } catch(ArgumentException e) {
            Debug.LogError(string.Format("ERROR in {0}; {1}", e.Source, e.Message));
        }

        return null;
    }

    public static void LoadNextLevel() {
        string nextLevel = "Level_";
        int curLevelNum = GetLevelNum();
        curLevelNum++;

        if(curLevelNum < 10)
            nextLevel += "0";

        nextLevel += curLevelNum.ToString();

        try {
            AssetBundleManager.Instance.GetScenePath(nextLevel);
            SceneManager.LoadSceneAsync(nextLevel);
        } catch(ArgumentException e) {
            Debug.LogError(string.Format("ERROR in {0}; {1}", e.Source, e.Message));
        }
	}
	
	public static void ReloadLevel(){
		SceneManager.LoadScene(GetCurrentLevel());
	}

	#endregion
	#region Level Management

	public void ChangeToLevel(string level){
		if(GameMaster.Instance.PlayerManager.activePlayer != null)
			GameMaster.Instance.PlayerManager.activePlayer.firstBall = true;

		if(level == "Next"){
			if(GetLevelNum() == 20){
				GUIManager.Instance.DestroyInGameGUI(GUIManager.TargetMenuOptions.WinMenu);
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
		if(GameMaster.Instance.PlayerManager.activePlayer != null)
			GameMaster.Instance.PlayerManager.activePlayer.firstBall = true;

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
		EventManager.TriggerEvent(EventManager.EventNames.LevelReset);
		GUIManager.Instance.InGameGui.TogglePrompt(true);
	}

	public void RestartLevel(){
		GameMaster.Instance.PlayerManager.SetPlayerScore(0);
		GameMaster.Instance.PlayerManager.SetPlayerLives(3);

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
		SceneManager.sceneLoaded += GUIManager.Instance.SceneChange;
		SceneManager.sceneLoaded += GameMaster.Instance.GameObjectManager.SceneChange;
		SceneManager.sceneLoaded += GameMaster.Instance.PlayerManager.SceneChange;
		SceneManager.sceneLoaded += AssetBundleManager.Instance.SceneChange;
	}

	void DisableDelegates(){
		SceneManager.sceneLoaded -= SceneChange;
		SceneManager.sceneLoaded -= GUIManager.Instance.SceneChange;
		SceneManager.sceneLoaded -= GameMaster.Instance.GameObjectManager.SceneChange;
		SceneManager.sceneLoaded -= GameMaster.Instance.PlayerManager.SceneChange;
		SceneManager.sceneLoaded -= AssetBundleManager.Instance.SceneChange;
	}

	void SceneChange(Scene scene, LoadSceneMode mode){
		if(scene.name == "MainMenu"){
			EventManager.TriggerEvent(EventManager.EventNames.LevelFinish);

			if(MusicPlayer.instance.isPlaying)
				MusicPlayer.instance.StopMusic();

			MusicPlayer.instance.MenuMusicSet();
			MusicPlayer.instance.StartMusic();

			OptionsManager.instance.SetAudioClip();
		} else if(scene.name.Contains("Level")){
			PrefsManager.SetCurrentLevel(GetLevelNum());

			if(GetLevelNum() > PrefsManager.GetLatestCheckpoint()){
				PrefsManager.SetLatestCheckpoint(GetLevelNum());
				PrefsManager.SetLevelUnlocked(GetLevelNum());
			}
		} else if(scene.name != "Splash"){
			EventManager.TriggerEvent(EventManager.EventNames.LevelFinish);
			StartCoroutine(ResourceManager.UnloadUnusedResources());
		}
	}

	#endregion
}
