using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	private GameMaster gameMaster;
	private GameObject mainMenu;
	private GameObject levelSelectMenu;
	private GameObject optionsMenu;
	private InGameUI inGameUI;
	
	void Start(){
		gameMaster = FindObjectOfType<GameMaster>();
		inGameUI = GetComponentInChildren<InGameUI>();
		inGameUI.gameObject.SetActive(false);
		mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
		mainMenu.SetActive(false);
		levelSelectMenu = GameObject.FindGameObjectWithTag("LevelSelectMenu");
		levelSelectMenu.SetActive(false);
		optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
		optionsMenu.SetActive(false);
	}
	
	public void OpenMainMenu(){
		mainMenu.SetActive(true);
		levelSelectMenu.SetActive(false);
		optionsMenu.SetActive(false);
		inGameUI.gameObject.SetActive(false);
	}
	
	public void OpenLevelSelectMenu(){
		mainMenu.SetActive(false);
		levelSelectMenu.SetActive(true);
		optionsMenu.SetActive(false);
		inGameUI.gameObject.SetActive(false);
	}
	
	public void OpenOptionsMenu(){
		optionsMenu.SetActive(true);
		mainMenu.SetActive(false);
		levelSelectMenu.SetActive(false);
		inGameUI.gameObject.SetActive(false);
		optionsMenu.GetComponent<OptionsController>().SetSliders();
	}
	
	public void OpenInGameUI(){
		inGameUI.gameObject.SetActive(true);
		mainMenu.SetActive(false);
		levelSelectMenu.SetActive(false);
		optionsMenu.SetActive(false);
	}
	
	public void ToggleInGameMenu(){
		inGameUI.ToggleMenu();
	}
	
	public void CloseAll(){
		mainMenu.SetActive(false);
		levelSelectMenu.SetActive(false);
		optionsMenu.SetActive(false);
		inGameUI.gameObject.SetActive(false);
	}
	
	public void LaunchPromptOn(){
		inGameUI.TogglePrompt(true);
	}
	
	public void LaunchPromptOff(){
		inGameUI.TogglePrompt(false);
	}
	
	public void EndLevelMenu(){
		inGameUI.ToggleEndLevelPanel(true);
		inGameUI.CalculateTimeBonus();
	}
	
	public void ProceedToNextLevel(){
		inGameUI.ToggleEndLevelPanel(false);
		gameMaster.GamePause();
		gameMaster.ChangeToLevel("Next");
	}
}