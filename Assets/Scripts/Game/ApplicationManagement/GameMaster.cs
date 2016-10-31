using UnityEngine;
using System;
using ApplicationManagement.DebugTools;
using ApplicationManagement.ResourceControl;

namespace ApplicationManagement {
    [RequireComponent(typeof(EventManager))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(GameObjectManager))]
    [RequireComponent(typeof(TimeManager))]
    [RequireComponent(typeof(LevelManager))]
    [RequireComponent(typeof(OptionsManager))]
    public class GameMaster : MonoBehaviour {

        #region variables

        public static GameMaster Instance { get; private set; }

        [Serializable]
        private class AssetBundleOptions {
            public string assetBundleUrl = string.Empty;
            [Tooltip("Use AssetBundles from local file system (use to test).")] public bool useLocalAssetBundles = false;
            [Tooltip("Write Logs to file on disk if TRUE or use Debug.Log if FALSE.")] public bool writeLogToFile = false;
        }

        [Serializable]
        private class Prefabs {
            public GameObject splashPrefab = null;
            public GameObject musicPlayerPrefab = null;
            public GameObject cameraPrefab = null;
            public GameObject guiManagerPrefab = null;
        }

        /*-----MANAGERS-----*/
        public InputManager InputManager { get; private set; }
        public PlayerManager PlayerManager { get; private set; }
        public GameObjectManager GameObjectManager { get; private set; }
        public OptionsManager OptionsManager { get; private set; }
        /*------------------*/

        public static IDebugLogger Logger { get; private set; }

        [SerializeField] private AssetBundleOptions assetBundleOptions = null;
        [SerializeField] private Prefabs prefabs = null;

        #endregion

        #region Mono Functions

        private void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;

            DontDestroyOnLoad(gameObject);

            if(DebugLogger.Instance == null) {
                Logger = new DebugLogger(assetBundleOptions.writeLogToFile);
                DebugLogger.Instance = (DebugLogger)Logger;
            }

            AssignManagers();
            InstantiatePrefabs();
        }

        #endregion

        #region Manager Init

        private void InstantiatePrefabs() {

            if(MusicPlayer.instance)
                DestroyImmediate(MusicPlayer.instance.gameObject);

            Instantiate(prefabs.musicPlayerPrefab);
            MusicPlayer.instance.transform.SetParent(transform);

            if(CameraManager.instance)
                DestroyImmediate(CameraManager.instance.gameObject);

            Instantiate(prefabs.cameraPrefab);
            CameraManager.instance.transform.SetParent(transform);

            Instantiate(prefabs.guiManagerPrefab).GetComponent<GUIManager>();
            GUIManager.Instance.transform.SetParent(transform);
        }

        private void AssignManagers() {
            InputManager = GetComponent<InputManager>();
            PlayerManager = GetComponent<PlayerManager>();
            GameObjectManager = GetComponent<GameObjectManager>();
            OptionsManager = GetComponent<OptionsManager>();

            gameObject.AddComponent<AssetBundleManager>()
                .Init(assetBundleOptions.assetBundleUrl, assetBundleOptions.useLocalAssetBundles);

            // Set IAssetBundler refs
            try {
                ResourceManager.SetAssetBundler(AssetBundleManager.Instance);
                LevelManager.SetAssetBundler(AssetBundleManager.Instance);
            } catch(InvalidOperationException e) {
                string warning = string.Format("PROBLEM in {0}: {1}", e.Source, e.Message);
                Logger.LogWarning(warning);
            }
        }

        #endregion
    }
}