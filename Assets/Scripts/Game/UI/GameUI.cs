using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	private GameMaster gameMaster;
	private Text scoreText;
	private int curScore = 0;
	private Text timeText;
	private Text livesText;
	private Image livesImage;
	private int elapsedTime;
	private int livesRemaining;
	
	void Start(){
		elapsedTime = 0;
		gameMaster = (GameMaster)FindObjectOfType<GameMaster>();
		scoreText = (Text)GameObject.Find("Score").GetComponent<Text>();
		curScore = gameMaster.totalScore;
		scoreText.text = curScore.ToString();
		timeText = (Text)GameObject.Find("Time").GetComponent<Text>();
		timeText.text = "00:00";
		livesText = (Text)GameObject.Find("Lives").GetComponent<Text>();
		livesRemaining = gameMaster.playerLives;
		UpdateLivesText();
		livesImage = (Image)GameObject.Find("LivesImage").GetComponent<Image>();
		livesImage.color = PrefsManager.GetBallColor();
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
}
