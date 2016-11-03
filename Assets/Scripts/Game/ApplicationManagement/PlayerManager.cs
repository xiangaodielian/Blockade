using System;
using ApplicationManagement.DebugTools;
using UnityEngine;

namespace ApplicationManagement {
    public class PlayerManager : MonoBehaviour {

        #region Variables

        [System.Serializable]
        private class Prefabs {
            public GameObject paddlePrefab = null;
        }

        [System.Serializable]
        private class ScoringValues {
            public int playerLives = 3;
            public int playerScore = 0;
        }

        public static PlayerManager Instance { get; private set; }
        public Paddle ActivePlayer { get; private set; }

        [SerializeField] private Prefabs prefabs = null;
        [SerializeField] private ScoringValues scoringValues = null;

        private int lifeGrantedAt;

        #endregion

        #region Mono Functions

        private void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;
        }

        private void OnEnable() {
            EventManager.Instance.AddListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        private void OnDisable() {
            EventManager.Instance.RemoveListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        private void Update() {
            if(scoringValues.playerScore - lifeGrantedAt <= 5000)
                return;

            if(scoringValues.playerLives < 99)
                scoringValues.playerLives++;

            lifeGrantedAt = scoringValues.playerScore;
        }

        #endregion

        #region Player Management

        private void SpawnPlayer() {
            if(ActivePlayer)
                throw new InvalidOperationException("Attempt to call SpawnPlayer when Player already exists in scene...");

            ActivePlayer = Instantiate(prefabs.paddlePrefab).GetComponent<Paddle>();
            ActivePlayer.ResetToDefaultState();
        }

        public void DestroyPlayer() {
            if(ActivePlayer == null)
                return;

            Destroy(ActivePlayer.gameObject);
            ActivePlayer = null;
        }

        #endregion

        #region ScoringValues Management

        /// <summary>
        /// Adds value to player lives.
        /// </summary>
        public void AddToPlayerLives(int value) {
            scoringValues.playerLives += value;
        }

        /// <summary>
        /// Sets the player lives to numLives.
        /// </summary>
        public void SetPlayerLives(int numLives) {
            scoringValues.playerLives = numLives;
        }

        /// <summary>
        /// Gets the player lives.
        /// </summary>
        /// <returns>The player lives.</returns>
        public int GetPlayerLives() {
            return scoringValues.playerLives;
        }

        /// <summary>
        /// Adds value to player score.
        /// </summary>
        public void AddToPlayerScore(int value) {
            scoringValues.playerScore += value;
        }

        /// <summary>
        /// Sets the player score to value.
        /// </summary>
        public void SetPlayerScore(int value) {
            scoringValues.playerScore = value;
        }

        /// <summary>
        /// Gets the player score.
        /// </summary>
        /// <returns>The player score.</returns>
        public int GetPlayerScore() {
            return scoringValues.playerScore;
        }

        #endregion

        #region Delegate Listeners

        private void OnSceneChange(LevelManager.SceneChangeEvent e) {
            if(e.scene.name == "MainMenu" || e.scene.name == "EndGame")
                SetPlayerLives(3);
            else if(e.scene.name != "Splash") {
                try {
                    SpawnPlayer();
                } catch(InvalidOperationException exception) {
                    string warning = string.Format("WARNING in {0}: {1}", exception.Source, exception.Message);
                    DebugManager.Logger.LogWarning(warning);
                }
            }
        }

        #endregion
    }
}