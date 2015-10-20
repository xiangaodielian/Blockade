﻿using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public bool autoPlay = false;
	
	private Ball ball;
	
	void Start(){
		ball = GameObject.FindObjectOfType<Ball>();
	}
	
	void Update(){
		if(!autoPlay){
			MoveWithMouse();
		} else {
			AutoPlay();
		}
	}
	
	void MoveWithMouse(){
		Vector3 paddlePos = new Vector3(0.5f,this.transform.position.y,0f);
		float mousePosInBlocks = Input.mousePosition.x / Screen.width * 16;
		paddlePos.x = Mathf.Clamp(mousePosInBlocks, 0.75f,15.25f);
		this.transform.position = paddlePos;
	}
	
	void AutoPlay(){
		Vector3 paddlePos = new Vector3(0.5f,this.transform.position.y,0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp(ballPos.x, 0.5f,15.5f);
		this.transform.position = paddlePos;
	}
}
