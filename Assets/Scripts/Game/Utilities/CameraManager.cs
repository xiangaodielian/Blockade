/*-----------------------------------------/
  CameraManager Class - Blockade
  Controlling class for all Camera related
  functions and animations
  Writen by Joe Arthur
  Latest Revision - 27 Mar, 2016
/-----------------------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {

	//Singleton Instance of CameraManager
	public static CameraManager instance {get; private set;}

	[Tooltip("Position if Camera has a 16:9 aspect.")]
	[SerializeField] private Vector3 sixteenNinePos = new Vector3();
	[Tooltip("Field of View if Camera has a 16:9 aspect.")]
	[SerializeField] private float sixteenNineFOV = 65f;
	[Tooltip("Position if Camera has a 16:10 aspect.")]
	[SerializeField] private Vector3 sixteenTenPos = new Vector3();
	[Tooltip("Field of View if Camera has a 16:10 aspect.")]
	[SerializeField] private float sixteenTenFOV = 70f;

	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
	}

	void Start(){
		Camera cam = GetComponent<Camera>();

		if(Mathf.Approximately(cam.aspect, 1.778055f)){
			this.transform.position = sixteenNinePos;
			cam.fieldOfView = sixteenNineFOV;
		} else if(Mathf.Approximately(cam.aspect, 1.599752f)){
			this.transform.position = sixteenTenPos;
			cam.fieldOfView = sixteenTenFOV;
		}
	}
}
