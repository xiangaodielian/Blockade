/*-------------------------------------/
  AssetBundleManager Class - Universal
  Manages the loading/caching of
  AssetBundles from remote and local
  storage
  Writen by Joe Arthur
  Latest Revision - 4 Apr, 2016
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

	private AssetBundleManifest manifest;
	private Dictionary<string, AssetBundle> assetBundleDict;
	private float totalBundles = 0f;
	private float[] downloadProgressArray = null;
	private string[] scenePaths = null;

	#endregion
	#region Mono Functions

	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		assetBundleDict = new Dictionary<string, AssetBundle>();
	}

	void Update(){
		totalDownloadProgress = 0f;
		if(downloadProgressArray != null){
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

			string pathPrefix = "";
			if(useLocalBundles)
				pathPrefix = "file://" + Application.dataPath + "/AssetBundles/";
			else
				pathPrefix = mainAssetBundleURL;

			string pathSuffix = "";
			#if UNITY_STANDALONE || UNITY_EDITOR
			pathSuffix = "Standalone/Win_x86/";
			#elif UNITY_WEBGL
			pathSuffix = "WebGL/";
			#endif

			totalBundles = (float)bundleQueue.Count;
			downloadProgressArray = new float[(int)totalBundles];
			int bundleIndex = 0;

			while(bundleQueue.Count > 0){
				StartCoroutine(LoadAssetBundle(pathPrefix + pathSuffix, bundleQueue.Dequeue(), bundleIndex));
				bundleIndex++;
			}
		}
	}

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
			} else
				downloadProgressArray[bundleIndex] = 1f/totalBundles;

			assetBundleDict.Add(bundleName, www.assetBundle);
			if(bundleName == "levels")
				scenePaths = www.assetBundle.GetAllScenePaths();
		}
	}

	#endregion
	#region Asset Management

	public T LoadGUIAsset<T>(string assetName) where T : Object{
		assetName = assetName.ToLower();
		Object asset = default(T);
		string bundleName = "gui";

		AssetBundle curBundle;
		assetBundleDict.TryGetValue(bundleName, out curBundle);
		asset = (T)curBundle.LoadAsset(assetName);

		return (T)asset;
	}

	public T LoadAsset<T>(string assetName) where T : Object{
		assetName = assetName.ToLower();
		Object asset = default(T);
		string bundleName = "";

		if(typeof(T) == typeof(Texture2D))
			bundleName = "textures";
		else if(typeof(T) == typeof(AudioClip))
			bundleName = "audio";

		if(!assetBundleDict.ContainsKey(bundleName)){
			Debug.LogError("Asset Loading Error: " + assetName + " cannot be found!");
			return default(T);
		}

		AssetBundle curBundle;
		assetBundleDict.TryGetValue(bundleName, out curBundle);
		asset = (T)curBundle.LoadAsset(assetName);

		return (T)asset;
	}

	public bool GetScenePath(string requestedScene){
		foreach(string path in scenePaths){
			if(path.Contains(requestedScene))
				return true;
		}

		Debug.LogError("Requested Scene (" + requestedScene + ") Not Found!");
		return false;
	}

	#endregion
}
