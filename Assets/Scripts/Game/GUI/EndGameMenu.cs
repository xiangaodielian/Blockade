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
		buttons.continueButton.onClick.AddListener(() => UIManager.instance.ProceedToLevel("MainMenu", false));

		if(buttons.tryAgainButton)
			buttons.tryAgainButton.onClick.AddListener(() => UIManager.instance.ReloadPreviousLevel());
	}
}
