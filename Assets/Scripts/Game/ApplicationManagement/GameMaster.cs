/*
 * GameMaster Class
 * Responsible for Initiating Game from Splash
 * Handles all high level Manager Instantiation/Initialization 
 */

using UnityEngine;
using System;
using ApplicationManagement.DebugTools;
using ApplicationManagement.ResourceControl;

namespace ApplicationManagement {
    [RequireComponent(typeof(AssetBundleManager))]
    [RequireComponent(typeof(DebugManager))]
    [RequireComponent(typeof(EventManager))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(LevelManager))]
    [RequireComponent(typeof(OptionsManager))]
    [RequireComponent(typeof(TimeManager))]
    public class GameMaster : MonoBehaviour {

        #region variables

        public static GameMaster Instance { get; private set; }

        [Serializable]
        private class AssetBundleOptions {
            public string assetBundleUrl = string.Empty;
            [Tooltip("Use AssetBundles from local file system (use to test).")]
            public bool useLocalAssetBundles = false;
        }

        [Serializable]
        private class DebugOptions {
            [Tooltip("Write Logs to file on disk if TRUE or use Debug.Log if FALSE.")]
            public bool writeLogToFile = false;
        }

        [Serializable]
        private class Prefabs {
            public GameObject audioManagerPrefab = null;
            public GameObject cameraManagerPrefab = null;
            public GameObject gameObjectManagerPrefab = null;
            public GameObject guiManagerPrefab = null;
            public GameObject playerManagerPrefab = null;
        }

        [SerializeField] private AssetBundleOptions assetBundleOptions = null;
        [SerializeField] private DebugOptions debugOptions = null;
        [SerializeField] private Prefabs managerPrefabs = null;

        #endregion

        #region Mono Functions

        private void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;

            DontDestroyOnLoad(gameObject);

            InitManagers();
            InstantiatePrefabs();
        }

        #endregion

        #region Manager Init

        /// <summary>
        /// Instantiate and parent Managers that need to be instantiated.
        /// (Only Managers that spawn other Objects are instantiated.)
        /// </summary>
        private void InstantiatePrefabs() {
            if(AudioManager.Instance)
                DestroyImmediate(AudioManager.Instance.gameObject);
            Instantiate(managerPrefabs.audioManagerPrefab);
            AudioManager.Instance.transform.SetParent(transform);

            if(CameraManager.Instance)
                DestroyImmediate(CameraManager.Instance.gameObject);
            Instantiate(managerPrefabs.cameraManagerPrefab);
            CameraManager.Instance.transform.SetParent(transform);

            if(GameObjectManager.Instance)
                DestroyImmediate(GameObjectManager.Instance.gameObject);
            Instantiate(managerPrefabs.gameObjectManagerPrefab);
            GameObjectManager.Instance.transform.SetParent(transform);

            if(GUIManager.Instance)
                DestroyImmediate(GUIManager.Instance.gameObject);
            Instantiate(managerPrefabs.guiManagerPrefab);
            GUIManager.Instance.transform.SetParent(transform);

            if(PlayerManager.Instance)
                DestroyImmediate(PlayerManager.Instance.gameObject);
            Instantiate(managerPrefabs.playerManagerPrefab);
            PlayerManager.Instance.transform.SetParent(transform);
        }

        /// <summary>
        /// Initialize Managers that need to be initialized.
        /// </summary>
        private void InitManagers() {
            DebugManager.Instance.InitLogger(debugOptions.writeLogToFile);
            AssetBundleManager.Instance.Init(assetBundleOptions.assetBundleUrl, assetBundleOptions.useLocalAssetBundles);
        }

        #endregion
    }
}