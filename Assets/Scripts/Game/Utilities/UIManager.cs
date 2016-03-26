/*----------------------------/
  UIManager Class - Blockade
  Manages all GUI elements and
  their functions
  Writen by Joe Arthur
  Latest Revision - 25 Mar, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	
	#region variables
	
	//Singleton instance for UIManager
	public static UIManager instance {get; private set;}
	
	[System.Serializable] private class MenuPrefabs{
		public GameObject mainMenu = null;
		public GameObject levelSelectMenu = null;
		public GameObject optionsMenu = null;
		public GameObject highScoresMenu = null;
		public GameObject winMenu = null;
		public GameObject loseMenu = null;
		public GameObject inGameUI = null;
		public GameObject debugUI = null;
	}

	[SerializeField] private MenuPrefabs menuPrefabs = null;

	private GameObject mainMenu = null;
	private GameObject levelSelectMenu = null;
	private GameObject optionsMenu = null;
	private GameObject highScoresMenu = null;
	private GameObject winMenu = null;
	private GameObject loseMenu = null;
	private DebugUI debugUI = null;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		InstantiatePrefabs();
	}
	
	void Start(){
		CloseAll();
	}
	
	#endregion
	#region Utility Functions

	void InstantiatePrefabs(){
		mainMenu = Instantiate(menuPrefabs.mainMenu);
		mainMenu.transform.SetParent(this.transform);

		levelSelectMenu = Instantiate(menuPrefabs.levelSelectMenu);
		levelSelectMenu.transform.SetParent(this.transform);

		optionsMenu = Instantiate(menuPrefabs.optionsMenu);
		optionsMenu.transform.SetParent(this.transform);

		highScoresMenu = Instantiate(menuPrefabs.highScoresMenu);
		highScoresMenu.transform.SetParent(this.transform);

		winMenu = Instantiate(menuPrefabs.winMenu);
		winMenu.transform.SetParent(this.transform);

		loseMenu = Instantiate(menuPrefabs.loseMenu);
		loseMenu.transform.SetParent(this.transform);

		if(!InGameUI.instance){
			Instantiate(menuPrefabs.inGameUI);
			InGameUI.instance.transform.SetParent(this.transform);
		}

		debugUI = Instantiate(menuPrefabs.debugUI).GetComponent<DebugUI>();
		debugUI.transform.SetParent(this.transform);
	}

	public void ProceedToLevel(string level, bool async){
		if(async)
			StartCoroutine(GameMaster.instance.ChangeToLevelAsync(level));
		else
			GameMaster.instance.ChangeToLevel(level);
	}
	
	public void ProceedToNextLevel(){
		InGameUI.instance.ToggleEndLevelPanel(false);
		GameMaster.instance.GamePause();
		GameMaster.instance.ChangeToLevel("Next");
	}

	public void ReloadPreviousLevel(){
		GameMaster.instance.RestartGame();
	}

	public void QuitRequest(){
		LevelManager.QuitApplication();
	}

	#endregion
	#region GUI Management
	
	public void OpenMainMenu(){
		CloseAll();
		mainMenu.SetActive(true);
	}
	
	public void OpenLevelSelectMenu(){
		CloseAll();
		levelSelectMenu.SetActive(true);
	}
	
	public void OpenOptionsMenu(){
		CloseAll();
		optionsMenu.SetActive(true);
	}
	
	public void OpenHighScoreMenu(){
		CloseAll();
		highScoresMenu.SetActive(true);
	}
	
	public void OpenInGameUI(){
		CloseAll();
		InGameUI.instance.gameObject.SetActive(true);
	}
	
	public void OpenEndGameMenu(string level){
		CloseAll();
		if(level == "Win")
			winMenu.SetActive(true);
		else
			loseMenu.SetActive(true);
			
		Text scoreText = (Text)GameObject.Find("Score").GetComponent<Text>();
		scoreText.text = "YOUR SCORE: " +GameMaster.instance.gameValues.totalScore;
	}
	
	public void ToggleInGameMenu(){
		InGameUI.instance.ToggleMenu();
	}

	public void ToggleDebugConsole(){
		debugUI.ToggleDebugConsole();
	}

	public void SendDebugCommand(string input){
		debugUI.ProcessCommand(input);
	}

	public void BeginGame(){
		InGameUI.instance.SetTimeDifference((int)Time.timeSinceLevelLoad);
		ToggleLaunchPrompt(true);
	}

	public void EndGame(){
		InGameUI.instance.StopTimer();
	}
	
	public void CloseAll(){
		mainMenu.SetActive(false);
		levelSelectMenu.SetActive(false);
		optionsMenu.SetActive(false);
		highScoresMenu.SetActive(false);
		InGameUI.instance.gameObject.SetActive(false);
		winMenu.SetActive(false);
		loseMenu.SetActive(false);
	}
	
	public void ToggleLaunchPrompt(bool visible){
		InGameUI.instance.TogglePrompt(visible);
	}
	
	public void EndLevelMenu(){
		InGameUI.instance.ToggleEndLevelPanel(true);
		InGameUI.instance.CalculateTimeBonus();
	}
	
	#endregion
}