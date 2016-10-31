using UnityEngine;
using ApplicationManagement.ResourceControl;

[RequireComponent(typeof(Rigidbody))]
public class Laser : MonoBehaviour {
	
	private Rigidbody rigidBody;
	
	void Start(){
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.velocity = new Vector3(0f,10f,0f);
		ResourceManager.SetMaterialTextures(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col){
		if(col.transform.tag != "Player")
			Destroy(gameObject);
	}
}
