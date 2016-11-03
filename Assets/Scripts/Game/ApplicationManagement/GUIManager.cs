using UnityEngine;
using System.Collections;
using ApplicationManagement.ResourceControl;

namespace ApplicationManagement {
    [RequireComponent(typeof(AudioSource))]
    public class GUIManager : MonoBehaviour {

        #region Variables

        [System.Serializable]
        private class MenuPrefabs {
            public GameObject splashPrefab = null;
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

        [System.Serializable]
        private class GuiSounds {
            public string buttonClick = "";
            public string inGameMenuOpen = "";
            public string inGameMenuClose = "";
        }

        public enum TargetMenuOptions {
            MainMenu,
            LevelSelect,
            OptionsMenu,
            HighScore,
            WinMenu,
            LoseMenu
        };

        public static GUIManager Instance { get; private set; }

        public InGameGUI InGameGui { get; private set; }
        public DebugUI DebugUi { get; private set; }

        [SerializeField] private MenuPrefabs menuPrefabs = null;
        [SerializeField] private GuiSounds guiSounds = null;

        private GameObject targetMenu;
        private GameObject activeMenu;
        private GameObject splashScreen;
        private GameObject mainMenu;
        private GameObject levelSelectMenu;
        private GameObject optionsMenu;
        private GameObject highScoresMenu;
        private GameObject winMenu;
        private GameObject loseMenu;
        private GameObject quitConfirm;
        private GameObject interviewConfirm;
        private GameObject interviewerTutorialPanel;
        private SceneFader menuTransitionPanel;
        private AudioSource audioSource;

        #endregion

        #region Mono Functions

        void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;

            audioSource = GetComponent<AudioSource>();
            audioSource.volume = PrefsManager.GetMasterSFXVolume();

            DebugUi = Instantiate(menuPrefabs.debugUI).GetComponent<DebugUI>();
            DebugUi.transform.SetParent(transform, false);
        }

        void OnEnable() {
            EventManager.Instance.AddListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        void OnDisable() {
            EventManager.Instance.RemoveListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        #endregion

        #region GUI Audio

        public void PlayButtonSound() {
            audioSource.clip = ResourceManager.LoadAudioClip(guiSounds.buttonClick);
            audioSource.Play();
        }

        public void PlayMenuToggleSound(bool isOpen) {
            if(isOpen)
                audioSource.clip = ResourceManager.LoadAudioClip(guiSounds.inGameMenuOpen);
            else
                audioSource.clip = ResourceManager.LoadAudioClip(guiSounds.inGameMenuClose);

            audioSource.Play();
        }

        #endregion

        #region GUI Management

        public void ToggleQuitConfirm(bool visible) {
            quitConfirm.SetActive(visible);
            mainMenu.GetComponent<MainMenu>().SetButtonInteraction(!visible);
        }

        public void ToggleInterviewConfirm(bool visible) {
            interviewConfirm.SetActive(visible);
            mainMenu.GetComponent<MainMenu>().SetButtonInteraction(!visible);
        }

        public void MenuFadeTransition(string screenToLoad) {
            menuTransitionPanel.StartFade(screenToLoad);
        }

        #endregion

        #region Menu Instantiation/Destruction

        private void InstantiateMainMenus() {
            if(splashScreen != null)
                Destroy(splashScreen);

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

        void DestroyMainMenus() {
            if(mainMenu != null) {
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

        void InstantiateInGameGUI() {
            if(InGameGui == null) {
                InGameGui = Instantiate(menuPrefabs.inGameUI).GetComponent<InGameGUI>();
                InGameGui.transform.SetParent(this.transform);
                InGameGui.transform.localScale = Vector3.one;
                InGameGui.transform.localPosition = Vector3.zero;
            }
        }

        public void DestroyInGameGUI(TargetMenuOptions menu) {
            Destroy(InGameGui.gameObject);
            InGameGui = null;

            StartCoroutine(SetTargetMenu(menu));
        }

        void InstantiateEndGameMenus() {
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

        public void DestroyEndGameMenus() {
            if(loseMenu != null) {
                Destroy(winMenu);
                winMenu = null;

                Destroy(loseMenu);
                loseMenu = null;

                StartCoroutine(SetTargetMenu(TargetMenuOptions.HighScore));
            }
        }

        public void InstantiateInterviewerTutorial() {
            interviewerTutorialPanel = Instantiate(menuPrefabs.interviewerTutorialPanel);
            interviewerTutorialPanel.transform.SetParent(this.transform, false);
        }

        void DestroyInterviewerTutorial() {
            if(interviewerTutorialPanel != null) {
                Destroy(interviewerTutorialPanel);
                interviewerTutorialPanel = null;
            }
        }

        #endregion

        #region Utility

        public IEnumerator SetTargetMenu(TargetMenuOptions menu) {
            if(menu == TargetMenuOptions.WinMenu || menu == TargetMenuOptions.LoseMenu) {
                while(winMenu == null && loseMenu == null)
                    yield return null;

                if(menu == TargetMenuOptions.WinMenu)
                    targetMenu = winMenu;
                else
                    targetMenu = loseMenu;
            } else {
                while(mainMenu == null)
                    yield return null;

                switch(menu) {
                    case TargetMenuOptions.MainMenu:
                        targetMenu = mainMenu;
                        break;

                    case TargetMenuOptions.LevelSelect:
                        targetMenu = levelSelectMenu;
                        break;

                    case TargetMenuOptions.OptionsMenu:
                        targetMenu = optionsMenu;
                        break;

                    case TargetMenuOptions.HighScore:
                        targetMenu = highScoresMenu;
                        break;
                }
            }

            ChangeToTargetMenu();
        }

        void ChangeToTargetMenu() {
            if(targetMenu == null)
                targetMenu = mainMenu;

            if(activeMenu != null)
                activeMenu.SetActive(false);

            targetMenu.SetActive(true);

            activeMenu = targetMenu;
            targetMenu = null;
        }

        #endregion

        #region Delegate Listeners

        /// <summary>
        /// Delegate Listener for OnSceneChange call from SceneManager.sceneLoaded Event.
        /// </summary>
        public void OnSceneChange(LevelManager.SceneChangeEvent e) {
            if(e.scene.name == "Splash")
                splashScreen = Instantiate(menuPrefabs.splashPrefab);
            else if(e.scene.name == "MainMenu") {
                InstantiateMainMenus();
                mainMenu.GetComponent<MainMenu>().UpdateCheck();
            } else if(e.scene.name == "EndGame")
                InstantiateEndGameMenus();
            else if(e.scene.name.Contains("Level")) {
                DestroyMainMenus();
                DestroyEndGameMenus();
                DestroyInterviewerTutorial();
                InstantiateInGameGUI();
                InGameGui.TogglePrompt(true);
            }
        }

        #endregion
    }
}