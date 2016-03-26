/*---------------------------------/
  LevelSelectMenu Class - Blockade
  Manages Level Selection Menu
  Writen by Joe Arthur
  Latest Revision - 25 Mar, 2016
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

	private int highestUnlocked = 0;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		levelLoadScreen = Instantiate(levelLoadScreen);
		levelLoadScreen.transform.SetParent(this.transform);
		levelLoadScreen.transform.localPosition = Vector3.zero;
		levelLoadScreen.transform.localScale = Vector3.one;
		levelLoadScreen.SetActive(false);
		levels01To10.SetActive(true);
		levels11To20.SetActive(false);
		highestUnlocked = PrefsManager.GetLevelUnlocked();
		nextButton.GetComponentInChildren<Text>().text = "11-20";
		previousButton.gameObject.SetActive(false);
		
		SetLevelImages();
		SetOnClick();
	}
	
	void Update(){
		if(highestUnlocked != PrefsManager.GetLevelUnlocked()){
			highestUnlocked = PrefsManager.GetLevelUnlocked();
			SetLevelImages();
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
	}
	
	public void NextLevelScreen(){
		if(levels01To10.activeSelf){
			levels01To10.SetActive(false);
			levels11To20.SetActive(true);
			nextButton.gameObject.SetActive(false);
			previousButton.gameObject.SetActive(true);
			previousButton.GetComponentInChildren<Text>().text = "01-10";
		}
	}
	
	public void PreviousLevelScreen(){
		if(levels11To20.activeSelf){
			levels11To20.SetActive(false);
			levels01To10.SetActive(true);
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
