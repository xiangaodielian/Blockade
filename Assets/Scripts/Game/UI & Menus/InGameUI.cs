/*----------------------------/
  InGameUI Class - Blockade
  Controls all GUI visible in-game
  and their functions
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {
	
	private Text scoreText;
	private int curScore = 0;
	private Text timeText;
	private Text livesText;
	private Image livesImage;
	private int elapsedTime;
	private int livesRemaining;
	private Text launchPromptText;
	private GameObject menuPanel;
	private GameObject mainPanel;
	private GameObject optionsPanel;
	private GameObject endLevelPanel;
	private bool promptActive = false;
	
	void Start(){
		elapsedTime = 0;
		scoreText = (Text)GameObject.Find("InGameScore").GetComponent<Text>();
		curScore = GameMaster.instance.totalScore;
		scoreText.text = curScore.ToString();
		timeText = (Text)GameObject.Find("InGameTime").GetComponent<Text>();
		timeText.text = "00:00";
		livesText = (Text)GameObject.Find("InGameLives").GetComponent<Text>();
		livesRemaining = GameMaster.instance.playerLives;
		UpdateLivesText();
		livesImage = (Image)GameObject.Find("InGameLivesImage").GetComponent<Image>();
		livesImage.color = PrefsManager.GetBallColor();
		launchPromptText = (Text)GameObject.Find("LaunchPromptText").GetComponent<Text>();
		
		#if UNITY_STANDALONE
		launchPromptText.text = "CLICK TO LAUNCH!";
		#elif UNITY_ANDROID || UNITY_IOS
		launchPromptText.text = "TAP TO LAUNCH!";
		#elif UNITY_WSA || UNITY_WEBGL
		if(Input.mousePresent)
			launchPromptText.text = "CLICK TO LAUNCH!";
		else
			launchPromptText.text = "TAP TO LAUNCH!";
		#endif
		
		menuPanel = GameObject.Find("MenuPanel");
		mainPanel = GameObject.Find("MainMenuPanel");
		optionsPanel = GameObject.Find("OptionsPanel");
		endLevelPanel = GameObject.Find("EndLevelPanel");
		mainPanel.SetActive(false);
		optionsPanel.SetActive(false);
		endLevelPanel.SetActive(false);
		menuPanel.gameObject.SetActive(false);
	}
	
	void Update(){
		UpdateTimeText();
		
		if(livesRemaining != GameMaster.instance.playerLives)
			UpdateLivesText();
			
		if(curScore != GameMaster.instance.totalScore)
			UpdateScoreText();
			
		if(livesImage.color != PrefsManager.GetBallColor())
			livesImage.color = PrefsManager.GetBallColor();
	}
	
	//Update Time Played Display for Current Level
	void UpdateTimeText(){
		elapsedTime = (int)Time.timeSinceLevelLoad;
		int elapsedSec = elapsedTime % 60;
		int elapsedMin = elapsedTime / 60;
		if(elapsedMin < 10){
			if(elapsedSec < 10)
				timeText.text = "0"+elapsedMin+":"+"0"+elapsedSec;
			else
				timeText.text = "0"+elapsedMin+":"+elapsedSec;
		} else{
			if(elapsedSec < 10)
				timeText.text = elapsedMin+":"+"0"+elapsedSec;
			else
				timeText.text = elapsedMin+":"+elapsedSec;
		}
	}
	
	//Update Lives Remaining Display
	void UpdateLivesText(){
		livesRemaining = GameMaster.instance.playerLives;
		if(livesRemaining < 10)
			livesText.text = "X 0"+livesRemaining.ToString();
		else
			livesText.text = "X "+livesRemaining.ToString();
	}
	
	//Update Current Score Display
	void UpdateScoreText(){
		curScore = GameMaster.instance.totalScore;
		scoreText.text = curScore.ToString();
	}
	
	public void TogglePrompt(bool isOn){
		launchPromptText.gameObject.SetActive(isOn);
	}
	
	//Toggle In Game Menu
	public void ToggleMenu(){
		GameMaster.instance.GamePause();
		if(promptActive){
			launchPromptText.gameObject.SetActive(true);
			promptActive = false;
		}
		if(launchPromptText.gameObject.activeSelf && !menuPanel.gameObject.activeSelf){
			launchPromptText.gameObject.SetActive(false);
			promptActive = true;
		}
		
		mainPanel.SetActive(true);
		menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
	}
	
	public void InGameOptions(){
		mainPanel.SetActive(false);
		optionsPanel.SetActive(true);
		optionsPanel.GetComponent<OptionsController>().SetSliders();
	}
	
	public void InGameMain(){
		optionsPanel.SetActive(false);
		mainPanel.SetActive(true);
	}
	
	public void ToggleEndLevelPanel(bool isOn){
		endLevelPanel.SetActive(isOn);
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
		
		GameMaster.instance.totalScore += timeBonus;
	}
}