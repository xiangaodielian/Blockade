using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class GUIManager : MonoBehaviour {
	
	#region Variables
	
	[System.Serializable] private class MenuPrefabs{
		public GameObject mainMenu = null;
		public GameObject levelSelectMenu = null;
		public GameObject optionsMenu = null;
		public GameObject highScoresMenu = null;
		public GameObject winMenu = null;
		public GameObject loseMenu = null;
		public GameObject inGameUI = null;
		public GameObject debugUI = null;
		public GameObject menuTransitionPanel = null;
		public GameObject quitConfirmPanel = null;
		public GameObject interviewConfirmPanel = null;
		public GameObject interviewerTutorialPanel = null;
	}

	[System.Serializable] private class GUISounds{
		public string buttonHighlight = "";
		public string buttonClick = "";
		public string inGameMenuOpen = "";
		public string inGameMenuClose = "";
	}

	public enum TargetMenuOptions{
		mainMenu,
		levelSelect,
		optionsMenu,
		highScore,
		winMenu,
		loseMenu
	};

	public static GUIManager instance {get; private set;}

	public InGameGUI inGameGUI {get; private set;}
	public DebugUI debugUI {get; private set;}

	[SerializeField] private MenuPrefabs menuPrefabs = null;
	[SerializeField] private GUISounds guiSounds = null;

	private GameObject targetMenu = null;
	private GameObject activeMenu = null;
	private GameObject mainMenu = null;
	private GameObject levelSelectMenu = null;
	private GameObject optionsMenu = null;
	private GameObject highScoresMenu = null;
	private GameObject winMenu = null;
	private GameObject loseMenu = null;
	private GameObject quitConfirm = null;
	private GameObject interviewConfirm = null;
	private GameObject interviewerTutorialPanel = null;
	private SceneFader menuTransitionPanel = null;
	private AudioSource audioSource;
	
	#endregion
	#region Mono Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);

		instance = this;

		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();

		debugUI = Instantiate(menuPrefabs.debugUI).GetComponent<DebugUI>();
		debugUI.transform.SetParent(this.transform, false);
	}
	
	#endregion
	#region GUI Audio

	public void PlayButtonSound(){
		audioSource.clip = ResourceManager.LoadAudioClip(guiSounds.buttonClick);
		audioSource.Play();
	}

	public void PlayMenuToggleSound(bool isOpen){
		if(isOpen)
			audioSource.clip = ResourceManager.LoadAudioClip(guiSounds.inGameMenuOpen);
		else
			audioSource.clip = ResourceManager.LoadAudioClip(guiSounds.inGameMenuClose);

		audioSource.Play();
	}

	#endregion
	#region GUI Management
	
	public void ToggleQuitConfirm(bool visible){
		quitConfirm.SetActive(visible);
		mainMenu.GetComponent<MainMenu>().SetButtonInteraction(!visible);
	}

	public void ToggleInterviewConfirm(bool visible){
		interviewConfirm.SetActive(visible);
		mainMenu.GetComponent<MainMenu>().SetButtonInteraction(!visible);
	}

	public void MenuFadeTransition(string screenToLoad){
		menuTransitionPanel.StartFade(screenToLoad);
	}

	#endregion
	#region Menu Instantiation/Destruction

	void InstantiateMainMenus(){
		mainMenu = Instantiate(menuPrefabs.mainMenu);
		mainMenu.transform.SetParent(this.transform, false);

		levelSelectMenu = Instantiate(menuPrefabs.levelSelectMenu);
		levelSelectMenu.transform.SetParent(this.transform, false);
		levelSelectMenu.SetActive(false);

		optionsMenu = Instantiate(menuPrefabs.optionsMenu);
		optionsMenu.transform.SetParent(this.transform, false);
		optionsMenu.SetActive(false);

		highScoresMenu = Instantiate(menuPrefabs.highScoresMenu);
		highScoresMenu.transform.SetParent(this.transform, false);
		highScoresMenu.SetActive(false);

		interviewConfirm = Instantiate(menuPrefabs.interviewConfirmPanel);
		interviewConfirm.transform.SetParent(this.transform, false);
		interviewConfirm.SetActive(false);

		quitConfirm = Instantiate(menuPrefabs.quitConfirmPanel);
		quitConfirm.transform.SetParent(this.transform, false);
		quitConfirm.SetActive(false);

		menuTransitionPanel = Instantiate(menuPrefabs.menuTransitionPanel).GetComponent<SceneFader>();
		menuTransitionPanel.transform.SetParent(this.transform, false);

		activeMenu = mainMenu;
	}

	void DestroyMainMenus(){
		if(mainMenu != null){
			Destroy(mainMenu);
			mainMenu = null;

			Destroy(levelSelectMenu);
			levelSelectMenu = null;

			Destroy(optionsMenu);
			optionsMenu = null;

			Destroy(highScoresMenu);
			highScoresMenu = null;

			Destroy(interviewConfirm);
			interviewConfirm = null;

			Destroy(quitConfirm);
			quitConfirm = null;

			Destroy(menuTransitionPanel.gameObject);
			menuTransitionPanel = null;
		}
	}

	void InstantiateInGameGUI(){
		if(inGameGUI == null){
			inGameGUI = Instantiate(menuPrefabs.inGameUI).GetComponent<InGameGUI>();
			inGameGUI.transform.SetParent(this.transform);
			inGameGUI.transform.localScale = Vector3.one;
			inGameGUI.transform.localPosition = Vector3.zero;
		}
	}

	public void DestroyInGameGUI(TargetMenuOptions menu){
		Destroy(inGameGUI.gameObject);
		inGameGUI = null;

		StartCoroutine(SetTargetMenu(menu));
	}

	void InstantiateEndGameMenus(){
		winMenu = Instantiate(menuPrefabs.winMenu);
		winMenu.transform.SetParent(this.transform);
		winMenu.transform.localScale = Vector3.one;
		winMenu.transform.localPosition = Vector3.zero;
		winMenu.SetActive(false);

		loseMenu = Instantiate(menuPrefabs.loseMenu);
		loseMenu.transform.SetParent(this.transform);
		loseMenu.transform.localScale = Vector3.one;
		loseMenu.transform.localPosition = Vector3.zero;
		loseMenu.SetActive(false);
	}

	public void DestroyEndGameMenus(){
		if(loseMenu != null){
			Destroy(winMenu);
			winMenu = null;

			Destroy(loseMenu);
			loseMenu = null;

			StartCoroutine(SetTargetMenu(TargetMenuOptions.highScore));
		}
	}

	public void InstantiateInterviewerTutorial(){
		interviewerTutorialPanel = Instantiate(menuPrefabs.interviewerTutorialPanel);
		interviewerTutorialPanel.transform.SetParent(this.transform, false);
	}

	void DestroyInterviewerTutorial(){
		if(interviewerTutorialPanel != null){
			Destroy(interviewerTutorialPanel);
			interviewerTutorialPanel = null;
		}
	}

	#endregion
	#region Utility

	public IEnumerator SetTargetMenu(TargetMenuOptions menu){
		if(menu == TargetMenuOptions.winMenu || menu == TargetMenuOptions.loseMenu){
			while(winMenu == null && loseMenu == null)
				yield return null;

			if(menu == TargetMenuOptions.winMenu)
				targetMenu = winMenu;
			else
				targetMenu = loseMenu;
		} else{
			while(mainMenu == null)
				yield return null;

			switch(menu){
				case TargetMenuOptions.mainMenu:
					targetMenu = mainMenu;
					break;

				case TargetMenuOptions.levelSelect:
					targetMenu = levelSelectMenu;
					break;

				case TargetMenuOptions.optionsMenu:
					targetMenu = optionsMenu;
					break;

				case TargetMenuOptions.highScore:
					targetMenu = highScoresMenu;
					break;
			}
		}
		
		ChangeToTargetMenu();
	}

	void ChangeToTargetMenu(){
		if(targetMenu == null)
			targetMenu = mainMenu;

		if(activeMenu != null)
			activeMenu.SetActive(false);

		targetMenu.SetActive(true);

		activeMenu = targetMenu;
		targetMenu = null;
	}

	#endregion
	#region Delegates

	/// <summary>
	/// Delegate Listener for SceneChange call from SceneManager.sceneLoaded Event.
	/// </summary>
	public void SceneChange(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode){
		if(scene.name == "MainMenu"){
			InstantiateMainMenus();
			mainMenu.GetComponent<MainMenu>().UpdateCheck();
		} else if(scene.name == "EndGame")
			InstantiateEndGameMenus();
		else if(scene.name.Contains("Level")){
			DestroyMainMenus();
			DestroyEndGameMenus();
			DestroyInterviewerTutorial();
			InstantiateInGameGUI();
			inGameGUI.TogglePrompt(true);
		}
	}

	#endregion
}