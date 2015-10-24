using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour {

	private LevelManager levelManager;
	
	void Start(){
		levelManager = GameObject.FindObjectOfType<LevelManager>();
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Ball[] ballArray = FindObjectsOfType<Ball>();
		
		if(collider.tag == "Ball")
			if(ballArray.Length == 1)
				levelManager.LoadLevel("Lose");
			else
				Destroy(collider.gameObject);
	}
}
