using UnityEngine;
using UnityEngine.UI;
using ApplicationManagement;

public class EndLevelMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button continueButton = null;
	}

	[SerializeField] private Buttons buttons = null;

	void Awake(){
		buttons.continueButton.onClick.AddListener(() => {
			GUIManager.Instance.InGameGui.StopTimer();
			GUIManager.Instance.InGameGui.ToggleEndLevelPanel(false);
		});
	}
}
