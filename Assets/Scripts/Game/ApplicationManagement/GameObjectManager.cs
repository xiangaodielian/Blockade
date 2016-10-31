using System;
using UnityEngine;
using System.Collections.Generic;

namespace ApplicationManagement {
    public class GameObjectManager : MonoBehaviour {

        #region Variables

        [System.Serializable]
        private class Prefabs {
            public GameObject playspacePrefab = null;
            public GameObject starsPSPrefab = null;
            public GameObject ballPrefab = null;
            public GameObject powerupPrefab = null;
        }

        [SerializeField] private Prefabs prefabs = null;

        private static List<GameObject> breakableBricks = new List<GameObject>();
        private static GameObject stars;

        #endregion

        #region Mono Functions

        void OnEnable() {
            EventManager.Instance.AddListener<LevelManager.LevelResetEvent>(OnLevelReset);
            EventManager.Instance.AddListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        void OnDisable() {
            EventManager.Instance.RemoveListener<LevelManager.LevelResetEvent>(OnLevelReset);
            EventManager.Instance.RemoveListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        #endregion

        #region GameObject Management

        /// <summary>
        /// Populates the powerups of current bricks.
        /// </summary>
        private static void PopulatePowerups() {
            foreach(GameObject brick in breakableBricks)
                brick.GetComponent<Brick>().SetPowerup();
        }

        private void StartStars() {
            stars = Instantiate(prefabs.starsPSPrefab);
            stars.transform.SetParent(transform);
        }

        private void SpawnBall() {
            Instantiate(prefabs.ballPrefab);
        }

        /// <summary>
        /// Clears all active GameObjects.
        /// </summary>
        public static void ClearGameObjects() {
            if(PlaySpace.instance)
                Destroy(PlaySpace.instance.gameObject);

            try {
                GameMaster.Instance.PlayerManager.DestroyPlayer();
            } catch(InvalidOperationException e) {
                string warning = string.Format("WARNING in {0}: {1}", e.Source, e.Message);
                GameMaster.Logger.LogWarning(warning);
            }

            Ball[] ballsInScene = FindObjectsOfType<Ball>();
            if(ballsInScene.Length > 0) {
                foreach(Ball ball in ballsInScene)
                    Destroy(ball.gameObject);
            }

            Brick[] bricksInScene = FindObjectsOfType<Brick>();
            if(bricksInScene.Length > 0) {
                foreach(Brick brick in bricksInScene)
                    Destroy(brick.gameObject);

                breakableBricks = new List<GameObject>();
            }

            Powerup[] powerupsInScene = FindObjectsOfType<Powerup>();
            if(powerupsInScene.Length > 0) {
                foreach(Powerup powerup in powerupsInScene)
                    Destroy(powerup.gameObject);
            }
        }

        #endregion

        #region Delegate Listeners

        private void OnLevelReset(LevelManager.LevelResetEvent e) {
            SpawnBall();
        }

        private void OnSceneChange(LevelManager.SceneChangeEvent e) {
            if(!e.scene.name.Contains("Level")) {
                ClearGameObjects();
                if(e.scene.name == "MainMenu")
                    StartStars();
            } else {
                breakableBricks = new List<GameObject>();

                Ball[] ballsInScene = FindObjectsOfType<Ball>();
                if(ballsInScene.Length > 0) {
                    foreach(Ball ball in ballsInScene)
                        Destroy(ball.gameObject);
                }

                SpawnBall();

                if(!PlaySpace.instance) {
                    Instantiate(prefabs.playspacePrefab);
                    PlaySpace.instance.transform.SetParent(transform);
                    PlaySpace.instance.StartTimer();
                }

                if(LevelManager.GetLevelNum() > 10)
                    PopulatePowerups();
            }
        }

        #endregion
    }
}