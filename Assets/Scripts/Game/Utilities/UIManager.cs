using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	private InGameUI inGameUI;
	private OptionsController optionsController;
	private GameObject mainMenu;
	private GameMaster gameMaster;
	
	void Start(){
		gameMaster = FindObjectOfType<GameMaster>();
		inGameUI = GetComponentInChildren<InGameUI>();
		inGameUI.gameObject.SetActive(false);
		optionsController = GetComponentInChildren<OptionsController>();
		optionsController.gameObject.SetActive(false);
		mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
		mainMenu.SetActive(false);
	}
	
	public void OpenMainMenu(){
		mainMenu.SetActive(true);
		optionsController.gameObject.SetActive(false);
		inGameUI.gameObject.SetActive(false);
	}
	
	public void OpenOptionsMenu(){
		optionsController.gameObject.SetActive(true);
		mainMenu.SetActive(false);
		inGameUI.gameObject.SetActive(false);
		optionsController.SetSliders();
	}
	
	public void OpenInGameUI(){
		inGameUI.gameObject.SetActive(true);
		mainMenu.SetActive(false);
		optionsController.gameObject.SetActive(false);
	}
	
	public void ToggleInGameMenu(){
		inGameUI.ToggleMenu();
	}
	
	public void CloseAll(){
		mainMenu.SetActive(false);
		optionsController.gameObject.SetActive(false);
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