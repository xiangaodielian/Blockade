/*--------------------------------------/
  InGameMainMenu Class - Blockade
  Controls GUI for the In Game Main Menu
  Writen by Joe Arthur
  Latest Revision - 20 Mar, 2016
/--------------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameMainMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button resumeButton = null;
		public Button optionsButton = null;
		public Button exitButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	void Awake(){
		if(InGameUI.instance){
			buttons.resumeButton.onClick.AddListener(() => InGameUI.instance.ToggleMenu());
			buttons.optionsButton.onClick.AddListener(() => InGameUI.instance.InGameOptions());
			buttons.exitButton.onClick.AddListener(() => { InGameUI.instance.ToggleMenu(); 
														   UIManager.instance.ProceedToLevel("MainMenu", false); });
		}
	}
}
