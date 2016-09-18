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
	public static GameMaster Instance {get; private set;}

	[System.Serializable] private class Prefabs{
		public GameObject splashPrefab = null;
		public GameObject musicPlayerPrefab = null;
		public GameObject cameraPrefab = null;
		public GameObject guiManagerPrefab = null;
	}

	/*-----MANAGERS-----*/
	public InputManager InputManager {get; private set;}
	public PlayerManager PlayerManager {get; private set;}
	public GameObjectManager GameObjectManager {get; private set;}
	public OptionsManager OptionsManager {get; private set;}
	/*------------------*/

	[SerializeField] private Prefabs prefabs = null;
	
	#endregion
	#region Mono Functions
	
	private void Awake(){
		if(Instance != null && Instance != this)
			Destroy(gameObject);

		Instance = this;

		DontDestroyOnLoad(gameObject);

		AssignManagers();
		InstantiatePrefabs();
	}
	
	#endregion
	#region Manager Init

	private void InstantiatePrefabs(){

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

	private void AssignManagers(){
		InputManager = GetComponent<InputManager>();
		PlayerManager = GetComponent<PlayerManager>();
		GameObjectManager = GetComponent<GameObjectManager>();
		OptionsManager = GetComponent<OptionsManager>();
	}

	#endregion
}