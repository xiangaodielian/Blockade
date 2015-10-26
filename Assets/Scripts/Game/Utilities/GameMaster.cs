using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public int breakableCount = 0;
	public int playerLives = 3;
	public int totalScore = 0;
	
	[SerializeField] private GameObject musicPlayerPrefab = null;
	[SerializeField] private GameObject cameraPrefab = null;
	
	private MusicPlayer musicPlayer;
	private Camera mainCamera;
	private string curLevel;
	
	void Awake(){
		GameMasterCheck();
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	void Start(){
		MusicPlayerCheck();
		CameraCheck();
		curLevel = LevelManager.GetCurrentLevel();
	}
	
	void Update(){
		MusicPlayerCheck();
		CameraCheck();
		if(curLevel != LevelManager.GetCurrentLevel())
			curLevel = LevelManager.GetCurrentLevel();
			
		if(LevelManager.GetCurrentLevel() == "Win" || LevelManager.GetCurrentLevel() == "Lose"){
			Text scoreText = (Text)GameObject.Find("Score").GetComponent<Text>();
			scoreText.text = "YOUR SCORE: " +totalScore;
		}
	}

	// Checks for other GameMasters on Awake. If others found, Destroys Self
	void GameMasterCheck()
	{
		GameMaster[] otherMasters = FindObjectsOfType<GameMaster>();
		if(otherMasters.Length >= 2)
			Destroy(this.gameObject);
	}

	// Checks for multiple MusicPlayers. Destroys all non-children.
	void MusicPlayerCheck(){
		MusicPlayer[] otherPlayers = FindObjectsOfType<MusicPlayer>();
		if(otherPlayers.Length >= 2){
			foreach(MusicPlayer player in otherPlayers){
				if(player.transform.parent != this.transform)
					Destroy(player.gameObject);
			}
		}
		
		musicPlayer = (MusicPlayer)FindObjectOfType<MusicPlayer>();
		if(musicPlayer && musicPlayer.transform.parent != this.transform)
			musicPlayer.transform.parent = this.transform;
		else if(!musicPlayer){
			GameObject musicPlayerObj = (GameObject)Instantiate (musicPlayerPrefab, Vector3.zero, Quaternion.identity);
			musicPlayer = musicPlayerObj.GetComponent<MusicPlayer>();
			musicPlayer.transform.parent = this.transform;
		}
	}
	
	// Checks for multiple Cameras. Destroys all non-children.
	void CameraCheck(){
		Camera[] otherCameras = FindObjectsOfType<Camera>();
		if(otherCameras.Length >= 2){
			foreach(Camera cameras in otherCameras){
				if(cameras.transform.parent != this.transform)
					Destroy(cameras.gameObject);
			}
		}
		
		mainCamera = FindObjectOfType<Camera>();
		if(mainCamera && mainCamera.transform.parent != this.transform)
			mainCamera.transform.parent = this.transform;
		else if(!mainCamera){
			GameObject mainCameraObj = (GameObject)Instantiate(cameraPrefab);
			mainCamera = mainCameraObj.GetComponent<Camera>();
			mainCamera.transform.parent = this.transform;
		}
		
		GameObject.DontDestroyOnLoad(mainCamera.gameObject);
	}
	
	public void BrickDestroyed(int pointValue){
		totalScore += pointValue;
		breakableCount--;
		if(breakableCount <= 0){
			LevelManager.LoadNextLevel();
			breakableCount = 0;
		}
	}
	
	public void ChangeToLevel(string level){
		GameObject.FindObjectOfType<GameMaster>().breakableCount = 0;
		LevelManager.LoadLevel(level);
	}
	
	public void ResetCurrentLevel(){
		GameObject.FindObjectOfType<GameMaster>().breakableCount = 0;
		LevelManager.ReloadLevel();
	}
	
	public void RestartGame(){
		GameObject.FindObjectOfType<GameMaster>().totalScore = 0;
		ChangeToLevel("Level_01");
	}
	
	public void QuitRequest(){
		LevelManager.QuitApplication();
	}
}
