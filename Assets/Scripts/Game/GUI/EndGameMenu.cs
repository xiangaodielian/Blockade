/*------------------------------/
  EndGameMenu Class - Blockade
  Controlling class for EndGame
  GUIs
  Writen by Joe Arthur
  Latest Revision - 3 Apr, 2016
/------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button tryAgainButton = null;
		public Button continueButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	void Awake(){
		buttons.continueButton.onClick.AddListener(() => { UIManager.instance.ProceedToLevel("MainMenu", false);
														   UIManager.instance.OpenHighScoreMenu();});

		if(buttons.tryAgainButton)
			buttons.tryAgainButton.onClick.AddListener(() => UIManager.instance.ReloadPreviousLevel());
	}
}
