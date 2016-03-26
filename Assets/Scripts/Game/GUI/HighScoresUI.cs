/*------------------------------/
  HighScoresUI Class - Blockade
  Manages High Score page and
  updates High Score data
  Writen by Joe Arthur
  Latest Revision - 22 Mar, 2016
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
	#if UNITY_IOS || UNITY_ANDROID || UNITY_WSA
	private TouchScreenKeyboard keyboard = null;
	private bool keyboardOpen = false;
	#endif
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		buttons.continueButton.onClick.AddListener(() => UIManager.instance.OpenMainMenu());
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
				highScores[replaceScore].text = inputName.ToUpper()+": "+GameMaster.instance.gameValues.totalScore.ToString();
				#if UNITY_STANDALONE || UNITY_WEBGL
				foreach(char c in Input.inputString){
					if(c >= 'a' && c <= 'z' && inputName.Length < 3){
						inputName += c;
						highScores[replaceScore].text = inputName.ToUpper()+": "+GameMaster.instance.gameValues.totalScore.ToString();
					} else if(c == '\b' && inputName.Length > 0){
						inputName = inputName.Substring(0,inputName.Length-1);
						highScores[replaceScore].text = inputName.ToUpper()+": "+GameMaster.instance.gameValues.totalScore.ToString();
					}
					if(c == '\r'){
						PrefsManager.SetHighScoreName(replaceScore+1,inputName);
						PrefsManager.SetHighScore(replaceScore+1,GameMaster.instance.gameValues.totalScore);
						enterHighScore = false;
						finishedEntering = true;
						inputName = "";
					}
				}
				#elif UNITY_IOS || UNITY_ANDROID || UNITY_WSA
				if(keyboard == null && !keyboardOpen){
					keyboard = TouchScreenKeyboard.Open(inputName,TouchScreenKeyboardType.Default,false,false,false,false,"");
					keyboardOpen = true;
				}
				if(keyboard.active){
					inputName = keyboard.text;
					if(inputName.Length > 3)
						inputName = inputName.Substring(0,3);
					if(keyboard.done){
						inputName = keyboard.text;
						keyboard.active = false;
					}
				}
				if(!keyboard.active && keyboardOpen){
					keyboard = null;
					PrefsManager.SetHighScoreName(replaceScore+1,inputName);
					PrefsManager.SetHighScore(replaceScore+1,gameMaster.totalScore);
					keyboardOpen = false;
					enterHighScore = false;
					finishedEntering = true;
					inputName = "";
				}
				#endif
			}
		}
	}
	
	#endregion
	
	void HighScoreCheck(){
		for(int i=1;i<6;i++){
			if(GameMaster.instance.gameValues.totalScore > PrefsManager.GetHighScore(i)){
				replaceScore = i-1;
				enterHighScore = true;
				break;
			}
		}
	}
}