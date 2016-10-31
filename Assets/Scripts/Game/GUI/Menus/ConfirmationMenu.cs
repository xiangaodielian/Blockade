using UnityEngine;
using UnityEngine.UI;
using ApplicationManagement;

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
			buttons.cancelButton.onClick.AddListener(() => GUIManager.Instance.ToggleQuitConfirm(false));

		if(buttons.cancelButton)
			buttons.quitButton.onClick.AddListener(() => LevelManager.QuitApplication());

		if(buttons.confirmButton)
			buttons.confirmButton.onClick.AddListener(() => {
				GUIManager.Instance.InstantiateInterviewerTutorial();
				MusicPlayer.instance.NextTrack();
			});

		if(buttons.noButton)
			buttons.noButton.onClick.AddListener(() => {
				GUIManager.Instance.ToggleInterviewConfirm(false);
				GUIManager.Instance.MenuFadeTransition("LevelSelectMenu");
			});
	}
}
