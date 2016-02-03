/*---------------------------------/
  LevelSelectMenu Class - Blockade
  Manages Level Selection Menu
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/--------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour {

	[SerializeField] private Button[] levelButtons = new Button[20];
	[SerializeField] private Sprite[] levelImages = new Sprite[21];
	[SerializeField] private Button nextButton = null;
	[SerializeField] private Button previousButton = null;
	
	private GameObject levels01To10;
	private GameObject levels11To20;
	private int highestUnlocked = 0;
	
	void Awake(){
		levels01To10 = GameObject.Find("Levels_01-10");
		levels01To10.SetActive(true);
		levels11To20 = GameObject.Find("Levels_11-20");
		levels11To20.SetActive(false);
		highestUnlocked = PrefsManager.GetLevelUnlocked();
		nextButton.GetComponentInChildren<Text>().text = "11-20";
		previousButton.gameObject.SetActive(false);
		
		SetLevelImages();
	}
	
	void Update(){
		if(highestUnlocked != PrefsManager.GetLevelUnlocked()){
			highestUnlocked = PrefsManager.GetLevelUnlocked();
			SetLevelImages();
		}
	}
	
	void SetLevelImages(){
		for(int i=0;i<levelButtons.Length;i++){
			if(i<highestUnlocked){
				levelButtons[i].GetComponent<Image>().sprite = levelImages[i];
				levelButtons[i].enabled = true;
			} else{
				levelButtons[i].GetComponent<Image>().sprite = levelImages[20];
				levelButtons[i].enabled = false;
			}
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
}
