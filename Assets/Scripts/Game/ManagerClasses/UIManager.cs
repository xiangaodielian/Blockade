/*----------------------------/
  UIManager Class - Blockade
  Manages all GUI elements and
  their functions
  Writen by Joe Arthur
  Latest Revision - 8 May, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
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
		public GameObject menuTransitionPanel = null;
		public GameObject quitConfirmPanel = null;
		public GameObject interviewConfirmPanel = null;
		public GameObject interviewerTutorialPanel = null;
	}

	[System.Serializable] private class GUISounds{
		public string buttonHighlight = "";
		public string buttonClick = "";
		public string inGameMenuOpen = "";
		public string inGameMenuClose = "";
	}

	[SerializeField] private MenuPrefabs menuPrefabs = null;
	[SerializeField] private GUISounds guiSounds = null;

	private GameObject mainMenu = null;
	private GameObject levelSelectMenu = null;
	private GameObject optionsMenu = null;
	private GameObject highScoresMenu = null;
	private GameObject winMenu = null;
	private GameObject loseMenu = null;
	private GameObject quitConfirm = null;
	private GameObject interviewConfirm = null;
	private GameObject interviewerTutorialPanel = null;
	private SceneFader menuTransitionPanel = null;
	private DebugUI debugUI = null;
	private AudioSource audioSource;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();

		InstantiatePrefabs();
	}
	
	void Start(){
		OpenMainMenu();
	}
	
	#endregion
	#region Utility Functions

	void InstantiatePrefabs(){

		mainMenu = Instantiate(menuPrefabs.mainMenu);
		mainMenu.transform.SetParent(this.transform);
		mainMenu.transform.localScale = Vector3.one;
		mainMenu.transform.localPosition = Vector3.zero;

		levelSelectMenu = Instantiate(menuPrefabs.levelSelectMenu);
		levelSelectMenu.transform.SetParent(this.transform);
		levelSelectMenu.transform.localScale = Vector3.one;
		levelSelectMenu.transform.localPosition = Vector3.zero;

		optionsMenu = Instantiate(menuPrefabs.optionsMenu);
		optionsMenu.transform.SetParent(this.transform);
		optionsMenu.transform.localScale = Vector3.one;
		optionsMenu.transform.localPosition = Vector3.zero;

		highScoresMenu = Instantiate(menuPrefabs.highScoresMenu);
		highScoresMenu.transform.SetParent(this.transform);
		highScoresMenu.transform.localScale = Vector3.one;
		highScoresMenu.transform.localPosition = Vector3.zero;

		winMenu = Instantiate(menuPrefabs.winMenu);
		winMenu.transform.SetParent(this.transform);
		winMenu.transform.localScale = Vector3.one;
		winMenu.transform.localPosition = Vector3.zero;

		loseMenu = Instantiate(menuPrefabs.loseMenu);
		loseMenu.transform.SetParent(this.transform);
		loseMenu.transform.localScale = Vector3.one;
		loseMenu.transform.localPosition = Vector3.zero;

		if(!InGameUI.instance){
			Instantiate(menuPrefabs.inGameUI);
			InGameUI.instance.transform.SetParent(this.transform);
			InGameUI.instance.transform.localScale = Vector3.one;
			InGameUI.instance.transform.localPosition = Vector3.zero;
		}

		debugUI = Instantiate(menuPrefabs.debugUI).GetComponent<DebugUI>();
		debugUI.transform.SetParent(this.transform);
		debugUI.transform.localScale = Vector3.one;
		debugUI.transform.localPosition = Vector3.zero;

		menuTransitionPanel = Instantiate(menuPrefabs.menuTransitionPanel).GetComponent<SceneFader>();
		menuTransitionPanel.transform.SetParent(this.transform);
		menuTransitionPanel.transform.localScale = Vector3.one;
		menuTransitionPanel.transform.localPosition = Vector3.zero;

		quitConfirm = Instantiate(menuPrefabs.quitConfirmPanel);
		quitConfirm.transform.SetParent(this.transform);
		quitConfirm.transform.localScale = Vector3.one;
		quitConfirm.transform.localPosition = new Vector3(0f, -45f, 0f);

		interviewConfirm = Instantiate(menuPrefabs.interviewConfirmPanel);
		interviewConfirm.transform.SetParent(this.transform);
		interviewConfirm.transform.localScale = Vector3.one;
		interviewConfirm.transform.localPosition = new Vector3(0f, -45f, 0f);

		interviewerTutorialPanel = Instantiate(menuPrefabs.interviewerTutorialPanel);
		interviewerTutorialPanel.transform.SetParent(this.transform);
		interviewerTutorialPanel.transform.localScale = Vector3.one;
		interviewerTutorialPanel.transform.localPosition = Vector3.zero;
	}

	public void UpdateCheck(){
		mainMenu.GetComponent<MainMenu>().UpdateCheck();
	}

	public void ProceedToLevel(string level, bool async){
		if(async)
			StartCoroutine(GameMaster.instance.ChangeToLevelAsync(level));
		else
			GameMaster.instance.ChangeToLevel(level);
	}
	
	public void ProceedToNextLevel(){
		InGameUI.instance.StopTimer();
		InGameUI.instance.ToggleEndLevelPanel(false);
		GameMaster.instance.GamePause();
		GameMaster.instance.ChangeToLevel("Next");
	}

	public void ReloadPreviousLevel(){
		GameMaster.instance.RestartGame();
	}

	public void QuitRequest(){
		ToggleQuitConfirm(true);
	}

	public void PlayButtonSound(){
		audioSource.clip = ResourceManager.LoadAudioClip(false, guiSounds.buttonClick);
		audioSource.Play();
	}

	public void PlayMenuToggleSound(bool isOpen){
		if(isOpen)
			audioSource.clip = ResourceManager.LoadAudioClip(false, guiSounds.inGameMenuOpen);
		else
			audioSource.clip = ResourceManager.LoadAudioClip(false, guiSounds.inGameMenuClose);

		audioSource.Play();
	}

	#endregion
	#region GUI Management
	
	public void ToggleQuitConfirm(bool visible){
		quitConfirm.SetActive(visible);
	}

	public void ToggleInterviewConfirm(bool visible){
		interviewConfirm.SetActive(visible);	
	}

	public void MenuFadeTransition(string screenToLoad){
		menuTransitionPanel.StartFade(screenToLoad);
	}

	public void OpenMainMenu(){
		CloseAll();
		GameMaster.instance.gameValues.totalScore = 0;
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

	public void OpenInterviewerTutorial(){
		CloseAll();
		interviewerTutorialPanel.SetActive(true);
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
		GameMaster.instance.allowStart = true;
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
		quitConfirm.SetActive(false);
		interviewConfirm.SetActive(false);
		interviewerTutorialPanel.SetActive(false);
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