/*--------------------------------------/
  InGameMainMenu Class - Blockade
  Controls GUI for the In Game Main Menu
  Writen by Joe Arthur
  Latest Revision - 3 Apr, 2016
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
														   GameMaster.instance.ClearGameObjects();
														   UIManager.instance.ProceedToLevel("MainMenu", false);
														   UIManager.instance.OpenMainMenu(); });
		}
	}
}
