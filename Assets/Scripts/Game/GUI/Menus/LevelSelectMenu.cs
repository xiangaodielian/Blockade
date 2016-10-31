using UnityEngine;
using UnityEngine.UI;
using ApplicationManagement;
using ApplicationManagement.ResourceControl;

public class LevelSelectMenu : MonoBehaviour {

	#region Variables

	[System.Serializable] private class Buttons{
		public Button nextButton = null;
		public Button previousButton = null;
		public Button resumeButton = null;
		public Button backButton = null;
		public Button[] levelButtons = new Button[20];
	}

	[SerializeField] private GameObject levelLoadScreen = null;
	[SerializeField] private GameObject levels01To10 = null;
	[SerializeField] private GameObject levels11To20 = null;
	[SerializeField] private Buttons buttons = null;

	private int highestUnlocked = 0;
	private Vector3 screenOneNewPos;
	private Vector3 screenTwoNewPos;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		levelLoadScreen = Instantiate(levelLoadScreen);
		levelLoadScreen.transform.SetParent(this.transform);
		levelLoadScreen.transform.localPosition = Vector3.zero;
		levelLoadScreen.transform.localScale = Vector3.one;
		levelLoadScreen.SetActive(false);

		highestUnlocked = PrefsManager.GetLevelUnlocked();

		buttons.nextButton.GetComponentInChildren<Text>().text = "11-20";
		buttons.previousButton.gameObject.SetActive(false);

		SetOnClick();
	}

	void Start(){
		SetLevelImages();
		screenOneNewPos = levels01To10.transform.position;
		screenTwoNewPos = levels11To20.transform.position;
	}
	
	void OnGUI(){
		if(highestUnlocked != PrefsManager.GetLevelUnlocked()){
			highestUnlocked = PrefsManager.GetLevelUnlocked();
			SetLevelImages();
		}

		if(levels01To10.transform.position != screenOneNewPos){
			float screenOneX = Mathf.Lerp(levels01To10.transform.position.x, screenOneNewPos.x, Time.deltaTime * 3f);
			levels01To10.transform.position = new Vector3(screenOneX, levels01To10.transform.position.y, levels01To10.transform.position.z);

			float screenTwoX = Mathf.Lerp(levels11To20.transform.position.x, screenTwoNewPos.x, Time.deltaTime * 3f);
			levels11To20.transform.position = new Vector3(screenTwoX, levels11To20.transform.position.y, levels11To20.transform.position.z);
		}
	}
	
	#endregion
	#region Level Select Functions
	
	//Set Level Images to LevelImage if unlocked or ? if locked
	private void SetLevelImages(){
		for(int i=0;i<buttons.levelButtons.Length;i++){
			if(i<highestUnlocked){
				buttons.levelButtons[i].GetComponent<Image>().sprite = ResourceManager.SetGuiTexture(buttons.levelButtons[i].name);
				buttons.levelButtons[i].enabled = true;
			} else{
				buttons.levelButtons[i].GetComponent<Image>().sprite = ResourceManager.SetGuiTexture("LevelLocked");
				buttons.levelButtons[i].enabled = false;
			}
		}
	}

	private void SetOnClick(){
		int index = 1;
		string levelNameBase = "Level_";

		foreach(Button button in buttons.levelButtons){
			string levelToLoad = levelNameBase;
			if(index < 10)
				levelToLoad += "0" + index.ToString();
			else
				levelToLoad += index.ToString();

			button.onClick.AddListener(() => { 
				StartCoroutine(LevelManager.Instance.ChangeToLevelAsync(levelToLoad));
				MusicPlayer.instance.NextTrack();
				ShowLoadScreen(); 
			});

			index++;
		}

		buttons.resumeButton.onClick.AddListener(() => {
			LevelManager.Instance.ChangeToLevelAsync("LatestCheckpoint");
			MusicPlayer.instance.NextTrack(); 
		});
		buttons.backButton.onClick.AddListener(() => GUIManager.Instance.MenuFadeTransition("MainMenu"));
	}
	
	public void NextLevelScreen(){
		Vector3 posOffset = new Vector3();
		posOffset.x = levels11To20.transform.position.x - levels01To10.transform.position.x;

		if(levels01To10.activeSelf){
			screenOneNewPos -= posOffset;
			screenTwoNewPos -= posOffset;
			buttons.nextButton.gameObject.SetActive(false);
			buttons.previousButton.gameObject.SetActive(true);
			buttons.previousButton.GetComponentInChildren<Text>().text = "01-10";
		}
	}
	
	public void PreviousLevelScreen(){
		Vector3 posOffset = new Vector3();
		posOffset.x = levels11To20.transform.position.x - levels01To10.transform.position.x;

		if(levels11To20.activeSelf){
			screenOneNewPos += posOffset;
			screenTwoNewPos += posOffset;
			buttons.previousButton.gameObject.SetActive(false);
			buttons.nextButton.gameObject.SetActive(true);
			buttons.nextButton.GetComponentInChildren<Text>().text = "11-20";
		}
	}

	void ShowLoadScreen(){
		levelLoadScreen.SetActive(true);
	}
	
	#endregion
}
