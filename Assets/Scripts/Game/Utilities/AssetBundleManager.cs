/*-------------------------------------/
  AssetBundleManager Class - Universal
  Manages the loading/caching of
  AssetBundles from remote and local
  storage
  Writen by Joe Arthur
  Latest Revision - 3 May, 2016
/-------------------------------------*/

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class AssetBundleManager : MonoBehaviour{
	
	#region Variables

	public static AssetBundleManager instance {get; private set;}

	public string mainAssetBundleURL = "";
	[Tooltip("Use AssetBundles from local file system (use to test).")]
	public bool useLocalBundles = false;
	[HideInInspector] public float totalDownloadProgress = 0f;

	private AssetBundleManifest manifest;						//Main bundle manifest
	private Dictionary<string, AssetBundle> assetBundleDict;	//Currently loaded bundles
	private string[] scenePaths = null;							//Paths to all Scenes in "levels" bundle
	private float totalBundles = 0f;
	private float[] downloadProgressArray = null;

	#endregion
	#region Mono Functions

	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		assetBundleDict = new Dictionary<string, AssetBundle>();
	}

	void Update(){
		if(downloadProgressArray != null){
			totalDownloadProgress = 0f;
			foreach(float progress in downloadProgressArray)
				totalDownloadProgress += progress;

			if(totalDownloadProgress == 1f)
				downloadProgressArray = null;
		}
	}

	#endregion
	#region AssetBundle Builds

	#if UNITY_EDITOR
	[MenuItem("Assets/AssetBundles/Build WebGL")]
	static void BuildWebGLAssetBundles(){
		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles"))
			AssetDatabase.CreateFolder("Assets", "AssetBundles");

		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/WebGL"))
			AssetDatabase.CreateFolder("Assets/AssetBundles","WebGL");

		BuildPipeline.BuildAssetBundles("Assets/AssetBundles/WebGL", BuildAssetBundleOptions.None, BuildTarget.WebGL);
	}

	[MenuItem("Assets/AssetBundles/Build Standalone/Windows_x86")]
	static void BuildWinx86AssetBundles(){
		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles"))
			AssetDatabase.CreateFolder("Assets", "AssetBundles");

		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone"))
			AssetDatabase.CreateFolder("Assets/AssetBundles","Standalone");

		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone/Win_x86"))
			AssetDatabase.CreateFolder("Assets/AssetBundles/Standalone", "Win_x86");

		BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Standalone/Win_x86", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}

	[MenuItem("Assets/AssetBundles/Build Standalone/OSX")]
	static void BuildOSXAssetBundles(){
		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles"))
			AssetDatabase.CreateFolder("Assets", "AssetBundles");

		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone"))
			AssetDatabase.CreateFolder("Assets/AssetBundles","Standalone");

		if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone/OSX"))
			AssetDatabase.CreateFolder("Assets/AssetBundles/Standalone", "OSX");

		BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Standalone/OSX", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
	}

	[MenuItem("Assets/AssetBundles/Get AssetBundle Names")]
	static void GetBundleNames(){
		string[] names = AssetDatabase.GetAllAssetBundleNames();

		foreach(string name in names)
			Debug.Log("AssetBundle: " + name);
	}

	[MenuItem("Assets/AssetBundles/Get Bundle Contents")]
	static void GetBundleContents(){
		string[] names = AssetDatabase.GetAllAssetBundleNames();

		foreach(string name in names){
			string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(name);
			Debug.Log("AssetBundle: " + name);
			foreach(string path in paths)
				Debug.Log("Asset Path: " + path);
		}
	}
	#endif

	#endregion
	#region Bundle Management

	//Loads the main bundle manifest and uses it to download and cache any updated
	//or non-cached bundles (queues and loads any dependencies first)
	public IEnumerator LoadBundleManifest(string url){
		using(WWW www = new WWW(url)){
			yield return www;

			if(www.error != null){
				Debug.LogError("AssetBundleManifest Error: " + www.error);
				yield break;
			}

			manifest = www.assetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
			www.assetBundle.Unload(false);

			string[] bundleNames = manifest.GetAllAssetBundles();
			Queue<string> bundleQueue = new Queue<string>();

			//Iterate through bundles and get all dependencies
			//Add them to the Queue first if not already there
			foreach(string name in bundleNames){
				string[] dependencies = manifest.GetAllDependencies(name);
				if(dependencies.Length > 0){
					foreach(string dependent in dependencies){
						if(!bundleQueue.Contains(dependent))
							bundleQueue.Enqueue(dependent);
					}
				}

				bundleQueue.Enqueue(name);
			}

			string bundlePath = GetBundlePath();

			totalBundles = (float)bundleQueue.Count;
			downloadProgressArray = new float[(int)totalBundles];
			int bundleIndex = 0;

			//Download and cache all bundles in Queue and immediately unload them from memory
			//except for "gui" and "levels" bundles
			while(bundleQueue.Count > 0){
				yield return StartCoroutine(LoadAssetBundle(bundlePath, bundleQueue.Dequeue(), bundleIndex));
				bundleIndex++;
			}
		}
	}

	//This version is solely used on initial startup to load updated/non-cached bundles
	//Download the bundle or load from cache
	//Uses bundleIndex during Splash Screen for the loading bar
	IEnumerator LoadAssetBundle(string path, string bundleName, int bundleIndex){
		while(!Caching.ready)
			yield return null;

		using(WWW www = WWW.LoadFromCacheOrDownload(path + bundleName, manifest.GetAssetBundleHash(bundleName))){
			if(!Caching.IsVersionCached(path + bundleName, manifest.GetAssetBundleHash(bundleName))){
				while(!www.isDone && downloadProgressArray != null){
					downloadProgressArray[bundleIndex] = www.progress/totalBundles;
					yield return null;
				}

				if(www.error != null){
					Debug.LogError("AssetBundle Error: " + www.error);
					yield break;
				}
			} else{
				if(downloadProgressArray != null)
					downloadProgressArray[bundleIndex] = 1f/totalBundles;
			}

			if(bundleName == "levels")
				scenePaths = www.assetBundle.GetAllScenePaths();

			if(bundleName != "levels" && bundleName != "gui")
				www.assetBundle.Unload(false);
			else
				assetBundleDict.Add(bundleName, www.assetBundle);
		}

		yield break;
	}

	//Same as LoadAssetBundle except with no bundleIndex
	IEnumerator LoadAssetBundle(string path, string bundleName){
		while(!Caching.ready)
			yield return null;

		using(WWW www = WWW.LoadFromCacheOrDownload(path + bundleName, manifest.GetAssetBundleHash(bundleName))){
			if(!Caching.IsVersionCached(path + bundleName, manifest.GetAssetBundleHash(bundleName))){
				while(!www.isDone)
					yield return null;

				if(www.error != null){
					Debug.LogError("AssetBundle Error: " + www.error);
					yield break;
				}
			}

			if(bundleName == "levels")
				scenePaths = www.assetBundle.GetAllScenePaths();

			if(!assetBundleDict.ContainsValue(www.assetBundle))
				assetBundleDict.Add(bundleName, www.assetBundle);
		}

		yield break;
	}

	//Unloads unused bundles
	public void UnloadUnusedBundles(){
		AssetBundle curBundle;

		List<string> keys = new List<string>(assetBundleDict.Keys);

		foreach(string key in keys){
			assetBundleDict.TryGetValue(key, out curBundle);
			curBundle.Unload(false);
			assetBundleDict.Remove(key);
		}
	}

	#endregion
	#region Asset Management

	//Loads GUI assets
	public T LoadGUIAsset<T>(string assetName) where T : Object{
		assetName = assetName.ToLower();
		Object asset = default(T);
		string bundleName = "gui";

		if(!assetBundleDict.ContainsKey(bundleName)){
			string bundlePath = GetBundlePath();
			StartCoroutine(LoadAssetBundle(bundlePath, bundleName));
			while(!assetBundleDict.ContainsKey(bundleName)){}
		}

		AssetBundle curBundle;
		assetBundleDict.TryGetValue(bundleName, out curBundle);
		asset = (T)curBundle.LoadAsset(assetName);

		return (T)asset;
	}

	//Loads non-GUI assets
	public T LoadAsset<T>(string assetName) where T : Object{
		assetName = assetName.ToLower();
		Object asset = default(T);
		string bundleName = "";

		if(typeof(T) == typeof(Texture2D)){
			if(PrefsManager.GetTextureRes() == 0)
				bundleName = "textures.sd";
			else if(PrefsManager.GetTextureRes() == 1)
				bundleName = "textures.hd";
			else
				bundleName = "textures.ud";
		} else if(typeof(T) == typeof(AudioClip))
			bundleName = "audio";

		if(!assetBundleDict.ContainsKey(bundleName)){
			string bundlePath = GetBundlePath();
			StartCoroutine(LoadAssetBundle(bundlePath, bundleName));
			while(!assetBundleDict.ContainsKey(bundleName)){}
		}

		AssetBundle curBundle;
		assetBundleDict.TryGetValue(bundleName, out curBundle);
		asset = (T)curBundle.LoadAsset(assetName);

		return (T)asset;
	}

	//Returns true if the given Scene has an existing path
	public bool GetScenePath(string requestedScene){
		foreach(string path in scenePaths){
			if(path.Contains(requestedScene)){
				if(!assetBundleDict.ContainsKey("levels")){
					string bundlePath = GetBundlePath();
					StartCoroutine(LoadAssetBundle(bundlePath, "levels"));
					while(!assetBundleDict.ContainsKey("levels")){}
				}

				return true;
			}
		}

		Debug.LogError("Requested Scene (" + requestedScene + ") Not Found!");
		return false;
	}

	#endregion
	#region Utility

	//Returns the path for bundles based on platform
	string GetBundlePath(){
		string pathPrefix = "";
		if(useLocalBundles)
			pathPrefix = "file://" + Application.dataPath + "/AssetBundles/";
		else
			pathPrefix = mainAssetBundleURL;

		string pathSuffix = "";
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		pathSuffix = "Standalone/Win_x86/";
		#elif UNITY_STANDALONE_OSX
		pathSuffix = "Standalone/OSX/";
		#elif UNITY_WEBGL
		pathSuffix = "WebGL/";
		#endif

		return pathPrefix + pathSuffix;
	}

	#endregion
}
