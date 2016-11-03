using UnityEngine;
using UnityEngine.UI;
using ApplicationManagement;

public class InGameGUI : MonoBehaviour {
	
	#region Variables

	[System.Serializable] private class DisplayedGUI{
		public Text scoreText = null;
		public Text timeText = null;
		public Text livesText = null;
		public Text launchPromptText = null;
		public Text powerupNotificationText = null;
		public Image livesImage = null;
	}

	[System.Serializable] private class MenuPanelPrefabs{
		public GameObject inGameMainMenuPanel = null;
		public GameObject inGameOptionsPanel = null;
		public GameObject endLevelPanel = null;
		public GameObject inGameTutorials = null;
	}

	[SerializeField] private DisplayedGUI displayedGUI = null;
	[SerializeField] private MenuPanelPrefabs menuPanelPrefabs = null;

	private GameObject inGameMainMenuPanel;
	private GameObject optionsPanel;
	private GameObject endLevelPanel;
	private GameObject inGameTutorials;
	private int powerupTextTimer = 0;
	private int elapsedTime = 0;
	private int timeDifference = 0;
	private bool runTimer = false;
	private bool promptActive = false;
	private bool displayTutorial = true;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		UpdateScoreText();
		displayedGUI.timeText.text = "00:00";
		UpdateLivesText();
		displayedGUI.livesImage.color = PrefsManager.GetBallColor();

		InstantiatePrefabs();

		displayedGUI.powerupNotificationText.text = "";
		TogglePrompt(false);
		inGameMainMenuPanel.SetActive(false);
		optionsPanel.SetActive(false);
		inGameTutorials.SetActive(false);
	}
	
	void OnGUI(){
		if(!PrefsManager.GetMouseControl()){
			displayedGUI.launchPromptText.text = "PRESS SPACE TO LAUNCH!";
			displayedGUI.launchPromptText.fontSize = 45;
		} else{
			if(Input.mousePresent)
				displayedGUI.launchPromptText.text = "CLICK TO LAUNCH!";
			else
				displayedGUI.launchPromptText.text = "TAP TO LAUNCH!";

			displayedGUI.launchPromptText.fontSize = 60;
		}

		if(runTimer)
			UpdateTimeText();
		
		if(displayedGUI.livesText.text != PlayerManager.Instance.GetPlayerLives().ToString())
			UpdateLivesText();
			
		if(displayedGUI.scoreText.text != PlayerManager.Instance.GetPlayerScore().ToString())
			UpdateScoreText();
			
		if(displayedGUI.livesImage.color != PrefsManager.GetBallColor())
			displayedGUI.livesImage.color = PrefsManager.GetBallColor();

		if(PrefsManager.GetCurrentLevel() == 1 && displayTutorial && PrefsManager.GetLevelUnlocked() <= PrefsManager.GetCurrentLevel()){
			ToggleInGameTutorials(true, 0);
			displayTutorial = false;
		}

		if(PrefsManager.GetCurrentLevel() == 2 && !displayTutorial)
			displayTutorial = true;

		if(PrefsManager.GetCurrentLevel() == 5 && displayTutorial && PrefsManager.GetLevelUnlocked() <= PrefsManager.GetCurrentLevel()){
			ToggleInGameTutorials(true, 3);
			displayTutorial = false;
		}

		//Clear Powerup Notification after 2 sec
		if(powerupTextTimer != 0 && (int)Time.time - powerupTextTimer >= 2){
			powerupTextTimer = 0;
			displayedGUI.powerupNotificationText.text = "";
		}
	}
	
	#endregion
	#region GUI Functions
	
	//Update Time Played Display for Current Level
	private void UpdateTimeText(){
		elapsedTime = (int)Time.timeSinceLevelLoad - timeDifference;
		int elapsedSec = elapsedTime % 60;
		int elapsedMin = elapsedTime / 60;
		if(elapsedMin < 10){
			if(elapsedSec < 10)
				displayedGUI.timeText.text = "0"+elapsedMin+":"+"0"+elapsedSec;
			else
				displayedGUI.timeText.text = "0"+elapsedMin+":"+elapsedSec;
		} else{
			if(elapsedSec < 10)
				displayedGUI.timeText.text = elapsedMin+":"+"0"+elapsedSec;
			else
				displayedGUI.timeText.text = elapsedMin+":"+elapsedSec;
		}
	}
	
	//Update Lives Remaining Display
	private void UpdateLivesText(){
		if(PlayerManager.Instance.GetPlayerLives() < 10)
			displayedGUI.livesText.text = "X 0"+PlayerManager.Instance.GetPlayerLives().ToString();
		else
			displayedGUI.livesText.text = "X "+PlayerManager.Instance.GetPlayerLives().ToString();
	}
	
	//Update Current Score Display
	private void UpdateScoreText(){
		displayedGUI.scoreText.text = PlayerManager.Instance.GetPlayerScore().ToString();
	}
	
	public void TogglePrompt(bool isOn){
		displayedGUI.launchPromptText.gameObject.SetActive(isOn);
	}
	
	//Toggle In Game Menu
	public void ToggleMenu(){
		if(endLevelPanel == null && !inGameTutorials.activeSelf)
			TimeManager.Pause();

		if(!inGameMainMenuPanel.activeSelf && !optionsPanel.activeSelf)
			GUIManager.Instance.PlayMenuToggleSound(true);
		else
			GUIManager.Instance.PlayMenuToggleSound(false);

		if(optionsPanel.activeSelf)
			optionsPanel.SetActive(false);
		else
			inGameMainMenuPanel.SetActive(!inGameMainMenuPanel.activeSelf);

		if(promptActive){
			displayedGUI.launchPromptText.gameObject.SetActive(true);
			promptActive = false;
		}

		if(displayedGUI.launchPromptText.gameObject.activeSelf && inGameMainMenuPanel.activeSelf){
			displayedGUI.launchPromptText.gameObject.SetActive(false);
			promptActive = true;
		}
	}
	
	public void InGameOptions(){
		inGameMainMenuPanel.SetActive(false);
		optionsPanel.SetActive(true);
	}
	
	public void InGameMain(){
		optionsPanel.SetActive(false);
		inGameMainMenuPanel.SetActive(true);
	}
	
	public void ToggleEndLevelPanel(bool isActive){
		if(!inGameMainMenuPanel.activeSelf && !optionsPanel.activeSelf && isActive)
			TimeManager.Pause();

		if(endLevelPanel == null && isActive){
			endLevelPanel = Instantiate(menuPanelPrefabs.endLevelPanel);
			endLevelPanel.transform.SetParent(transform, false);
		}else if(!isActive){
			Destroy(endLevelPanel);
			endLevelPanel = null;
			LevelManager.Instance.ChangeToLevel("Next");
			TimeManager.Pause();
		}
	}

	public void ToggleInGameTutorials(bool visible, int tutNum){
		TimeManager.Pause();
		inGameTutorials.SetActive(visible);

		if(visible)
			inGameTutorials.GetComponent<TutorialGUI>().SetTutorial(tutNum);
			
	}

	public void DisplayPowerupNotification(Powerup.PowerupType type){
		switch(type){
			case Powerup.PowerupType.Expand:
				displayedGUI.powerupNotificationText.text = "OBTAINED EXPAND!";
				break;

			case Powerup.PowerupType.Explode:
				displayedGUI.powerupNotificationText.text = "OBTAINED EXPLOSIVE BALL!";
				break;

			case Powerup.PowerupType.FeatherBall:
				displayedGUI.powerupNotificationText.text = "FEATHER BALL: BALL DAMAGE HALVED!";
				break;

			case Powerup.PowerupType.IronBall:
				displayedGUI.powerupNotificationText.text = "IRON BALL: BALL DAMAGE DOUBLED!";
				break;

			case Powerup.PowerupType.Lasers:
				if(PrefsManager.GetMouseControl())
					displayedGUI.powerupNotificationText.text = "CLICK TO FIRE LASERS!";
				else
					displayedGUI.powerupNotificationText.text = "PRESS SPACE TO FIRE LASERS!";
				break;

			case Powerup.PowerupType.Mirror:
				displayedGUI.powerupNotificationText.text = "MOVEMENT MIRRORED!";
				break;

			case Powerup.PowerupType.Multiball:
				displayedGUI.powerupNotificationText.text = "MULTIBALL!";
				break;

			case Powerup.PowerupType.Shrink:
				displayedGUI.powerupNotificationText.text = "OBTAINED SHRINK!";
				break;

			case Powerup.PowerupType.SlowDown:
				displayedGUI.powerupNotificationText.text = "BALL SPEED LOWERED!";
				break;

			case Powerup.PowerupType.SpeedUp:
				displayedGUI.powerupNotificationText.text = "BALL SPEED RAISED!";
				break;

			case Powerup.PowerupType.StickyBall:
				displayedGUI.powerupNotificationText.text = "OBTAINED STICKY BALL!";
				break;

			default:
				Debug.LogError("Invalid Powerup Type!");
				break;
		}

		powerupTextTimer = (int)Time.time;
	}
	
	#endregion
	#region Utility Functions

	void InstantiatePrefabs(){
		inGameMainMenuPanel = Instantiate(menuPanelPrefabs.inGameMainMenuPanel);
		inGameMainMenuPanel.transform.SetParent(this.transform);
		inGameMainMenuPanel.transform.localPosition = Vector3.zero;

		optionsPanel = Instantiate(menuPanelPrefabs.inGameOptionsPanel);
		optionsPanel.transform.SetParent(this.transform);
		optionsPanel.transform.localPosition = Vector3.zero;

		inGameTutorials = Instantiate(menuPanelPrefabs.inGameTutorials);
		inGameTutorials.transform.SetParent(this.transform);
		inGameTutorials.transform.localPosition = Vector3.zero;
	}

	//Sets the offset for Level Timer
	public void SetTimeDifference(int dif){
		timeDifference = dif;
		runTimer = true;
	}

	public void StopTimer(){
		runTimer = false;
		timeDifference = (int)Time.timeSinceLevelLoad;
		UpdateTimeText();
	}

	//Calculate Bonus Points for Level based on Time in Level
	public void CalculateTimeBonus(){
		Text timeBonusText = (Text)GameObject.Find("TimeBonusText").GetComponent<Text>();
		int timeBonus = 0;
		if(Time.timeSinceLevelLoad < 1.99f)
			timeBonus = LevelManager.GetLevelNum()*1000;
		else{
			timeBonus = (LevelManager.GetLevelNum()*1000)/Mathf.Clamp((elapsedTime/10),1,1000);	
		}
			
		int elapsedSec = elapsedTime % 60;
		int elapsedMin = elapsedTime / 60;
		if(elapsedMin < 10){
			if(elapsedSec < 10)
				timeBonusText.text = "0"+elapsedMin+":"+"0"+elapsedSec+" = "+timeBonus;
			else
				timeBonusText.text = "0"+elapsedMin+":"+elapsedSec+" = "+timeBonus;
		} else{
			if(elapsedSec < 10)
				timeBonusText.text = elapsedMin+":"+"0"+elapsedSec+" = "+timeBonus;
			else
				timeBonusText.text = elapsedMin+":"+elapsedSec+" = "+timeBonus;
		}
		
		PlayerManager.Instance.AddToPlayerScore(timeBonus);
	}

	#endregion
}