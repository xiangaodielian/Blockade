/*------------------------------------/
  MainMenu Class - Blockade
  Controls GUI for Confirmation Menus
  Writen by Joe Arthur
  Latest Revision - 9 Apr, 2016
/------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmationMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button cancelButton = null;
		public Button quitButton = null;
		public Button confirmButton = null;
		public Button noButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	void Awake(){
		SetOnClick();
	}

	void SetOnClick(){
		if(buttons.quitButton)
			buttons.cancelButton.onClick.AddListener(() => UIManager.instance.ToggleQuitConfirm(false));

		if(buttons.cancelButton)
			buttons.quitButton.onClick.AddListener(() => LevelManager.QuitApplication());

		if(buttons.confirmButton)
			buttons.confirmButton.onClick.AddListener(() => UIManager.instance.OpenInterviewerTutorial());

		if(buttons.noButton)
			buttons.noButton.onClick.AddListener(() => { UIManager.instance.ToggleInterviewConfirm(false);
														 UIManager.instance.MenuFadeTransition("LevelSelectMenu");});
	}
}
