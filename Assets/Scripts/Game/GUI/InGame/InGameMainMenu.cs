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
		buttons.resumeButton.onClick.AddListener(() => GUIManager.instance.inGameGUI.ToggleMenu());
		buttons.optionsButton.onClick.AddListener(() => GUIManager.instance.inGameGUI.InGameOptions());
		buttons.exitButton.onClick.AddListener(() => {
			GUIManager.instance.inGameGUI.ToggleMenu();
			GameObjectManager.ClearGameObjects();
			GUIManager.instance.DestroyInGameGUI(GUIManager.TargetMenuOptions.mainMenu);
			LevelManager.instance.ChangeToLevel("MainMenu");
		});
	}
}
