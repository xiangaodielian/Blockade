/*---------------------------------/
  LevelSelectMenu Class - Blockade
  Manages Level Selection Menu
  Writen by Joe Arthur
  Latest Revision - 27 Mar, 2016
/--------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour {

	#region Variables

	[SerializeField] private GameObject levelLoadScreen = null;
	[SerializeField] private GameObject levels01To10 = null;
	[SerializeField] private GameObject levels11To20 = null;
	[SerializeField] private Button[] levelButtons = new Button[20];
	[SerializeField] private Sprite[] levelImages = new Sprite[21];
	[SerializeField] private Button nextButton = null;
	[SerializeField] private Button previousButton = null;
	[SerializeField] private Button resumeButton = null;
	[SerializeField] private Button backButton = null;

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
		nextButton.GetComponentInChildren<Text>().text = "11-20";
		previousButton.gameObject.SetActive(false);

		SetLevelImages();
		SetOnClick();
	}

	void Start(){
		screenOneNewPos = levels01To10.transform.position;
		screenTwoNewPos = levels11To20.transform.position;
	}
	
	void Update(){
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
		for(int i=0;i<levelButtons.Length;i++){
			if(i<highestUnlocked){
				levelButtons[i].GetComponent<Image>().sprite = levelImages[i];
				levelButtons[i].enabled = true;
			} else{
				levelButtons[i].GetComponent<Image>().sprite = levelImages[levelImages.Length-1];
				levelButtons[i].enabled = false;
			}
		}
	}

	private void SetOnClick(){
		int index = 1;
		string levelNameBase = "Level_";

		foreach(Button button in levelButtons){
			string levelToLoad = levelNameBase;
			if(index < 10)
				levelToLoad += "0" + index.ToString();
			else
				levelToLoad += index.ToString();

			button.onClick.AddListener(() => { UIManager.instance.ProceedToLevel(levelToLoad, true);
											   ShowLoadScreen(); });
			index++;
		}

		resumeButton.onClick.AddListener(() => UIManager.instance.ProceedToLevel("LatestCheckpoint", true));
		backButton.onClick.AddListener(() => UIManager.instance.MenuFadeTransition("MainMenu"));
	}
	
	public void NextLevelScreen(){
		Vector3 posOffset = new Vector3();
		posOffset.x = levels11To20.transform.position.x - levels01To10.transform.position.x;

		if(levels01To10.activeSelf){
			screenOneNewPos -= posOffset;
			screenTwoNewPos -= posOffset;
			nextButton.gameObject.SetActive(false);
			previousButton.gameObject.SetActive(true);
			previousButton.GetComponentInChildren<Text>().text = "01-10";
		}
	}
	
	public void PreviousLevelScreen(){
		Vector3 posOffset = new Vector3();
		posOffset.x = levels11To20.transform.position.x - levels01To10.transform.position.x;

		if(levels11To20.activeSelf){
			screenOneNewPos += posOffset;
			screenTwoNewPos += posOffset;
			previousButton.gameObject.SetActive(false);
			nextButton.gameObject.SetActive(true);
			nextButton.GetComponentInChildren<Text>().text = "11-20";
		}
	}

	void ShowLoadScreen(){
		levelLoadScreen.SetActive(true);
	}
	
	#endregion
}
