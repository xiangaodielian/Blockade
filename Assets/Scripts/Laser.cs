using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Laser : MonoBehaviour {
	
	private Rigidbody2D rigidBody;
	
	void Start(){
		rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.velocity = new Vector2(0f,10f);
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if(col.transform.tag != "Player")
			Destroy(gameObject);
	}
}
