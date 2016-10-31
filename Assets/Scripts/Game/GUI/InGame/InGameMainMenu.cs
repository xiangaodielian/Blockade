using UnityEngine;
using UnityEngine.UI;
using ApplicationManagement;

public class InGameMainMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button resumeButton = null;
		public Button optionsButton = null;
		public Button exitButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	void Awake(){
		buttons.resumeButton.onClick.AddListener(() => GUIManager.Instance.InGameGui.ToggleMenu());
		buttons.optionsButton.onClick.AddListener(() => GUIManager.Instance.InGameGui.InGameOptions());
		buttons.exitButton.onClick.AddListener(() => {
			GUIManager.Instance.InGameGui.ToggleMenu();
			GameObjectManager.ClearGameObjects();
			GUIManager.Instance.DestroyInGameGUI(GUIManager.TargetMenuOptions.MainMenu);
			LevelManager.Instance.ChangeToLevel("MainMenu");
		});
	}
}
