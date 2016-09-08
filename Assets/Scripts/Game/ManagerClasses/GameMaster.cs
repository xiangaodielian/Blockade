using UnityEngine;

[RequireComponent(typeof(EventManager))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(GameObjectManager))]
[RequireComponent(typeof(TimeManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(OptionsManager))]
[RequireComponent(typeof(AssetBundleManager))]
public class GameMaster : MonoBehaviour {
	
	#region variables
	
	//Singleton Instance of GameMaster
	public static GameMaster GMInstance {get; private set;}

	[System.Serializable] private class Prefabs{
		public GameObject splashPrefab = null;
		public GameObject musicPlayerPrefab = null;
		public GameObject cameraPrefab = null;
		public GameObject guiManagerPrefab = null;
	}

	/*-----MANAGERS-----*/
	public InputManager inputManager {get; private set;}
	public PlayerManager playerManager {get; private set;}
	public GameObjectManager gameObjectManager {get; private set;}
	public OptionsManager optionsManager {get; private set;}
	public AssetBundleManager abManager {get; private set;}
	/*------------------*/

	[SerializeField] private Prefabs prefabs = null;
	
	#endregion
	#region Mono Functions
	
	void Awake(){
		if(GMInstance != null && GMInstance != this)
			Destroy(gameObject);

		GMInstance = this;

		DontDestroyOnLoad(gameObject);

		AssignManagers();
		InstantiatePrefabs();
	}
	
	#endregion
	#region Manager Init

	void InstantiatePrefabs(){

		//Music Player
		if(MusicPlayer.instance)
			DestroyImmediate(MusicPlayer.instance.gameObject);

		Instantiate(prefabs.musicPlayerPrefab);
		MusicPlayer.instance.transform.SetParent(this.transform);

		//Camera
		if(CameraManager.instance)
			DestroyImmediate(CameraManager.instance.gameObject);

		Instantiate(prefabs.cameraPrefab);
		CameraManager.instance.transform.SetParent(this.transform);

		Instantiate(prefabs.guiManagerPrefab).GetComponent<GUIManager>();
		GUIManager.instance.transform.SetParent(this.transform);
	}

	void AssignManagers(){
		inputManager = GetComponent<InputManager>();
		playerManager = GetComponent<PlayerManager>();
		gameObjectManager = GetComponent<GameObjectManager>();
		optionsManager = GetComponent<OptionsManager>();
		abManager = GetComponent<AssetBundleManager>();
	}

	#endregion
}