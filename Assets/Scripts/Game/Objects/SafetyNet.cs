using UnityEngine;
using System.Collections;

public class SafetyNet : MonoBehaviour {

	private Vector3 targetScale = new Vector3();
	
	void Start(){
		targetScale = transform.localScale;
		transform.localScale = new Vector3(0.01f,targetScale.y,targetScale.z);
	}
	
	void Update(){
		if(transform.localScale != targetScale)
			transform.localScale = Vector3.Lerp(transform.localScale,targetScale,3f*Time.deltaTime);
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Ball")
			Destroy(gameObject);
	}
}
