/*-----------------------------------/
  InputManager Class - Blockade
  Manages all Input including
  Player Movement and Button Presses
  Writen by Joe Arthur
  Latest Revision - 11 Apr, 2016
/-----------------------------------*/

using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	#region Variables

	//Singleton Instance of InputManager
	public static InputManager instance {get; private set;}
	public bool useCursorMovement = false;		//Defaults to movement using Keyboard

	#endregion
	#region Mono Functions

	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		useCursorMovement = PrefsManager.GetMouseControl();
	}

	void Update(){
		//InGame Menu
		if(Input.GetButtonDown("InGameMenu") && GameMaster.instance.inGame)
			UIManager.instance.ToggleInGameMenu();

		//Debug Console
		if(Input.GetButtonDown("DebugConsole"))
			UIManager.instance.ToggleDebugConsole();

		//Movement
		if(!GameMaster.instance.gamePaused && GameMaster.instance.allowStart){
			if(Paddle.instance){
				if(useCursorMovement){
					if(Input.mousePresent)
						MoveWithMouse();
					else
						MoveWithTouch();
				} else
					MoveWithKeyboard();
			}

			// Control Laser Firing
			if(Paddle.instance.hasLasers){
				if(Input.GetButtonDown("Fire"))
					Paddle.instance.FireLasers();
			}
		}
	}

	#endregion
	#region Utility Functions

	//Control Paddle motion using mouse
	void MoveWithMouse(){
		Vector3 newPos = Paddle.instance.transform.position;
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10f;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		if(Paddle.instance.mirroredMovement)
			newPos.x += 16f - mousePos.x - newPos.x;
		else
			newPos.x += mousePos.x - newPos.x;
		
		Paddle.instance.MovePaddle(newPos);

		if(Input.GetMouseButtonDown(0))
			Paddle.instance.LaunchBall();
	}

	//Control Paddle motion using finger on a touch device
	void MoveWithTouch(){
		if(Input.touchCount > 0){
			Vector3 touchPos = Input.GetTouch(0).position;
			touchPos.z = 10f;
			touchPos = Camera.main.ScreenToWorldPoint(touchPos);

			Paddle.instance.MovePaddle(touchPos);
		}
	}

	//Control Paddle motion using Keyboard (A,D,<-,-> for motion, hold Shift for dash)
	void MoveWithKeyboard(){
		Vector3 newPos = Paddle.instance.transform.position;
		float movementMultiplier = 1f;

		if(Input.GetButton("Dash"))
			movementMultiplier = 3f;

		if(Input.GetAxis("Horizontal") < 0f){
			if(Paddle.instance.mirroredMovement)
				newPos.x += 0.15f * movementMultiplier;
			else
				newPos.x -= 0.15f * movementMultiplier;
		}

		if(Input.GetAxis("Horizontal") > 0f){
			if(Paddle.instance.mirroredMovement)
				newPos.x -= 0.15f * movementMultiplier;
			else
				newPos.x += 0.15f * movementMultiplier;
		}

		Paddle.instance.MovePaddle(newPos);

		if(Input.GetButtonDown("Launch"))
			Paddle.instance.LaunchBall();
	}

	public void ProcessInputString(string input){
		UIManager.instance.SendDebugCommand(input);
	}

	#endregion
}
