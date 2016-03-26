/*----------------------------/
  Laser Class - Blockade
  Controlling class for Laser
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 4 Mar, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Laser : MonoBehaviour {
	
	private Rigidbody rigidBody;
	
	void Start(){
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.velocity = new Vector3(0f,10f,0f);
	}
	
	void OnCollisionEnter(Collision col){
		if(col.transform.tag != "Player")
			Destroy(gameObject);
	}
}
