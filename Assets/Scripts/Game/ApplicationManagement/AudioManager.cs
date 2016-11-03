using UnityEngine;
using ApplicationManagement;

public class AudioManager : MonoBehaviour {

    #region Variables

    public static AudioManager Instance { get; private set; }

    [SerializeField] private GameObject musicPlayerPrefab = null;

    private MusicPlayer musicPlayer;

    #endregion
    #region Mono Functions

    private void Awake() {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;
    }

	private void Start () {
	    InitMusicPlayer();
	}

    private void OnEnable() {
        EventManager.Instance.AddListener<LevelManager.SceneChangeEvent>(OnSceneChange);
    }

    private void OnDisable() {
        EventManager.Instance.RemoveListener<LevelManager.SceneChangeEvent>(OnSceneChange);
    }

    #endregion

    private void InitMusicPlayer() {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        if(musicPlayer != null)
            Destroy(musicPlayer.gameObject);

        musicPlayer = Instantiate(musicPlayerPrefab).GetComponent<MusicPlayer>();
        musicPlayer.transform.SetParent(transform);
    }

    public void SetMusicPlayerVolume(float volume) {
        musicPlayer.SetVolume(volume);
    }

    public void NextMusicTrack() {
        musicPlayer.NextTrack();
    }

    #region Delegate Listeners

    private void OnSceneChange(LevelManager.SceneChangeEvent e) {
        if(e.scene.name != "MainMenu") return;

        if(musicPlayer.isPlaying)
            musicPlayer.StopMusic();

        musicPlayer.MenuMusicSet();
        musicPlayer.StartMusic();
    }

    #endregion
}
