/*----------------------------/
  GameMaster Class - Blockade
  Controlling class for all
  manager classes
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	//Singleton Instance of GameMaster
	public static GameMaster instance {get; private set;}
	
	public int breakableCount = 0;			//Number of Breakable Bricks in level
	public int playerLives = 3;				//Remaining Player Lives
	public int totalScore = 0;				//Accumulated Score
	
	private GameObject playSpace;			//Ref to Playable area (Walls, Background, etc)
	private string curLevel;
	private Paddle paddle = null;
	private bool inGame = false;			//FALSE - in Menu Screens TRUE - in Game
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	void Start(){
		playSpace = GameObject.FindGameObjectWithTag("PlaySpace");
		playSpace.SetActive(false);
		curLevel = LevelManager.GetCurrentLevel();
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape) && inGame){
			UIManager.instance.ToggleInGameMenu();
		}
		
		// New Level Loaded
		if(curLevel != LevelManager.GetCurrentLevel()){
			curLevel = LevelManager.GetCurrentLevel();
			PerformChecks();
		}
	}
	
	//Checks for duplicate Camera, PlaySpace, and Current Level
	private void PerformChecks(){
		CameraCheck();
		PlaySpaceCheck();
		LevelCheck(curLevel);
	}
	
	// Checks for duplicate Cameras. Destroys all non-children.
	private void CameraCheck(){
		Camera[] otherCameras = FindObjectsOfType<Camera>();
		if(otherCameras.Length > 1){
			foreach(Camera cams in otherCameras){
				if(cams.transform.parent != this.transform)
					Destroy(cams.gameObject);
			}
		}
	}

	// Check for multiple PlaySpaces, Destroy all non-children.
	private void PlaySpaceCheck(){
		GameObject[] playSpaces = GameObject.FindGameObjectsWithTag("PlaySpace");
		if(playSpaces.Length > 1){
			foreach(GameObject obj in playSpaces){
				if(obj.transform.parent != this.transform)
					Destroy(obj);
			}
		}
	}
	
	// Check the Level and load appropriate UI
	private void LevelCheck(string level){
		Text scoreText;
		switch(level){
			case "Splash":
				UIManager.instance.CloseAll();
				playSpace.SetActive(false);
				inGame = false;
				break;
			
			case "MainMenu":
				UIManager.instance.OpenMainMenu();
				playSpace.SetActive(false);
				if(!MusicPlayer.instance.isPlaying)
					MusicPlayer.instance.StartMusic();
				inGame = false;
				totalScore = 0;
				playerLives = 3;
				break;
				
			case "Win":
				UIManager.instance.OpenEndGameMenu(level);
				playSpace.SetActive(false);
				inGame = false;
				scoreText = (Text)GameObject.Find("Score").GetComponent<Text>();
				scoreText.text = "YOUR SCORE: " +totalScore;
				break;
				
			case "Lose":
				UIManager.instance.OpenEndGameMenu(level);
				playSpace.SetActive(false);
				inGame = false;
				scoreText = (Text)GameObject.Find("Score").GetComponent<Text>();
				scoreText.text = "YOUR SCORE: " +totalScore;
				break;
				
			//In Game
			default:
				UIManager.instance.OpenInGameUI();
				playSpace.SetActive(true);
				inGame = true;
				UIManager.instance.ToggleLaunchPrompt(true);
				if(paddle == null)
					paddle = (Paddle)GameObject.FindGameObjectWithTag("Player").GetComponent<Paddle>();
				PrefsManager.SetCurrentLevel(PrefsManager.GetLevelNumber());
				if(PrefsManager.GetLevelNumber() > PrefsManager.GetLatestCheckpoint()){
					PrefsManager.SetLatestCheckpoint(PrefsManager.GetLevelNumber());
					PrefsManager.SetLevelUnlocked(PrefsManager.GetLevelNumber());
				}
				break;
		}
	}
	
	public void GameStart(){
		UIManager.instance.ToggleLaunchPrompt(false);
	}
	
	//Pause Playback (Menu Open)
	public void GamePause(){
		Time.timeScale = Mathf.Abs(Time.timeScale - 1f);
		paddle.gamePaused = !paddle.gamePaused;
	}
	
	//Add value of Destroyed Brick to Score and decrement breakableCount
	//If breakableCount hits 0, end Level
	public void BrickDestroyed(int pointValue){
		totalScore += pointValue;
		breakableCount--;
		if(breakableCount <= 0){
			GamePause();
			UIManager.instance.EndLevelMenu();
		}
	}
	
	//Reset breakableCount and load Level "level"
	public void ChangeToLevel(string level){
		breakableCount = 0;
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
	
	//Reset Ball after Player Death
	public void ResetCurrentLevel(){
		paddle.ResetBall();
		UIManager.instance.ToggleLaunchPrompt(true);
	}
	
	//Reset Current Level after Player loses all Lives
	public void RestartGame(){
		totalScore = 0;
		playerLives = 3;
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
}