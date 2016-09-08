using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GameObjectManager : MonoBehaviour {

	#region Variables

	[System.Serializable] private class Prefabs{
		public GameObject playspacePrefab = null;
		public GameObject starsPSPrefab = null;
		public GameObject ballPrefab = null;
	}

	[SerializeField] private Prefabs prefabs = null;

	private UnityAction levelResetListener;
	private static List<GameObject> breakableBricks = new List<GameObject>();
	private static  GameObject stars = null;

	#endregion
	#region Mono Functions

	void Awake(){
		levelResetListener = new UnityAction(SpawnBall);

		stars = Instantiate(prefabs.starsPSPrefab);
		stars.transform.SetParent(this.transform);
		stars.SetActive(false);
	}

	void OnEnable(){
		EventManager.StartListening(EventManager.EventNames.levelReset, levelResetListener);
	}

	void OnDisable(){
		EventManager.StopListening(EventManager.EventNames.levelReset, levelResetListener);
	}

	#endregion
	#region Brick Management

	/// <summary>
	/// Adds the brick to list.
	/// </summary>
	public static void AddBrickToList(GameObject brick){
		if(!breakableBricks.Contains(brick))
			breakableBricks.Add(brick);
	}

	static void RemoveBrickFromList(GameObject brick){
		if(breakableBricks.Contains(brick))
			breakableBricks.Remove(brick);
	}

	/// <summary>
	/// Adds Brick score to playerScore and removes Brick from breakableBricks
	/// </summary>
	/// <param name="pointValue">Point value of the Brick.</param>
	public static void BrickDestroyed(int pointValue, GameObject brick){
		GameMaster.GMInstance.playerManager.AddToPlayerScore(pointValue);
		RemoveBrickFromList(brick);

		if(breakableBricks.Count == 0){
			GameMaster.GMInstance.playerManager.activePlayer.hasStarted = false;
			GUIManager.instance.inGameGUI.ToggleEndLevelPanel(true);
			GUIManager.instance.inGameGUI.CalculateTimeBonus();
		}
	}

	#endregion
	#region Powerup Management

	/// <summary>
	/// Populates the powerups of current bricks.
	/// </summary>
	public static void PopulatePowerups(){
		foreach(GameObject brick in breakableBricks)
			brick.GetComponent<Brick>().SetPowerup();
	}

	#endregion
	#region Utility

	/// <summary>
	/// Clears all active GameObjects.
	/// </summary>
	public static void ClearGameObjects(){
		if(PlaySpace.instance)
			Destroy(PlaySpace.instance.gameObject);

		GameMaster.GMInstance.playerManager.DestroyPlayer();

		Ball[] ballsInScene = FindObjectsOfType<Ball>();
		if(ballsInScene.Length > 0){
			foreach(Ball ball in ballsInScene)
				Destroy(ball.gameObject);
		}

		Brick[] bricksInScene = FindObjectsOfType<Brick>();
		if(bricksInScene.Length > 0){
			foreach(Brick brick in bricksInScene)
				Destroy(brick.gameObject);

			breakableBricks = new List<GameObject>();
		}

		Powerup[] powerupsInScene = FindObjectsOfType<Powerup>();
		if(powerupsInScene.Length > 0){
			foreach(Powerup powerup in powerupsInScene)
				Destroy(powerup.gameObject);
		}
	}

	public static void StartStars(){
		if(!stars.activeSelf)
			stars.SetActive(true);
	}

	void SpawnBall(){
		Instantiate(prefabs.ballPrefab);
	}

	#endregion
	#region Delegates

	public void SceneChange(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode){
		if(!scene.name.Contains("Level")){
			ClearGameObjects();
			if(scene.name == "MainMenu")
				StartStars();
		} else{
			breakableBricks = new List<GameObject>();

			Ball[] ballsInScene = FindObjectsOfType<Ball>();
			if(ballsInScene.Length > 0){
				foreach(Ball ball in ballsInScene)
					Destroy(ball.gameObject);
			}

			SpawnBall();

			if(!PlaySpace.instance){
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
