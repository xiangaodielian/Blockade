﻿/*--------------------------------/
  SafetyNet Class - Blockade
  Controlling class for SafetyNet
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/-------------------------------*/

using UnityEngine;
using System.Collections;

public class SafetyNet : MonoBehaviour {

	[SerializeField] private AudioClip netBounceClip = null;
	
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
		if(col.gameObject.tag == "Ball"){
			AudioSource.PlayClipAtPoint(netBounceClip,transform.position,PrefsManager.GetMasterSFXVolume());
			Destroy(gameObject);
		}
	}
}
