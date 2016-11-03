/*
 * GameObjectManager Class
 * Handles Instantiation and Destruction of all GameObjects
 */

using System;
using UnityEngine;
using ApplicationManagement.DebugTools;

namespace ApplicationManagement {
    public class GameObjectManager : MonoBehaviour {

        #region Variables

        /*----EVENTS----*/
        public class SpawnPowerupEvent : GameEvent {
            public Vector3 powerupLocation;
            public Powerup.PowerupType powerupType;

            public SpawnPowerupEvent(Vector3 location, Powerup.PowerupType type) {
                powerupLocation = location;
                powerupType = type;
            }
        }
        /*--------------*/

        [System.Serializable]
        private class Prefabs {
            public GameObject playspacePrefab = null;
            public GameObject starsPSPrefab = null;
            public GameObject ballPrefab = null;
            public GameObject powerupPrefab = null;
        }

        public static GameObjectManager Instance { get; private set; }

        [SerializeField] private Prefabs prefabs = null;

        private static GameObject stars;

        #endregion

        #region Mono Functions

        private void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;
        }

        private void OnEnable() {
            EventManager.Instance.AddListener<LevelManager.LevelResetEvent>(OnLevelReset);
            EventManager.Instance.AddListener<LevelManager.SceneChangeEvent>(OnSceneChange);
            EventManager.Instance.AddListener<SpawnPowerupEvent>(OnSpawnPowerup);
        }

        private void OnDisable() {
            EventManager.Instance.RemoveListener<LevelManager.LevelResetEvent>(OnLevelReset);
            EventManager.Instance.RemoveListener<LevelManager.SceneChangeEvent>(OnSceneChange);
            EventManager.Instance.RemoveListener<SpawnPowerupEvent>(OnSpawnPowerup);
        }

        #endregion

        #region GameObject Management

        /// <summary>
        /// Populates the powerups of current bricks.
        /// </summary>
        private static void PopulatePowerups() {
            Brick[] bricksInScene = FindObjectsOfType<Brick>();
            foreach(Brick brick in bricksInScene) {
                if(brick.tag == "Breakable")
                    brick.SetPowerup();
            }
        }

        private void StartStars() {
            stars = Instantiate(prefabs.starsPSPrefab);
            stars.transform.SetParent(transform);
        }

        private void SpawnBall() {
            Instantiate(prefabs.ballPrefab);
        }

        private void SpawnPowerup(Vector3 location, Powerup.PowerupType type) {
            Powerup powerup = prefabs.powerupPrefab.GetComponent<Powerup>();
            powerup.powerupType = type;

            Instantiate(prefabs.powerupPrefab, location, Quaternion.identity);
        }

        /// <summary>
        /// Clears all active GameObjects.
        /// </summary>
        public static void ClearGameObjects() {
            if(PlaySpace.instance)
                Destroy(PlaySpace.instance.gameObject);

            try {
                PlayerManager.Instance.DestroyPlayer();
            } catch(InvalidOperationException e) {
                string warning = string.Format("WARNING in {0}: {1}", e.Source, e.Message);
                DebugManager.Logger.LogWarning(warning);
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

        private void OnSpawnPowerup(SpawnPowerupEvent e) {
            SpawnPowerup(e.powerupLocation, e.powerupType);
        }

        #endregion
    }
}