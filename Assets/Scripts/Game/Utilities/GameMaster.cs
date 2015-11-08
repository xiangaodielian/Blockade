using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public int breakableCount = 0;
	public int playerLives = 3;
	public int totalScore = 0;
	
	private MusicPlayer musicPlayer;
	private UIManager uiManager;
	private GameObject playSpace;
	private GameObject winMenu;
	private GameObject loseMenu;
	private GameObject highScoresMenu;
	private string curLevel;
	private Paddle paddle = null;
	private bool inGame = false;
	
	void Awake(){
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	void Start(){
		musicPlayer = GetComponentInChildren<MusicPlayer>();
		uiManager = GetComponentInChildren<UIManager>();
		playSpace = GameObject.FindGameObjectWithTag("PlaySpace");
		playSpace.SetActive(false);
		winMenu = GameObject.FindGameObjectWithTag("WinMenu");
		winMenu.SetActive(false);
		loseMenu = GameObject.FindGameObjectWithTag("LoseMenu");
		loseMenu.SetActive(false);
		highScoresMenu = GameObject.FindGameObjectWithTag("HighScoresMenu");
		highScoresMenu.SetActive(false);
		curLevel = LevelManager.GetCurrentLevel();
		PerformChecks();
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape) && inGame){
			uiManager.ToggleInGameMenu();
		}
		
		// New Level Loaded
		if(curLevel != LevelManager.GetCurrentLevel()){
			curLevel = LevelManager.GetCurrentLevel();
			PerformChecks();
		}
	}
	
	// Checks for duplicate MusicPlayer, Camera, PlaySpace, and GameMaster
	void PerformChecks(){
		GameMasterCheck();
		MusicPlayerCheck();
		CameraCheck();
		PlaySpaceCheck();
		LevelCheck(curLevel);
	}

	// Check for duplicate GameMasters. Destroy if found
	void GameMasterCheck()
	{
		GameMaster[] otherMasters = GameObject.FindObjectsOfType<GameMaster>();
		foreach(GameMaster master in otherMasters){
			if(master != this)
				Destroy(master.gameObject);
		}
	}

	// Checks for duplicate MusicPlayers. Destroys all non-children.
	void MusicPlayerCheck(){
		MusicPlayer[] otherPlayers = FindObjectsOfType<MusicPlayer>();
		if(otherPlayers.Length >= 2){
			foreach(MusicPlayer player in otherPlayers){
				if(player.transform.parent != this.transform)
					Destroy(player.gameObject);
			}
		}
	}
	
	// Checks for duplicate Cameras. Destroys all non-children.
	void CameraCheck(){
		Camera[] otherCameras = FindObjectsOfType<Camera>();
		if(otherCameras.Length >= 2){
			foreach(Camera cameras in otherCameras){
				if(cameras.transform.parent != this.transform)
					Destroy(cameras.gameObject);
			}
		}
	}

	// Check for multiple PlaySpaces, Destroy all non-children. Add as child if not already
	void PlaySpaceCheck(){
		GameObject[] playSpaces = GameObject.FindGameObjectsWithTag("PlaySpace");
		if(playSpaces.Length > 1){
			foreach(GameObject obj in playSpaces){
				if(obj.transform.parent != this.transform)
					Destroy(obj);
			}
		}
	}
	
	// Check the Level and load appropriate UI
	void LevelCheck(string level){
		Text scoreText;
		switch(level){
			case "Splash":
				uiManager.CloseAll();
				playSpace.SetActive(false);
				winMenu.SetActive(false);
				loseMenu.SetActive(false);
				highScoresMenu.SetActive(false);
				inGame = false;
				break;
			
			case "MainMenu":
				uiManager.CloseAll();
				uiManager.OpenMainMenu();
				playSpace.SetActive(false);
				winMenu.SetActive(false);
				loseMenu.SetActive(false);
				highScoresMenu.SetActive(false);
				if(!musicPlayer.isPlaying)
					musicPlayer.StartMusic();
				inGame = false;
				totalScore = 0;
				playerLives = 3;
				break;
				
			case "Win":
				uiManager.CloseAll();
				playSpace.SetActive(false);
				winMenu.SetActive(true);
				inGame = false;
				scoreText = (Text)GameObject.Find("Score").GetComponent<Text>();
				scoreText.text = "YOUR SCORE: " +totalScore;
				break;
				
			case "Lose":
				uiManager.CloseAll();
				playSpace.SetActive(false);
				loseMenu.SetActive(true);
				inGame = false;
				scoreText = (Text)GameObject.Find("Score").GetComponent<Text>();
				scoreText.text = "YOUR SCORE: " +totalScore;
				break;
				
			default:
				uiManager.CloseAll();
				uiManager.OpenInGameUI();
				playSpace.SetActive(true);
				winMenu.SetActive(false);
				loseMenu.SetActive(false);
				highScoresMenu.SetActive(false);
				inGame = true;
				uiManager.LaunchPromptOn();
				if(paddle == null)
					paddle = (Paddle)GameObject.FindGameObjectWithTag("Player").GetComponent<Paddle>();
				if(PrefsManager.GetLevelNumber() % 5 == 0){
					PrefsManager.SetLatestCheckpoint(PrefsManager.GetLevelNumber());
					PrefsManager.SetScoreAtCheckpoint(totalScore);
				}
				if(PrefsManager.GetLevelNumber() >= PrefsManager.GetLatestCheckpoint())
					PrefsManager.SetLevelUnlocked(PrefsManager.GetLevelNumber());
				break;
		}
	}
	
	public void GameStart(){
		uiManager.LaunchPromptOff();
	}
	
	public void GamePause(){
		Time.timeScale = Mathf.Abs(Time.timeScale - 1f);
		paddle.gamePaused = !paddle.gamePaused;
	}
	
	public void BrickDestroyed(int pointValue){
		totalScore += pointValue;
		breakableCount--;
		if(breakableCount <= 0){
			GamePause();
			uiManager.EndLevelMenu();
		}
	}
	
	public void ChangeToLevel(string level){
		breakableCount = 0;
		if(level == "LatestCheckpoint"){
			totalScore = PrefsManager.GetScoreAtCheckpoint();
			int latestCheckpoint = PrefsManager.GetLatestCheckpoint();
			if(latestCheckpoint < 10)
				level = "Level_0"+latestCheckpoint;
			else
				level = "Level_"+latestCheckpoint;
				
			LevelManager.LoadLevel(level);
		}else if(level == "Next")
			LevelManager.LoadNextLevel();
		else{
			totalScore = 0;
			LevelManager.LoadLevel(level);
		}
	}
	
	public void ResetCurrentLevel(){
		paddle.ResetBall();
		uiManager.LaunchPromptOn();
	}
	
	public void RestartGame(){
		totalScore = PrefsManager.GetScoreAtCheckpoint();
		playerLives = 3;
		string levelName = "";
		int latestCheckpoint = PrefsManager.GetLatestCheckpoint();
		if(latestCheckpoint < 10)
			levelName = "Level_0"+latestCheckpoint;
		else
			levelName = "Level_"+latestCheckpoint;
			
		ChangeToLevel(levelName);
	}
	
	public void ShowHighScores(){
		uiManager.CloseAll();
		winMenu.SetActive(false);
		loseMenu.SetActive(false);
		highScoresMenu.SetActive(true);
	}
	
	public void HideHighScores(){
		highScoresMenu.SetActive(false);
		uiManager.OpenMainMenu();
		ChangeToLevel("MainMenu");
	}
	
	public void QuitRequest(){
		LevelManager.QuitApplication();
	}
}