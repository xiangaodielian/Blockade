/*-----------------------------------/
  LoadingRotate Class - Blockade
  Controls rotation of Splash Image
  during game loading
  Writen by Joe Arthur
  Latest Revision - 4 May, 2016
/----------------------------------*/

using UnityEngine;

public class LoadingRotate : MonoBehaviour {
	
	[SerializeField] private float rotationSpeed = 5f;
	[SerializeField] private Vector3 rotationDirection = Vector3.zero;

	void Update(){
		RotateImage();
	}

	void RotateImage(){
		transform.Rotate(rotationSpeed * rotationDirection * Time.deltaTime);
	}
}
