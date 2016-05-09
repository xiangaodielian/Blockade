/*-------------------------------------/
  UpdatePanel Class - Blockade
  Controls Notification of Application
  Update for User
  Writen by Joe Arthur
  Latest Revision - 8 May, 2016
/-------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

public class UpdatePanel : MonoBehaviour {

	[System.Serializable] private class Buttons{
		public Button continueButton = null;
		public Button downloadButton = null;
	}

	public string curVersion = "2.0.0";

	[SerializeField] private Text updateText = null;
	[SerializeField] private Buttons buttons = null;

	void Start(){
		buttons.continueButton.onClick.AddListener(() => Destroy(this.gameObject));
		buttons.downloadButton.onClick.AddListener(() => Application.OpenURL("http://josephearthur.com/games/blockade.html"));
	}

	public void SetUpdateText(string runningVersion){
		updateText.text = "NEW VERSION AVAILABLE! THE LATEST VERSION IS " + curVersion + " AND YOU ARE RUNNING " + runningVersion + ". DOWNLOAD AT JOSEPHEARTHUR.COM/GAMES/BLOCKADE.";
	}
}
