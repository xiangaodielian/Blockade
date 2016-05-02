/*----------------------------/
  InGameUI Class - Blockade
  Controls all GUI visible in-game
  and their functions
  Writen by Joe Arthur
  Latest Revision - 2 May, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {
	
	#region Variables
	
	//Singleton instance for InGameUI
	public static InGameUI instance {get; private set;}

	[System.Serializable] private class DisplayedGUI{
		public Text scoreText = null;
		public Text timeText = null;
		public Text livesText = null;
		public Text launchPromptText = null;
		public Image livesImage = null;
	}

	[System.Serializable] private class MenuPanelPrefabs{
		public GameObject inGameMainMenuPanel = null;
		public GameObject inGameOptionsPanel = null;
		public GameObject endLevelPanel = null;
		public GameObject inGameTutorials = null;
	}

	[SerializeField] private DisplayedGUI displayedGUI = null;
	[SerializeField] private MenuPanelPrefabs menuPanelPrefabs = null;

	private GameObject inGameMainMenuPanel;
	private GameObject optionsPanel;
	private GameObject endLevelPanel;
	private GameObject inGameTutorials;
	private int elapsedTime = 0;
	private int timeDifference = 0;
	private bool runTimer = false;
	private bool promptActive = false;
	private bool displayTutorial = true;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		UpdateScoreText();
		displayedGUI.timeText.text = "00:00";
		UpdateLivesText();
		displayedGUI.livesImage.color = PrefsManager.GetBallColor();

		InstantiatePrefabs();

		TogglePrompt(false);
		endLevelPanel.SetActive(false);
		inGameMainMenuPanel.SetActive(false);
		optionsPanel.SetActive(false);
		inGameTutorials.SetActive(false);
	}
	
	void Update(){
		if(!PrefsManager.GetMouseControl()){
			displayedGUI.launchPromptText.text = "PRESS SPACE TO LAUNCH!";
			displayedGUI.launchPromptText.fontSize = 45;
		} else{
			if(Input.mousePresent)
				displayedGUI.launchPromptText.text = "CLICK TO LAUNCH!";
			else
				displayedGUI.launchPromptText.text = "TAP TO LAUNCH!";

			displayedGUI.launchPromptText.fontSize = 60;
		}

		if(runTimer)
			UpdateTimeText();
		
		if(displayedGUI.livesText.text != GameMaster.instance.gameValues.playerLives.ToString())
			UpdateLivesText();
			
		if(displayedGUI.scoreText.text != GameMaster.instance.gameValues.totalScore.ToString())
			UpdateScoreText();
			
		if(displayedGUI.livesImage.color != PrefsManager.GetBallColor())
			displayedGUI.livesImage.color = PrefsManager.GetBallColor();

		if(PrefsManager.GetCurrentLevel() == 1 && displayTutorial && PrefsManager.GetLevelUnlocked() <= PrefsManager.GetCurrentLevel()){
			ToggleInGameTutorials(true, 0);
			displayTutorial = false;
		}

		if(PrefsManager.GetCurrentLevel() == 2 && !displayTutorial)
			displayTutorial = true;

		if(PrefsManager.GetCurrentLevel() == 5 && displayTutorial && PrefsManager.GetLevelUnlocked() <= PrefsManager.GetCurrentLevel()){
			ToggleInGameTutorials(true, 3);
			displayTutorial = false;
		}
	}
	
	#endregion
	#region GUI Functions
	
	//Update Time Played Display for Current Level
	private void UpdateTimeText(){
		elapsedTime = (int)Time.timeSinceLevelLoad - timeDifference;
		int elapsedSec = elapsedTime % 60;
		int elapsedMin = elapsedTime / 60;
		if(elapsedMin < 10){
			if(elapsedSec < 10)
				displayedGUI.timeText.text = "0"+elapsedMin+":"+"0"+elapsedSec;
			else
				displayedGUI.timeText.text = "0"+elapsedMin+":"+elapsedSec;
		} else{
			if(elapsedSec < 10)
				displayedGUI.timeText.text = elapsedMin+":"+"0"+elapsedSec;
			else
				displayedGUI.timeText.text = elapsedMin+":"+elapsedSec;
		}
	}
	
	//Update Lives Remaining Display
	private void UpdateLivesText(){
		if(GameMaster.instance.gameValues.playerLives < 10)
			displayedGUI.livesText.text = "X 0"+GameMaster.instance.gameValues.playerLives.ToString();
		else
			displayedGUI.livesText.text = "X "+GameMaster.instance.gameValues.playerLives.ToString();
	}
	
	//Update Current Score Display
	private void UpdateScoreText(){
		displayedGUI.scoreText.text = GameMaster.instance.gameValues.totalScore.ToString();
	}
	
	public void TogglePrompt(bool isOn){
		displayedGUI.launchPromptText.gameObject.SetActive(isOn);
	}
	
	//Toggle In Game Menu
	public void ToggleMenu(){
		if(!endLevelPanel.activeSelf && !inGameTutorials.activeSelf)
			GameMaster.instance.GamePause();

		if(!inGameMainMenuPanel.activeSelf && !optionsPanel.activeSelf)
			UIManager.instance.PlayMenuToggleSound(true);
		else
			UIManager.instance.PlayMenuToggleSound(false);

		if(optionsPanel.activeSelf)
			optionsPanel.SetActive(false);
		else
			inGameMainMenuPanel.SetActive(!inGameMainMenuPanel.activeSelf);

		if(promptActive){
			displayedGUI.launchPromptText.gameObject.SetActive(true);
			promptActive = false;
		}

		if(displayedGUI.launchPromptText.gameObject.activeSelf && inGameMainMenuPanel.activeSelf){
			displayedGUI.launchPromptText.gameObject.SetActive(false);
			promptActive = true;
		}
	}
	
	public void InGameOptions(){
		inGameMainMenuPanel.SetActive(false);
		optionsPanel.SetActive(true);
	}
	
	public void InGameMain(){
		optionsPanel.SetActive(false);
		inGameMainMenuPanel.SetActive(true);
	}
	
	public void ToggleEndLevelPanel(bool isOn){
		if(!inGameMainMenuPanel.activeSelf && !optionsPanel.activeSelf && isOn)
			GameMaster.instance.GamePause();

		endLevelPanel.SetActive(isOn);
	}

	public void ToggleInGameTutorials(bool visible, int tutNum){
		GameMaster.instance.GamePause();
		inGameTutorials.SetActive(visible);

		if(visible)
			inGameTutorials.GetComponent<TutorialGUI>().SetTutorial(tutNum);
			
	}
	
	#endregion
	#region Utility Functions

	void InstantiatePrefabs(){
		inGameMainMenuPanel = Instantiate(menuPanelPrefabs.inGameMainMenuPanel);
		inGameMainMenuPanel.transform.SetParent(this.transform);
		inGameMainMenuPanel.transform.localPosition = Vector3.zero;

		optionsPanel = Instantiate(menuPanelPrefabs.inGameOptionsPanel);
		optionsPanel.transform.SetParent(this.transform);
		optionsPanel.transform.localPosition = Vector3.zero;

		endLevelPanel = Instantiate(menuPanelPrefabs.endLevelPanel);
		endLevelPanel.transform.SetParent(this.transform);
		endLevelPanel.transform.localPosition = Vector3.zero;

		inGameTutorials = Instantiate(menuPanelPrefabs.inGameTutorials);
		inGameTutorials.transform.SetParent(this.transform);
		inGameTutorials.transform.localPosition = Vector3.zero;
	}

	//Sets the offset for Level Timer
	public void SetTimeDifference(int dif){
		timeDifference = dif;
		runTimer = true;
	}

	public void StopTimer(){
		runTimer = false;
		timeDifference = (int)Time.timeSinceLevelLoad;
		UpdateTimeText();
		GameMaster.instance.allowStart = false;
	}

	//Calculate Bonus Points for Level based on Time in Level
	public void CalculateTimeBonus(){
		Text timeBonusText = (Text)GameObject.Find("TimeBonusText").GetComponent<Text>();
		int timeBonus = 0;
		if(Time.timeSinceLevelLoad < 1.99f)
			timeBonus = LevelManager.GetLevelNum()*1000;
		else{
			timeBonus = (LevelManager.GetLevelNum()*1000)/Mathf.Clamp((elapsedTime/10),1,1000);	
		}
			
		int elapsedSec = elapsedTime % 60;
		int elapsedMin = elapsedTime / 60;
		if(elapsedMin < 10){
			if(elapsedSec < 10)
				timeBonusText.text = "0"+elapsedMin+":"+"0"+elapsedSec+" = "+timeBonus;
			else
				timeBonusText.text = "0"+elapsedMin+":"+elapsedSec+" = "+timeBonus;
		} else{
			if(elapsedSec < 10)
				timeBonusText.text = elapsedMin+":"+"0"+elapsedSec+" = "+timeBonus;
			else
				timeBonusText.text = elapsedMin+":"+elapsedSec+" = "+timeBonus;
		}
		
		GameMaster.instance.gameValues.totalScore += timeBonus;
	}

	#endregion
}