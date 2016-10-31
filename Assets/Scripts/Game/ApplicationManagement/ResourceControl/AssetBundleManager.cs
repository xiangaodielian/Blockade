using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ApplicationManagement.ResourceControl {
    public class AssetBundleManager : MonoBehaviour, IAssetBundler {

        #region Variables

        public static AssetBundleManager Instance { get; private set; }

        [HideInInspector]
        public static float TotalDownloadProgress { get; private set; }

        private readonly Dictionary<string, AssetBundle> assetBundleDict = new Dictionary<string, AssetBundle>();
        private string mainAssetBundleUrl;
        private bool useLocalBundles;
        private AssetBundleManifest manifest;
        private float[] downloadProgressArray;
        private string[] scenePaths;

        #endregion

        #region Mono Functions

        private void Awake() {
            if((Instance != null) && (Instance != this))
                Destroy(this);

            Instance = this;
        }

        private void OnEnable() {
            EventManager.Instance.AddListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        private void OnDisable() {
            EventManager.Instance.RemoveListener<LevelManager.SceneChangeEvent>(OnSceneChange);
        }

        private void Update() {
            if(downloadProgressArray == null)
                return;

            TotalDownloadProgress = 0f;

            foreach(float progress in downloadProgressArray)
                TotalDownloadProgress += progress;

            if(Math.Abs(TotalDownloadProgress - 1f) < 0.01f)
                downloadProgressArray = null;
        }

        #endregion

        #region Bundle Management

        /// <summary>
        ///     Loads the main bundle manifest and uses it to download and cache any updated or non-cached bundles (queues and
        ///     loads any dependencies first).
        /// </summary>
        private IEnumerator LoadBundleManifest(string url) {
            GameMaster.Logger.LogInfo("Retrieving AssetBundle Manifest...");

            using(WWW www = new WWW(url)) {
                yield return www;

                if(!string.IsNullOrEmpty(www.error))
                    throw new WebException(www.error + " in LoadAssetBundle method...");

                manifest = www.assetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
                www.assetBundle.Unload(false);

                string[] bundleNames = manifest.GetAllAssetBundles();
                Queue<string> bundleQueue = new Queue<string>();

                foreach(string bundleName in bundleNames) {
                    string[] dependencies = manifest.GetAllDependencies(bundleName);
                    if(dependencies.Length > 0)
                        foreach(string dependent in dependencies)
                            if(!bundleQueue.Contains(dependent))
                                bundleQueue.Enqueue(dependent);

                    bundleQueue.Enqueue(bundleName);
                }

                string bundlePath = GetBundlePath();

                downloadProgressArray = new float[bundleQueue.Count];
                int bundleIndex = 0;

                try {
                    while(bundleQueue.Count > 0) {
                        StartCoroutine(LoadAssetBundle(bundlePath, bundleQueue.Dequeue(), bundleIndex));
                        bundleIndex++;
                    }
                } catch(WebException e) {
                    string error = string.Format("ERROR in {0}: {1}", e.Source, e.Message);
                    GameMaster.Logger.LogError(1, error);
                }
            }
        }

        /// <summary>
        ///     Download the bundle or load from cache.
        /// </summary>
        /// <param name="path">Path to AssetBundles.</param>
        /// <param name="bundleName">Name of AssetBundle to load.</param>
        /// <param name="bundleIndex">Only used during initial AssetBundle loading sequence during Splash</param>
        private IEnumerator LoadAssetBundle(string path, string bundleName, int bundleIndex = -1) {
            GameMaster.Logger.LogInfo(string.Format("Retrieving AssetBundle {0} from {1}", bundleName, path));

            while(!Caching.ready)
                yield return null;

            using(WWW www = WWW.LoadFromCacheOrDownload(path + bundleName, manifest.GetAssetBundleHash(bundleName))) {
                if(!Caching.IsVersionCached(path + bundleName, manifest.GetAssetBundleHash(bundleName))) {
                    while(!www.isDone) {
                        if(downloadProgressArray != null)
                            downloadProgressArray[bundleIndex] = www.progress/downloadProgressArray.Length;

                        yield return null;
                    }

                    if((downloadProgressArray != null) &&
                       (Math.Abs(downloadProgressArray[bundleIndex] - 1f/downloadProgressArray.Length) > 0.05f))
                        downloadProgressArray[bundleIndex] = 1f/downloadProgressArray.Length;

                    if(!string.IsNullOrEmpty(www.error))
                        throw new WebException(www.error + " in LoadAssetBundle method...");
                } else {
                    if(downloadProgressArray != null)
                        downloadProgressArray[bundleIndex] = 1f/downloadProgressArray.Length;
                }

                if(bundleName == "levels")
                    scenePaths = www.assetBundle.GetAllScenePaths();

                if(bundleName.Contains("textures") && (bundleIndex >= 0))
                    www.assetBundle.Unload(false);
                else if(!assetBundleDict.ContainsKey(bundleName))
                    assetBundleDict.Add(bundleName, www.assetBundle);
            }
        }

        /// <summary>
        ///     Unloads unused bundles
        /// </summary>
        public void UnloadUnusedBundles() {
            GameMaster.Logger.LogInfo("Unloading AssetBundles...");

            List<string> keys = new List<string>(assetBundleDict.Keys);

            foreach(string key in keys) {
                AssetBundle curBundle;

                if(!assetBundleDict.TryGetValue(key, out curBundle))
                    throw new KeyNotFoundException(
                        "Key not found in assetBundleDictionary in UnloadUnusedBundles method...");

                curBundle.Unload(false);
                assetBundleDict.Remove(key);
            }
        }

        #endregion

        #region Asset Management

        /// <summary>
        ///     Loads an Asset from an AssetBundle.
        /// </summary>
        public T LoadAsset<T>(string assetName, BundleType bundleType) where T : UnityEngine.Object {
            assetName = assetName.ToLower();
            string bundleName;

            switch(bundleType) {
                case BundleType.Textures:
                    if(PrefsManager.GetTextureRes() == 0)
                        bundleName = "textures.sd";
                    else if(PrefsManager.GetTextureRes() == 1)
                        bundleName = "textures.hd";
                    else
                        bundleName = "textures.ud";
                    break;

                case BundleType.Gui:
                    bundleName = "gui";
                    break;

                case BundleType.Audio:
                    bundleName = "audio";
                    break;

                default:
                    throw new ArgumentException("Invalid BundleType provided in LoadAsset method...",
                        bundleType.ToString());
            }

            try {
                if(!assetBundleDict.ContainsKey(bundleName)) {
                    string bundlePath = GetBundlePath();
                    StartCoroutine(LoadAssetBundle(bundlePath, bundleName));
                    while(!assetBundleDict.ContainsKey(bundleName)) {}
                }
            } catch(WebException e) {
                string error = string.Format("ERROR in {0}: {1}", e.Source, e.Message);
                GameMaster.Logger.LogError(1, error);
            }

            AssetBundle curBundle;

            if(assetBundleDict.TryGetValue(bundleName, out curBundle))
                return (T) curBundle.LoadAsset(assetName);

            throw new KeyNotFoundException("Key was not found in assetBundleDictionary in LoadAsset method...");
        }

        /// <summary>
        ///     Checks if given Scene has an existing path.
        /// </summary>
        public void GetScenePath(string requestedScene) {
            if(!scenePaths.Any(path => path.Contains(requestedScene)))
                throw new ArgumentException("Requested Scene (" + requestedScene + ") not found...");

            if(assetBundleDict.ContainsKey("levels"))
                return;

            string bundlePath = GetBundlePath();

            try {
                StartCoroutine(LoadAssetBundle(bundlePath, "levels"));
                while(!assetBundleDict.ContainsKey("levels")) {}
            } catch(WebException e) {
                string error = string.Format("ERROR in {0}: {1}", e.Source, e.Message);
                GameMaster.Logger.LogError(1, error);
            }
        }

        #endregion

        #region Utility

        public void Init(string bundleUrl, bool useLocal) {
            mainAssetBundleUrl = bundleUrl;
            useLocalBundles = useLocal;
        }

        //Returns the path for bundles based on platform
        private string GetBundlePath() {
            string bundlePath;

            if(useLocalBundles)
                bundlePath = "file://" + Application.dataPath + "/AssetBundles/";
            else
                bundlePath = mainAssetBundleUrl;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            bundlePath += "Standalone/Win_x86/";
#elif UNITY_STANDALONE_OSX
		    bundlePath += "Standalone/OSX/";
		    #endif

            return bundlePath;
        }

        #endregion

        #region Delegate Listeners

        /// <summary>
        ///     Listener for SceneManager.sceneLoaded Event call.
        /// </summary>
        public void OnSceneChange(LevelManager.SceneChangeEvent e) {
            if(e.scene.name != "Splash")
                return;

            string manifestPath;

            if(useLocalBundles)
                manifestPath = "file://" + Application.dataPath + "/AssetBundles/";
            else
                manifestPath = mainAssetBundleUrl;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            manifestPath += "Standalone/Win_x86/Win_x86";
#elif UNITY_STANDALONE_OSX
		    manifestPath += "Standalone/OSX/OSX";
#endif

            try {
                StartCoroutine(LoadBundleManifest(manifestPath));
            } catch(WebException exception) {
                string error = string.Format("ERROR in {0}: {1}", exception.Source, exception.Message);
                GameMaster.Logger.LogError(1, error);
            }
        }

        #endregion
    }
}