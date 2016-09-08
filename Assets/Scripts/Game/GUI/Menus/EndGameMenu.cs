using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour {

	#region Variables

	[System.Serializable] private class Buttons{
		public Button tryAgainButton = null;
		public Button continueButton = null;
	}

	[SerializeField] private Buttons buttons = null;
	[SerializeField] private Text scoreText = null;

	#endregion
	#region Mono Functions

	void Awake(){
		scoreText.text = "YOUR SCORE: " + GameMaster.GMInstance.playerManager.GetPlayerScore().ToString();

		SetOnClick();
	}

	#endregion
	#region Utility

	void SetOnClick(){
		buttons.continueButton.onClick.AddListener(() => {
			LevelManager.instance.ChangeToLevel("MainMenu");
			GUIManager.instance.DestroyEndGameMenus();
		});

		if(buttons.tryAgainButton)
			buttons.tryAgainButton.onClick.AddListener(() => LevelManager.instance.RestartLevel());
	}

	#endregion
}
