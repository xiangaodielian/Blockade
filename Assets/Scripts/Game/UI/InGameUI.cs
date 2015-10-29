﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {
	
	private GameMaster gameMaster;
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
	
	void Awake(){
		elapsedTime = 0;
		gameMaster = (GameMaster)FindObjectOfType<GameMaster>();
		scoreText = (Text)GameObject.Find("InGameScore").GetComponent<Text>();
		curScore = gameMaster.totalScore;
		scoreText.text = curScore.ToString();
		timeText = (Text)GameObject.Find("InGameTime").GetComponent<Text>();
		timeText.text = "00:00";
		livesText = (Text)GameObject.Find("InGameLives").GetComponent<Text>();
		livesRemaining = gameMaster.playerLives;
		UpdateLivesText();
		livesImage = (Image)GameObject.Find("InGameLivesImage").GetComponent<Image>();
		livesImage.color = PrefsManager.GetBallColor();
		launchPromptText = (Text)GameObject.Find("LaunchPromptText").GetComponent<Text>();
		menuPanel = GameObject.Find("MenuPanel");
		mainPanel = GameObject.Find("MainPanel");
		optionsPanel = GameObject.Find("OptionsPanel");
		endLevelPanel = GameObject.Find("EndLevelPanel");
		optionsPanel.SetActive(false);
		endLevelPanel.SetActive(false);
		menuPanel.gameObject.SetActive(false);
	}
	
	void Update(){
		UpdateTimeText();
		if(livesRemaining != gameMaster.playerLives)
			UpdateLivesText();
			
		if(curScore != gameMaster.totalScore)
			UpdateScoreText();
	}
	
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
	
	void UpdateLivesText(){
		livesRemaining = gameMaster.playerLives;
		if(livesRemaining < 10)
			livesText.text = "X 0"+livesRemaining.ToString();
		else
			livesText.text = "X "+livesRemaining.ToString();
	}
	
	void UpdateScoreText(){
		curScore = gameMaster.totalScore;
		scoreText.text = curScore.ToString();
	}
	
	public void ResetTimer(){
		elapsedTime = 0;
	}
	
	public void TogglePrompt(bool isOn){
		launchPromptText.gameObject.SetActive(isOn);
	}
	
	public void ToggleMenu(){
		gameMaster.GamePause();
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
	
	public void CalculateTimeBonus(){
		Text timeBonusText = (Text)GameObject.Find("TimeBonusText").GetComponent<Text>();
		int timeBonus = 0;
		if(Time.timeSinceLevelLoad < 1.99f)
			timeBonus = LevelManager.GetLevelNum()*1000;
		else
			timeBonus = (LevelManager.GetLevelNum()*1000)/(elapsedTime/10);
			
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
		
		gameMaster.totalScore += timeBonus;
	}
}
