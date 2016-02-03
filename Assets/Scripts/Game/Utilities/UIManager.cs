/*----------------------------/
  UIManager Class - Blockade
  Manages all GUI elements and
  their functions
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	//Singleton instance for UIManager
	public static UIManager instance {get; private set;}
	
	private GameObject mainMenu;
	private GameObject levelSelectMenu;
	private GameObject optionsMenu;
	private GameObject highScoresMenu;
	private GameObject winMenu;
	private GameObject loseMenu;
	private InGameUI inGameUI;
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	void Start(){
		inGameUI = GetComponentInChildren<InGameUI>();
		mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
		levelSelectMenu = GameObject.FindGameObjectWithTag("LevelSelectMenu");
		optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
		highScoresMenu = GameObject.FindGameObjectWithTag("HighScoresMenu");
		winMenu = GameObject.FindGameObjectWithTag("WinMenu");
		loseMenu = GameObject.FindGameObjectWithTag("LoseMenu");
		CloseAll();
	}
	
	public void OpenMainMenu(){
		CloseAll();
		mainMenu.SetActive(true);
	}
	
	public void OpenLevelSelectMenu(){
		CloseAll();
		levelSelectMenu.SetActive(true);
	}
	
	public void OpenOptionsMenu(){
		CloseAll();
		optionsMenu.SetActive(true);
		optionsMenu.GetComponent<OptionsController>().SetSliders();
	}
	
	public void OpenHighScoreMenu(){
		CloseAll();
		highScoresMenu.SetActive(true);
	}
	
	public void OpenInGameUI(){
		CloseAll();
		inGameUI.gameObject.SetActive(true);
	}
	
	public void OpenEndGameMenu(string level){
		CloseAll();
		if(level == "Win")
			winMenu.SetActive(true);
		else
			loseMenu.SetActive(true);
	}
	
	public void ToggleInGameMenu(){
		inGameUI.ToggleMenu();
	}
	
	public void CloseAll(){
		mainMenu.SetActive(false);
		levelSelectMenu.SetActive(false);
		optionsMenu.SetActive(false);
		highScoresMenu.SetActive(false);
		inGameUI.gameObject.SetActive(false);
		winMenu.SetActive(false);
		loseMenu.SetActive(false);
	}
	
	public void ToggleLaunchPrompt(bool visible){
		inGameUI.TogglePrompt(visible);
	}
	
	public void EndLevelMenu(){
		inGameUI.ToggleEndLevelPanel(true);
		inGameUI.CalculateTimeBonus();
	}
	
	public void ProceedToNextLevel(){
		inGameUI.ToggleEndLevelPanel(false);
		GameMaster.instance.GamePause();
		GameMaster.instance.ChangeToLevel("Next");
	}
}