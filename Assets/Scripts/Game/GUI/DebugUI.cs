﻿/*-----------------------------------/
  DebugUI Class - Blockade
  Manages Debug UI including console,
  cheats, and debug info display
  Writen by Joe Arthur
  Latest Revision - 24 Mar, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {

	#region Debug Commands

	const string UNLOCK_ALL = "thisisgonbebig";
	const string MAX_LIVES = "luftballoons";
	const string ADD_ZAZZ = "itneedsmorezazz";
	const string MULTIBALL = "fishandloaves";
	const string DEBUG_INFO = "bigbrother";
	const string INTERVIEWER_MODE = "imaskingthequestions";

	#endregion
	#region Variables

	[System.Serializable] private class DebugFields{
		public InputField debugInput = null;
		public Text numBallsText = null;
		public Text ballSpeedText = null;
		public Text numBricksText = null;
		public Text responseText = null;
	}

	[SerializeField] private DebugFields debugFields = null;
	[SerializeField] private GameObject debugInfo = null;

	private InputField debugInput;
	//private Text debugFields.responseText;

	#endregion
	#region Mono Functions

	void Start(){
		debugInput = debugFields.debugInput;
		debugInput.onEndEdit.AddListener((value) => InputManager.instance.ProcessInputString(value));
		debugInput.gameObject.SetActive(false);
		debugFields.responseText.text = "";
		debugInfo.SetActive(false);
	}

	void Update(){
		if(debugInfo.activeSelf)
			UpdateDebugInfo();

		if(debugInput.gameObject.activeSelf){
			if(GameMaster.instance.inGame && !GameMaster.instance.gamePaused)
				GameMaster.instance.GamePause();
		}

		if(!debugInput.isFocused)
			SetFocus();
	}

	#endregion
	#region Utility Functions

	//Toggle InputField Debug Console
	public void ToggleDebugConsole(){
		debugInput.gameObject.SetActive(!debugInput.gameObject.activeSelf);
		if(debugInput.gameObject.activeSelf){
			if(GameMaster.instance.inGame && !GameMaster.instance.gamePaused)
				GameMaster.instance.GamePause();
			SetFocus();
		} else{
			if(GameMaster.instance.inGame && GameMaster.instance.gamePaused)
				GameMaster.instance.GamePause();
		}
	}

	//Updates Debug Info if Active
	void UpdateDebugInfo(){
		Ball[] balls = GameObject.FindObjectsOfType<Ball>();
		float ballSpeed = 0;
		debugFields.numBallsText.text = "Num Active Balls: " + balls.Length;
		if(balls.Length > 0)
			ballSpeed = balls[0].rigidBody.velocity.magnitude;
		debugFields.ballSpeedText.text = "Ball Velocity (m/s): " + ballSpeed.ToString("F2");

		GameObject[] bricks = GameObject.FindGameObjectsWithTag("Breakable");
		debugFields.numBricksText.text = "Num Active Bricks: " + bricks.Length;

		ShowAllPowerups(true);
	}

	//Keep Focus on InputField while Active
	public void SetFocus(){
		debugInput.Select();
		debugInput.ActivateInputField();
	}

	//Process string to see if it matches any Debug Commands
	public void ProcessCommand(string cmd){
		switch(cmd.ToLower()){
			case DEBUG_INFO:
				if(!debugInfo.activeSelf){
					debugFields.responseText.text = "INFORMATION SYSTEMS ONLINE!";
					debugInfo.SetActive(true);
					ShowAllPowerups(true);
				} else{
					debugFields.responseText.text = "INFORMATION SYSTEMS OFFLINE!";
					debugInfo.SetActive(false);
					ShowAllPowerups(false);
				}
				break;

			case UNLOCK_ALL:
				debugFields.responseText.text = "ALL LEVELS UNLOCKED!";
				PrefsManager.SetLevelUnlocked(20);
				break;

			case MAX_LIVES:
				if(GameMaster.instance.inGame){
					debugFields.responseText.text = "MAX LIVES!";
					GameMaster.instance.gameValues.playerLives = 99;
				} else
					debugFields.responseText.text = "TRY THIS ONE IN GAME!";
				break;

			case ADD_ZAZZ:
				debugFields.responseText.text = "ADDING MORE ZAZZ...";
				break;

			case MULTIBALL:
				if(GameMaster.instance.inGame){
					debugFields.responseText.text = "MULTIBALL MAYHEM!";
					AddMultiball();
				} else
					debugFields.responseText.text = "TRY THIS ONE IN GAME!";
				break;

			default:
				debugFields.responseText.text = "";
				break;
		}

		debugInput.text = "";
	}

	//Makes all Bricks contain Multiball Powerup
	void AddMultiball(){
		Brick[] bricks = GameObject.FindObjectsOfType<Brick>();

		foreach(Brick b in bricks){
			if(b.tag == "Breakable")
				b.SetPowerup(Powerup.PowerupType.Multiball);
		}
	}

	void ShowAllPowerups(bool show){
		Brick[] bricks = GameObject.FindObjectsOfType<Brick>();

		foreach(Brick brick in bricks)
			brick.ShowPowerup(show);
	}

	#endregion
}
