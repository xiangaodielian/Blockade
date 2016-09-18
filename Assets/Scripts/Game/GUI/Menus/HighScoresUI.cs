/*------------------------------/
  HighScoresUI Class - Blockade
  Manages High Score page and
  updates High Score data
  Writen by Joe Arthur
  Latest Revision - 3 Apr, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoresUI : MonoBehaviour {
	
	#region Variables
	
	[System.Serializable] private class Buttons{
		public Button continueButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	private Text[] highScores = new Text[5];
	private int replaceScore = 0;
	private bool enterHighScore = false;
	private bool finishedEntering = false;
	private string inputName = "";
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		buttons.continueButton.onClick.AddListener(() => GUIManager.Instance.MenuFadeTransition("MainMenu"));
	}

	void Start(){
		highScores[0] = GameObject.Find("HighScore1").GetComponent<Text>();
		highScores[0].text = PrefsManager.GetHighScoreName(1).ToUpper()+": "+PrefsManager.GetHighScore(1);
		highScores[1] = GameObject.Find("HighScore2").GetComponent<Text>();
		highScores[1].text = PrefsManager.GetHighScoreName(2).ToUpper()+": "+PrefsManager.GetHighScore(2);
		highScores[2] = GameObject.Find("HighScore3").GetComponent<Text>();
		highScores[2].text = PrefsManager.GetHighScoreName(3).ToUpper()+": "+PrefsManager.GetHighScore(3);
		highScores[3] = GameObject.Find("HighScore4").GetComponent<Text>();
		highScores[3].text = PrefsManager.GetHighScoreName(4).ToUpper()+": "+PrefsManager.GetHighScore(4);
		highScores[4] = GameObject.Find("HighScore5").GetComponent<Text>();
		highScores[4].text = PrefsManager.GetHighScoreName(5).ToUpper()+": "+PrefsManager.GetHighScore(5);
	}
	
	void Update(){
		for(int i=0;i<5;i++){
			if(highScores[i].text != PrefsManager.GetHighScoreName(i+1).ToUpper()+": "+PrefsManager.GetHighScore(i+1))
				highScores[i].text = PrefsManager.GetHighScoreName(i+1).ToUpper()+": "+PrefsManager.GetHighScore(i+1);
		}
		if(gameObject.activeSelf && !finishedEntering){
			if(!enterHighScore)
				HighScoreCheck();
			else{
				highScores[replaceScore].text = inputName.ToUpper()+": "+GameMaster.Instance.PlayerManager.GetPlayerScore().ToString();
				foreach(char c in Input.inputString){
					if(c >= 'a' && c <= 'z' && inputName.Length < 3){
						inputName += c;
						highScores[replaceScore].text = inputName.ToUpper()+": "+GameMaster.Instance.PlayerManager.GetPlayerScore().ToString();
					} else if(c == '\b' && inputName.Length > 0){
						inputName = inputName.Substring(0,inputName.Length-1);
						highScores[replaceScore].text = inputName.ToUpper()+": "+GameMaster.Instance.PlayerManager.GetPlayerScore().ToString();
					}
					if(c == '\r'){
						PrefsManager.SetHighScoreName(replaceScore+1,inputName);
						PrefsManager.SetHighScore(replaceScore+1,GameMaster.Instance.PlayerManager.GetPlayerScore());
						enterHighScore = false;
						finishedEntering = true;
						GameMaster.Instance.PlayerManager.SetPlayerScore(0);
						inputName = "";
					}
				}
			}
		}
	}
	
	#endregion
	
	void HighScoreCheck(){
		for(int i=1;i<6;i++){
			if(GameMaster.Instance.PlayerManager.GetPlayerScore() > PrefsManager.GetHighScore(i)){
				replaceScore = i-1;
				enterHighScore = true;
				break;
			}
		}
	}
}