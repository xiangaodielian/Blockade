/*--------------------------------/
  MainMenu Class - Blockade
  Controls GUI for Main Menu
  Writen by Joe Arthur
  Latest Revision - 27 Mar, 2016
/--------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button startButton = null;
		public Button highScoresButton = null;
		public Button optionsButton = null;
		public Button quitButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	void Start(){
		if(UIManager.instance){
			buttons.startButton.onClick.AddListener(() => UIManager.instance.ToggleInterviewConfirm(true));
			buttons.highScoresButton.onClick.AddListener(() => UIManager.instance.MenuFadeTransition("HighScoresMenu"));
			buttons.optionsButton.onClick.AddListener(() => UIManager.instance.MenuFadeTransition("OptionsMenu"));
			buttons.quitButton.onClick.AddListener(() => UIManager.instance.QuitRequest());
		}
	}
}
