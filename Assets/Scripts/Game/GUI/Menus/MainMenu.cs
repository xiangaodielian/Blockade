/*--------------------------------/
  MainMenu Class - Blockade
  Controls GUI for Main Menu
  Writen by Joe Arthur
  Latest Revision - 8 May, 2016
/--------------------------------*/

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

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

	void Start(){
		buttons.startButton.onClick.AddListener(() => UIManager.instance.ToggleInterviewConfirm(true));
		buttons.highScoresButton.onClick.AddListener(() => UIManager.instance.MenuFadeTransition("HighScoresMenu"));
		buttons.optionsButton.onClick.AddListener(() => UIManager.instance.MenuFadeTransition("OptionsMenu"));
		buttons.quitButton.onClick.AddListener(() => UIManager.instance.QuitRequest());

		runningVersion = GameObject.Find("VersionText").GetComponent<Text>().text;
		runningVersion = runningVersion.Replace("VERSION ", "");
	}

	public void UpdateCheck(){
		updatePanel = ResourceManager.LoadPrefab("gui", updatePanelPrefab);
		updatePanel = Instantiate(updatePanel);
		updatePanel.transform.SetParent(this.transform);
		updatePanel.transform.localPosition = Vector3.zero;

		if(runningVersion != updatePanel.GetComponent<UpdatePanel>().curVersion)
			updatePanel.GetComponent<UpdatePanel>().SetUpdateText(runningVersion);
		else
			Destroy(updatePanel);
	}
}
