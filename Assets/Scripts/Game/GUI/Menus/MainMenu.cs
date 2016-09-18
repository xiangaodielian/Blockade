using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	#region Variables

	[System.Serializable] private class Buttons{
		public Button startButton = null;
		public Button highScoresButton = null;
		public Button optionsButton = null;
		public Button quitButton = null;
	}

	[SerializeField] private Buttons buttons = null;
	[SerializeField] private string updatePanelPrefab = null;

	private string runningVersion = "";
	private GameObject updatePanel;

	#endregion
	#region Mono Functions

	void Start(){
		buttons.startButton.onClick.AddListener(() => GUIManager.Instance.ToggleInterviewConfirm(true));
		buttons.highScoresButton.onClick.AddListener(() => GUIManager.Instance.MenuFadeTransition("HighScoresMenu"));
		buttons.optionsButton.onClick.AddListener(() => GUIManager.Instance.MenuFadeTransition("OptionsMenu"));
		buttons.quitButton.onClick.AddListener(() => GUIManager.Instance.ToggleQuitConfirm(true));
	}

	#endregion
	#region Utility

	public void UpdateCheck(){
		runningVersion = GameObject.Find("VersionText").GetComponent<Text>().text;
		runningVersion = runningVersion.Replace("VERSION ", "");

		updatePanel = ResourceManager.LoadPrefab("gui", updatePanelPrefab);
		updatePanel = Instantiate(updatePanel);
		updatePanel.transform.SetParent(this.transform);
		updatePanel.transform.localPosition = Vector3.zero;

		if(runningVersion != updatePanel.GetComponent<UpdatePanel>().curVersion)
			updatePanel.GetComponent<UpdatePanel>().SetUpdateText(runningVersion);
		else
			Destroy(updatePanel);
	}

	public void SetButtonInteraction(bool isInteractable){
		buttons.startButton.interactable = isInteractable;
		buttons.highScoresButton.interactable = isInteractable;
		buttons.optionsButton.interactable = isInteractable;
		buttons.quitButton.interactable = isInteractable;
	}

	#endregion
}
