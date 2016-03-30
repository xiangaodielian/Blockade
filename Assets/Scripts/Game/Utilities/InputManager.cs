/*-----------------------------------/
  InputManager Class - Blockade
  Manages all Input including
  Player Movement and Button Presses
  Writen by Joe Arthur
  Latest Revision - 18 Mar, 2016
/-----------------------------------*/

using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	#region Variables

	//Singleton Instance of InputManager
	public static InputManager instance {get; private set;}

	#endregion
	#region Mono Functions

	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape) && GameMaster.instance.inGame)
			UIManager.instance.ToggleInGameMenu();

		if(Input.GetKeyDown(KeyCode.F12))
			UIManager.instance.ToggleDebugConsole();

		if(!GameMaster.instance.gamePaused && GameMaster.instance.allowStart){
			if(Paddle.instance){
			#if UNITY_STANDALONE
				MoveWithMouse();
			#elif UNITY_IOS || UNITY_ANDROID
				MoveWithTouch();
			#elif UNITY_WSA  || UNITY_WEBGL
			if(Input.mousePresent)
				MoveWithMouse();
			else
				MoveWithTouch();
			#endif
			}

			// Control Laser Firing
			if(Paddle.instance.hasLasers){
				if(Input.GetMouseButtonDown(0))
					Paddle.instance.FireLasers();
			}
		}
	}

	#endregion
	#region Utility Functions

	//Control Paddle motion using mouse
	void MoveWithMouse(){
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10f;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		
		Paddle.instance.MovePaddle(mousePos);
	}

	//Control Paddle motion using finger on a touch device
	void MoveWithTouch(){
		if(Input.touchCount > 0){
			Vector3 touchPos = Input.GetTouch(0).position;
			touchPos.z = 0f;
			touchPos = Camera.main.ScreenToWorldPoint(touchPos);

			Paddle.instance.MovePaddle(touchPos);
		}
	}

	public void ProcessInputString(string input){
		UIManager.instance.SendDebugCommand(input);
	}

	#endregion
}
