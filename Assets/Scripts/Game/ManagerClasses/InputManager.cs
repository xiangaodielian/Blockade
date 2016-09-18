using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InputManager : MonoBehaviour {

	#region Variables

	public static bool useCursorMovement = false;		//Defaults to movement using Keyboard
	public bool allowPlayerMovement = false;

	private UnityAction dissolveFinishListener;
	private UnityAction levelFisnishListener;

	#endregion
	#region Mono Functions

	void Awake(){
		dissolveFinishListener = new UnityAction(AllowPlayerMovement);
		levelFisnishListener = new UnityAction(DisallowPlayerMovement);
		useCursorMovement = PrefsManager.GetMouseControl();
	}

	void OnEnable(){
		EventManager.StartListening(EventManager.EventNames.DissolveFinish, dissolveFinishListener);
		EventManager.StartListening(EventManager.EventNames.LevelFinish, levelFisnishListener);
	}

	void onDisable(){
		EventManager.StopListening(EventManager.EventNames.DissolveFinish, dissolveFinishListener);
		EventManager.StopListening(EventManager.EventNames.LevelFinish, levelFisnishListener);
	}

	void Update(){
		//InGame Menu
		if(Input.GetKeyDown(KeyCode.Escape) && LevelManager.GetCurrentLevel().Contains("Level"))
			GUIManager.Instance.InGameGui.ToggleMenu();

		//Debug Console
		if(Input.GetKeyDown(KeyCode.F10))
			GUIManager.Instance.DebugUi.ToggleDebugConsole();

		//Movement
		if(!TimeManager.gamePaused && allowPlayerMovement){
			if(GameMaster.Instance.PlayerManager.activePlayer != null){
				if(useCursorMovement){
					if(Input.mousePresent)
						MoveWithMouse();
					else
						MoveWithTouch();
				} else
					MoveWithKeyboard();

				// Control Laser Firing
				if(GameMaster.Instance.PlayerManager.activePlayer.hasLasers){
					if(Input.GetButtonDown("Fire"))
						GameMaster.Instance.PlayerManager.activePlayer.FireLasers();
				}
			}
		}
	}

	#endregion
	#region Utility Functions

	//Control Paddle motion using mouse
	void MoveWithMouse(){
		Vector3 newPos = GameMaster.Instance.PlayerManager.activePlayer.transform.position;
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10f;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		if(GameMaster.Instance.PlayerManager.activePlayer.mirroredMovement)
			newPos.x += 16f - mousePos.x - newPos.x;
		else
			newPos.x += mousePos.x - newPos.x;
		
		GameMaster.Instance.PlayerManager.activePlayer.MovePaddle(newPos);

		if(Input.GetMouseButtonDown(0))
			EventManager.TriggerEvent(EventManager.EventNames.LaunchBall);
	}

	//Control Paddle motion using finger on a touch device
	void MoveWithTouch(){
		if(Input.touchCount > 0){
			Vector3 touchPos = Input.GetTouch(0).position;
			touchPos.z = 10f;
			touchPos = Camera.main.ScreenToWorldPoint(touchPos);

			GameMaster.Instance.PlayerManager.activePlayer.MovePaddle(touchPos);
		}
	}

	//Control Paddle motion using Keyboard (A,D,<-,-> for motion, hold Shift for dash)
	void MoveWithKeyboard(){
		Vector3 newPos = GameMaster.Instance.PlayerManager.activePlayer.transform.position;
		float movementMultiplier = 1f;

		if(Input.GetButton("Dash"))
			movementMultiplier = 3f;

		if(Input.GetAxis("Horizontal") < 0f){
			if(GameMaster.Instance.PlayerManager.activePlayer.mirroredMovement)
				newPos.x += 0.15f * movementMultiplier;
			else
				newPos.x -= 0.15f * movementMultiplier;
		}

		if(Input.GetAxis("Horizontal") > 0f){
			if(GameMaster.Instance.PlayerManager.activePlayer.mirroredMovement)
				newPos.x -= 0.15f * movementMultiplier;
			else
				newPos.x += 0.15f * movementMultiplier;
		}

		GameMaster.Instance.PlayerManager.activePlayer.MovePaddle(newPos);

		if(Input.GetButtonDown("Launch"))
			EventManager.TriggerEvent(EventManager.EventNames.LaunchBall);
	}

	public void ProcessInputString(string input){
		GUIManager.Instance.DebugUi.ProcessCommand(input);
	}

	#endregion
	#region Delegates

	void AllowPlayerMovement(){
		allowPlayerMovement = true;
	}

	void DisallowPlayerMovement(){
		allowPlayerMovement = false;
	}

	#endregion
}
