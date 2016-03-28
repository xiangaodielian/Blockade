/*----------------------------/
  InGameUI Class - Blockade
  Controls all GUI visible in-game
  and their functions
  Writen by Joe Arthur
  Latest Revision - 26 Mar, 2016
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
	}

	[SerializeField] private DisplayedGUI displayedGUI = null;
	[SerializeField] private MenuPanelPrefabs menuPanelPrefabs = null;

	private GameObject inGameMainMenuPanel;
	private GameObject optionsPanel;
	private GameObject endLevelPanel;
	private int elapsedTime = 0;
	private int timeDifference = 0;
	private bool runTimer = false;
	private bool promptActive = false;
	
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
		TogglePrompt(false);
		
		#if UNITY_STANDALONE
		displayedGUI.launchPromptText.text = "CLICK TO LAUNCH!";
		#elif UNITY_ANDROID || UNITY_IOS
		displayedGUI.launchPromptText.text = "TAP TO LAUNCH!";
		#elif UNITY_WSA || UNITY_WEBGL
		if(Input.mousePresent)
			displayedGUI.launchPromptText.text = "CLICK TO LAUNCH!";
		else
			displayedGUI.launchPromptText.text = "TAP TO LAUNCH!";
		#endif

		inGameMainMenuPanel = Instantiate(menuPanelPrefabs.inGameMainMenuPanel);
		inGameMainMenuPanel.transform.SetParent(this.transform);
		inGameMainMenuPanel.transform.localPosition = Vector3.zero;
		optionsPanel = Instantiate(menuPanelPrefabs.inGameOptionsPanel);
		optionsPanel.transform.SetParent(this.transform);
		optionsPanel.transform.localPosition = Vector3.zero;
		endLevelPanel = Instantiate(menuPanelPrefabs.endLevelPanel);
		endLevelPanel.transform.SetParent(this.transform);
		endLevelPanel.transform.localPosition = Vector3.zero;

		endLevelPanel.SetActive(false);
		inGameMainMenuPanel.SetActive(false);
		optionsPanel.SetActive(false);
	}
	
	void Update(){
		if(runTimer)
			UpdateTimeText();
		
		if(displayedGUI.livesText.text != GameMaster.instance.gameValues.playerLives.ToString())
			UpdateLivesText();
			
		if(displayedGUI.scoreText.text != GameMaster.instance.gameValues.totalScore.ToString())
			UpdateScoreText();
			
		if(displayedGUI.livesImage.color != PrefsManager.GetBallColor())
			displayedGUI.livesImage.color = PrefsManager.GetBallColor();
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
		GameMaster.instance.GamePause();

		optionsPanel.SetActive(false);
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
		endLevelPanel.SetActive(isOn);
	}
	
	#endregion
	#region Utility Functions

	//Sets the offset for Level Timer
	public void SetTimeDifference(int dif){
		timeDifference = dif;
		runTimer = true;
		GameMaster.instance.allowStart = true;
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