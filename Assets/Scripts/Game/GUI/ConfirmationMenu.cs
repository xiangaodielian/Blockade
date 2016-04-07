/*------------------------------------/
  MainMenu Class - Blockade
  Controls GUI for Confirmation Menus
  Writen by Joe Arthur
  Latest Revision - 7 Apr, 2016
/------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmationMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button cancelButton = null;
		public Button quitButton = null;
		//public Button confirmButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	void Awake(){
		buttons.cancelButton.onClick.AddListener(() => UIManager.instance.ToggleQuitConfirm(false));
		buttons.quitButton.onClick.AddListener(() => LevelManager.QuitApplication());
	}
}
