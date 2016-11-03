using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using ApplicationManagement.DebugTools;
using ApplicationManagement.ResourceControl;

namespace ApplicationManagement {
    public class LevelManager : MonoBehaviour {

        #region Variables

        /*----EVENTS----*/
        public class SceneChangeEvent : GameEvent {
            public Scene scene;
            public LoadSceneMode mode;

           public SceneChangeEvent(Scene s, LoadSceneMode m) {
                scene = s;
                mode = m;
            }
        }

        public class LevelResetEvent : GameEvent { }

        public class LevelFinishEvent : GameEvent { }
        /*-------------*/

        public static LevelManager Instance { get; private set; }
        private static IAssetBundler assetBundler;

        public AsyncOperation AsyncOp { get; private set; }

        private string previousLevel;

        #endregion

        #region Mono Functions

        private void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneChange;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneChange;
        }

        #endregion

        #region Level Loading

        public static void LoadLevel(string name) {
            if(name == "Splash")
                return;

            try {
                assetBundler.GetScenePath(name);
                SceneManager.LoadScene(name);
            } catch(ArgumentException e) {
                string error = string.Format("ERROR in {0}; {1}\n{2}", e.Source, e.Message, e.StackTrace);
                DebugManager.Logger.LogError(1, error);
            }
        }

        public static AsyncOperation LoadLevelAsync(string name) {
            try {
                assetBundler.GetScenePath(name);
                return SceneManager.LoadSceneAsync(name);
            } catch(ArgumentException e) {
                string error = string.Format("ERROR in {0}; {1}\n{2}", e.Source, e.Message, e.StackTrace);
                DebugManager.Logger.LogError(1, error);
            }

            return null;
        }

        public static void LoadNextLevel() {
            string nextLevel = "Level_";
            int curLevelNum = GetLevelNum();
            curLevelNum++;

            if(curLevelNum < 10)
                nextLevel += "0";

            nextLevel += curLevelNum.ToString();

            try {
                assetBundler.GetScenePath(nextLevel);
                SceneManager.LoadSceneAsync(nextLevel);
            } catch(ArgumentException e) {
                string error = string.Format("ERROR in {0}; {1}", e.Source, e.Message);
                DebugManager.Logger.LogError(1, error);
            }
        }

        public static void ReloadLevel() {
            SceneManager.LoadScene(GetCurrentLevel());
        }

        #endregion

        #region Level Management

        public void ChangeToLevel(string level) {
            if(PlayerManager.Instance.ActivePlayer != null)
                PlayerManager.Instance.ActivePlayer.firstBall = true;

            if(level == "Next") {
                if(GetLevelNum() == 20) {
                    GUIManager.Instance.DestroyInGameGUI(GUIManager.TargetMenuOptions.WinMenu);
                    LoadLevel("EndGame");
                } else
                    LoadNextLevel();
            } else {
                if(level == "LatestCheckpoint") {
                    int latestCheckpoint = PrefsManager.GetLatestCheckpoint();

                    if(latestCheckpoint < 10)
                        level = "Level_0" + latestCheckpoint;
                    else
                        level = "Level_" + latestCheckpoint;
                }

                LoadLevel(level);
            }
        }

        public IEnumerator ChangeToLevelAsync(string level) {
            if(PlayerManager.Instance.ActivePlayer != null)
                PlayerManager.Instance.ActivePlayer.firstBall = true;

            if(level == "LatestCheckpoint") {
                int latestCheckpoint = PrefsManager.GetLatestCheckpoint();

                if(latestCheckpoint < 10)
                    level = "Level_0" + latestCheckpoint;
                else
                    level = "Level_" + latestCheckpoint;
            }

            AsyncOp = LoadLevelAsync(level);

            while(!AsyncOp.isDone)
                yield return null;

            AsyncOp = null;
        }

        public void ResetCurrentLevel() {
            EventManager.Instance.Raise(new LevelResetEvent());
            GUIManager.Instance.InGameGui.TogglePrompt(true);
        }

        public void RestartLevel() {
            PlayerManager.Instance.SetPlayerScore(0);
            PlayerManager.Instance.SetPlayerLives(3);

            ChangeToLevel(previousLevel);
        }

        #endregion

        #region Application Management

        public static void QuitApplication() {
            Application.Quit();
        }

        #endregion

        #region Utility

        public static void SetAssetBundler(IAssetBundler bundler) {
            if(assetBundler == null)
                assetBundler = bundler;
            else
                throw new InvalidOperationException("IAssetBundler in LevelManager has already been set...");
        }

        public static string GetCurrentLevel() {
            return SceneManager.GetActiveScene().name;
        }

        public static int GetLevelNum() {
            string curLevel = GetCurrentLevel();

            curLevel = curLevel.Replace(curLevel.Contains("Level_0") ? "Level_0" : "Level_", "");

            return int.Parse(curLevel);
        }

        public void SetPreviousLevel() {
            previousLevel = GetCurrentLevel();
        }

        #endregion

        #region Delegate Listeners

        private void OnSceneChange(Scene scene, LoadSceneMode mode) {
            EventManager.Instance.Raise(new SceneChangeEvent(scene, mode));

            if(scene.name == "MainMenu") {
                EventManager.Instance.Raise(new LevelFinishEvent());

                OptionsManager.Instance.SetAudioClip();
            } else if(scene.name.Contains("Level")) {
                PrefsManager.SetCurrentLevel(GetLevelNum());

                if(GetLevelNum() > PrefsManager.GetLatestCheckpoint()) {
                    PrefsManager.SetLatestCheckpoint(GetLevelNum());
                    PrefsManager.SetLevelUnlocked(GetLevelNum());
                }
            } else if(scene.name != "Splash")
                EventManager.Instance.Raise(new LevelFinishEvent());

            StartCoroutine(ResourceManager.UnloadUnusedResources());
        }

        #endregion
    }
}