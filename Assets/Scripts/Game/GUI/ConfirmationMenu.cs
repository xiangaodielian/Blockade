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
	[SerializeField] private GameObject levelLoadScreen = null;

	void Awake(){
		if(levelLoadScreen){
			levelLoadScreen = Instantiate(levelLoadScreen);
			levelLoadScreen.transform.SetParent(this.transform);
			levelLoadScreen.transform.localPosition = new Vector3(0f, 45f, 0f);
			levelLoadScreen.transform.localScale = Vector3.one;
			levelLoadScreen.SetActive(false);
		}

		SetOnClick();
	}

	void SetOnClick(){
		if(buttons.quitButton)
			buttons.cancelButton.onClick.AddListener(() => UIManager.instance.ToggleQuitConfirm(false));

		if(buttons.cancelButton)
			buttons.quitButton.onClick.AddListener(() => LevelManager.QuitApplication());

		if(buttons.confirmButton)
			buttons.confirmButton.onClick.AddListener(() => { ShowLoadScreen();
															  UIManager.instance.ProceedToLevel("Level_15", true);
															  GameMaster.instance.gameValues.playerLives = 99;});

		if(buttons.noButton)
			buttons.noButton.onClick.AddListener(() => { UIManager.instance.ToggleInterviewConfirm(false);
														 UIManager.instance.MenuFadeTransition("LevelSelectMenu");});
	}

	void ShowLoadScreen(){
		levelLoadScreen.SetActive(true);
	}
}
