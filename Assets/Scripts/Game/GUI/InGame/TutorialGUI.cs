using UnityEngine;
using UnityEngine.UI;

public class TutorialGUI : MonoBehaviour {

	[System.Serializable] private class Images{
		public string interviewerTutorialImage = "";
		public string inGameBrickTutorial = "";
		public string inGamePowerupTutorial = "";
		public string inGameUnbreakableTutorial = "";
	}

	[System.Serializable] private class Positions{
		public Vector3 inGameBrickTutorialPos = Vector3.zero;
		public Vector3 inGamePowerupTutorialPos1 = Vector3.zero;
		public Vector3 inGamePowerupTutorialPos2 = Vector3.zero;
		public Vector3 inGameUnbreakableTutorialPos = Vector3.zero;
	}

	[SerializeField] private Images imageNames = null;
	[SerializeField] private Positions tutorialPositions = null;
	[SerializeField] private Button continueButton = null;
	[SerializeField] private bool inGame = false;
	[SerializeField] private GameObject levelLoadScreen = null;
	[SerializeField] private Image tutorialImage = null;
	[SerializeField] private Text movementText = null;

	void Start(){
		if(levelLoadScreen){
			levelLoadScreen = Instantiate(levelLoadScreen);
			levelLoadScreen.transform.SetParent(this.transform);
			levelLoadScreen.transform.localPosition = Vector3.zero;
			levelLoadScreen.transform.localScale = Vector3.one;
			levelLoadScreen.SetActive(false);
		}

		if(!inGame){
			tutorialImage.sprite = ResourceManager.SetGUITexture(imageNames.interviewerTutorialImage);
			continueButton.onClick.AddListener(() => {
				ShowLoadScreen();
				StartCoroutine(LevelManager.instance.ChangeToLevelAsync("Level_15"));
				GameMaster.GMInstance.playerManager.SetPlayerLives(99);
			});
		} else
			continueButton.onClick.AddListener(() => GUIManager.instance.inGameGUI.ToggleInGameTutorials(false, 0));
	}

	void ShowLoadScreen(){
		levelLoadScreen.SetActive(true);
	}

	public void SetTutorial(int tutNum){
		switch(tutNum){
			case 0:
				tutorialImage.sprite = ResourceManager.SetGUITexture(imageNames.inGameBrickTutorial);
				tutorialImage.transform.localPosition = tutorialPositions.inGameBrickTutorialPos;
				movementText.enabled = true;
				if(PrefsManager.GetMouseControl())
					movementText.text = "USE THE MOUSE CURSOR TO MOVE THE PADDLE. CLICK TO LAUNCH THE BALL. THIS CAN BE CHANGED TO KEYBOARD MOVEMENT IN THE OPTIONS MENU.";
				else
					movementText.text = "USE THE ARROW OR 'A' AND 'D' KEYS TO MOVE THE PADDLE. HOLD SHIFT FOR EXTRA SPEED. PRESS SPACE TO LAUNCH THE BALL. THIS CAN BE CHANGED TO MOUSE MOVEMENT IN THE OPTIONS MENU.";
				break;

			case 1:
				tutorialImage.sprite = ResourceManager.SetGUITexture(imageNames.inGamePowerupTutorial);
				tutorialImage.transform.localPosition = tutorialPositions.inGamePowerupTutorialPos1;
				movementText.enabled = false;
				break;

			case 2:
				tutorialImage.sprite = ResourceManager.SetGUITexture(imageNames.inGamePowerupTutorial);
				tutorialImage.transform.localPosition = tutorialPositions.inGamePowerupTutorialPos2;
				movementText.enabled = false;
				break;

			case 3:
				tutorialImage.sprite = ResourceManager.SetGUITexture(imageNames.inGameUnbreakableTutorial);
				tutorialImage.transform.localPosition = tutorialPositions.inGameUnbreakableTutorialPos;
				movementText.enabled = false;
				break;
		}
	}
}
