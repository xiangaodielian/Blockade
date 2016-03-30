/*----------------------------/
  GameMaster Class - Blockade
  Controlling class for all
  manager classes
  Writen by Joe Arthur
  Latest Revision - 27 Mar, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(OptionsController))]
public class GameMaster : MonoBehaviour {
	
	#region variables
	
	//Singleton Instance of GameMaster
	public static GameMaster instance {get; private set;}

	[System.Serializable] private class Prefabs{
		public GameObject splashPrefab = null;
		public GameObject musicPlayerPrefab = null;
		public GameObject cameraPrefab = null;
		public GameObject playSpacePrefab = null;
		public GameObject paddlePrefab = null;
		public GameObject ballPrefab = null;
		public GameObject guiManagerPrefab = null;
		public GameObject starsPSPrefab = null;
	}

	[System.Serializable] public class GameValues{
		[Tooltip("Number of Breakable Bricks in the level.")]
		public int breakableCount = 0;		//Number of Breakable Bricks in level
		public int playerLives = 3;			//Remaining Player Lives
		public int totalScore = 0;			//Accumulated Score
	}

	public GameValues gameValues = null;
	[HideInInspector] public bool allowStart = false;
	[HideInInspector] public bool inGame = false;				//FALSE = in Menu Screens TRUE = in Game
	[HideInInspector] public bool gamePaused = false;
	[HideInInspector] public AsyncOperation async = null;

	[SerializeField] private Prefabs prefabs = null;

	private GameObject stars = null;				//Ref to Stars (Background) Particle System
	private string curLevel;
	private int lifeGrantedAt = 0;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
		DontDestroyOnLoad(gameObject);

		InstantiatePrefabs();
	}
	
	void Start(){
		curLevel = LevelManager.GetCurrentLevel();
		LevelCheck(curLevel);
	}
	
	void Update(){		
		// New Level Loaded
		if(curLevel != LevelManager.GetCurrentLevel()){
			curLevel = LevelManager.GetCurrentLevel();
			LevelCheck(curLevel);
		}

		if(gameValues.totalScore-lifeGrantedAt > 10000){
			if(gameValues.playerLives < 99)
				gameValues.playerLives++;

			lifeGrantedAt = gameValues.totalScore;
		}
	}
	
	#endregion
	#region Utility Functions

	//Instantiates Prefabs for MusicPlayer, Camera, and UIManager, and StarsPS
	//Makes them children of GameMaster to keep them loaded between scenes
	void InstantiatePrefabs(){
		if(!MusicPlayer.instance){
			Instantiate(prefabs.musicPlayerPrefab);
			MusicPlayer.instance.transform.SetParent(this.transform);
		} else{
			if(MusicPlayer.instance.transform.parent != this.transform)
				MusicPlayer.instance.transform.SetParent(this.transform);
		}

		if(!CameraManager.instance){
			Instantiate(prefabs.cameraPrefab);
			CameraManager.instance.transform.SetParent(this.transform);
		} else{
			if(CameraManager.instance.transform.parent != this.transform)
				CameraManager.instance.transform.SetParent(this.transform);
		}

		if(!UIManager.instance){
			Instantiate(prefabs.guiManagerPrefab);
			UIManager.instance.transform.SetParent(this.transform);
		} else{
			if(UIManager.instance.transform.parent != this.transform)
				UIManager.instance.transform.SetParent(this.transform);
		}

		stars = Instantiate(prefabs.starsPSPrefab);
		stars.transform.SetParent(this.transform);
		stars.SetActive(false);
	}

	// Check the Level and load appropriate UI
	private void LevelCheck(string level){
		Ball[] ballsInScene = null;

		switch(level){
			case "Splash":
				Instantiate(prefabs.splashPrefab);
				UIManager.instance.CloseAll();
				inGame = false;
				break;
			
			case "MainMenu":
				UIManager.instance.OpenMainMenu();
				stars.SetActive(true);
				if(PlaySpace.instance)
					Destroy(PlaySpace.instance.gameObject);

				ballsInScene = FindObjectsOfType<Ball>();
				if(ballsInScene.Length > 0){
					foreach(Ball ball in ballsInScene)
						Destroy(ball.gameObject);
				}

				if(Paddle.instance)
					Destroy(Paddle.instance.gameObject);

				if(!MusicPlayer.instance.isPlaying)
					MusicPlayer.instance.StartMusic();

				if(allowStart)
					UIManager.instance.EndGame();

				inGame = false;
				gameValues.totalScore = 0;
				gameValues.playerLives = 3;
				break;
				
			case "Win":
				if(PlaySpace.instance)
					Destroy(PlaySpace.instance.gameObject);

				ballsInScene = FindObjectsOfType<Ball>();
				if(ballsInScene.Length > 0){
					foreach(Ball ball in ballsInScene)
						Destroy(ball.gameObject);
				}

				if(Paddle.instance)
						Destroy(Paddle.instance.gameObject);

				UIManager.instance.EndGame();
				UIManager.instance.OpenEndGameMenu(level);
				inGame = false;
				break;
				
			case "Lose":
				gameValues.playerLives = 3;
				if(PlaySpace.instance)
					Destroy(PlaySpace.instance.gameObject);

				ballsInScene = FindObjectsOfType<Ball>();
				if(ballsInScene.Length > 0){
					foreach(Ball ball in ballsInScene)
						Destroy(ball.gameObject);
				}

				if(Paddle.instance)
						Destroy(Paddle.instance.gameObject);

				UIManager.instance.EndGame();
				UIManager.instance.OpenEndGameMenu(level);
				inGame = false;
				break;
				
			//In Game
			default:
				UIManager.instance.OpenInGameUI();
				if(!PlaySpace.instance){
					UIManager.instance.ToggleLaunchPrompt(false);
					Instantiate(prefabs.playSpacePrefab);
					PlaySpace.instance.transform.SetParent(this.transform);
					PlaySpace.instance.StartTimer();
				} else
					UIManager.instance.BeginGame();

				if(!Paddle.instance)
					Instantiate(prefabs.paddlePrefab);

				ballsInScene = FindObjectsOfType<Ball>();
				if(ballsInScene.Length > 0){
					foreach(Ball ball in ballsInScene)
						Destroy(ball.gameObject);
				}

				Instantiate(prefabs.ballPrefab);

				inGame = true;
				PrefsManager.SetCurrentLevel(PrefsManager.GetLevelNumber());

				if(PrefsManager.GetLevelNumber() > PrefsManager.GetLatestCheckpoint()){
					PrefsManager.SetLatestCheckpoint(PrefsManager.GetLevelNumber());
					PrefsManager.SetLevelUnlocked(PrefsManager.GetLevelNumber());
				}

				if(PrefsManager.GetLevelNumber() > 10)
					PopulatePowerups();

				break;
		}
	}

	#endregion
	#region Gameplay Functions

	//Pause Playback (Menu Open)
	public void GamePause(){
		Time.timeScale = Mathf.Abs(Time.timeScale - 1f);
		gamePaused = !gamePaused;
	}
	
	//Add value of Destroyed Brick to Score and decrement breakableCount
	//If breakableCount hits 0, end Level
	public void BrickDestroyed(int pointValue){
		gameValues.totalScore += pointValue;
		gameValues.breakableCount--;
		if(gameValues.breakableCount <= 0){
			Paddle.instance.hasStarted = false;
			GamePause();
			UIManager.instance.EndLevelMenu();
		}
	}

	void PopulatePowerups(){
		List<Brick> bricks = new List<Brick>();
		bricks.AddRange(FindObjectsOfType<Brick>());

		foreach(Brick brick in bricks){
			if(brick.tag == "Breakable")
				brick.SetPowerup();
		}
	}
	
	#endregion
	#region Level/Application Management
	
	//Reset breakableCount and load Level "level"
	public void ChangeToLevel(string level){
		gameValues.breakableCount = 0;
		if(level == "Next")
			LevelManager.LoadNextLevel();
		else{
			if(level == "LatestCheckpoint"){
				int latestCheckpoint = PrefsManager.GetLatestCheckpoint();
				if(latestCheckpoint < 10)
					level = "Level_0"+latestCheckpoint;
				else
					level = "Level_"+latestCheckpoint;
			}
			LevelManager.LoadLevel(level);
		}
	}

	public IEnumerator ChangeToLevelAsync(string level){
		gameValues.breakableCount = 0;
		if(level == "LatestCheckpoint"){
			int latestCheckpoint = PrefsManager.GetLatestCheckpoint();
			if(latestCheckpoint < 10)
				level = "Level_0"+latestCheckpoint;
			else
				level = "Level_"+latestCheckpoint;
		}

		async = LevelManager.LoadLevelAsync(level);
		while(!async.isDone){
			yield return null;
		}

		async = null;
	}
	
	//Reset Ball after Player Death
	public void ResetCurrentLevel(){
		Paddle.instance.ResetBall();
		UIManager.instance.ToggleLaunchPrompt(true);
	}
	
	//Reset Current Level after Player loses all Lives
	public void RestartGame(){
		gameValues.totalScore = 0;
		gameValues.playerLives = 3;
		string levelName = "";
		int currentLevel = PrefsManager.GetCurrentLevel();
		if(currentLevel < 10)
			levelName = "Level_0"+currentLevel;
		else
			levelName = "Level_"+currentLevel;
			
		ChangeToLevel(levelName);
	}
	
	//Quit Application
	public void QuitRequest(){
		LevelManager.QuitApplication();
	}
	
	#endregion
}