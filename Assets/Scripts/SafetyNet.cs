using UnityEngine;
using System.Collections;

public class SafetyNet : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Ball")
			Destroy(gameObject);
	}
}
