using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	#region Variables

	[System.Serializable] private class Prefabs{
		public GameObject paddlePrefab = null;
	}

	[System.Serializable] private class ScoringValues{
		public int playerLives = 3;
		public int playerScore = 0;
	}

	[HideInInspector] public Paddle activePlayer = null;

	[SerializeField] private Prefabs prefabs = null;
	[SerializeField] private ScoringValues scoringValues = null;

	private int lifeGrantedAt = 0;

	#endregion
	#region Mono Functions

	void Update(){
		if(scoringValues.playerScore - lifeGrantedAt > 5000){
			if(scoringValues.playerLives < 99)
				scoringValues.playerLives++;

			lifeGrantedAt = scoringValues.playerScore;
		}
	}

	#endregion
	#region Player Management

	public void SpawnPlayer(){
		if(!activePlayer){
			activePlayer = Instantiate(prefabs.paddlePrefab).GetComponent<Paddle>();
			activePlayer.ResetToDefaultState();
		}
	}

	public void DestroyPlayer(){
		if(activePlayer != null){
			Destroy(activePlayer.gameObject);
			activePlayer = null;
		}
	}

	#endregion
	#region ScoringValues Management

	/// <summary>
	/// Adds value to player lives.
	/// </summary>
	public void AddToPlayerLives(int value){
		scoringValues.playerLives += value;
	}

	/// <summary>
	/// Sets the player lives to numLives.
	/// </summary>
	public void SetPlayerLives(int numLives){
		scoringValues.playerLives = numLives;
	}

	/// <summary>
	/// Gets the player lives.
	/// </summary>
	/// <returns>The player lives.</returns>
	public int GetPlayerLives(){
		return scoringValues.playerLives;
	}

	/// <summary>
	/// Adds value to player score.
	/// </summary>
	public void AddToPlayerScore(int value){
		scoringValues.playerScore += value;
	}

	/// <summary>
	/// Sets the player score to value.
	/// </summary>
	public void SetPlayerScore(int value){
		scoringValues.playerScore = value;
	}

	/// <summary>
	/// Gets the player score.
	/// </summary>
	/// <returns>The player score.</returns>
	public int GetPlayerScore(){
		return scoringValues.playerScore;
	}

	#endregion
	#region Delegates

	public void SceneChange(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode){
		if(scene.name == "MainMenu" || scene.name == "EndGame")
			SetPlayerLives(3);
		else if(scene.name != "Splash")
			SpawnPlayer();
	}

	#endregion
}
