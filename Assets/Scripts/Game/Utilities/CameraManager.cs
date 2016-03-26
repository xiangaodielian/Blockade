/*-----------------------------------------/
  CameraManager Class - Blockade
  Controlling class for all Camera related
  functions and animations
  Writen by Joe Arthur
  Latest Revision - 5 Mar, 2016
/-----------------------------------------*/

using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	//Singleton Instance of CameraManager
	public static CameraManager instance {get; private set;}

	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
	}
}
