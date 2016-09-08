/*----------------------------/
  Powerup Class - Blockade
  Controlling class for Powerup
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 29 Mar, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Powerup : MonoBehaviour {

	#region Variables
	
	public enum PowerupType 		{SpeedUp, SlowDown, Expand, Shrink, Lasers, Explode, 
							 		 Multiball, IronBall, StickyBall, FeatherBall, Mirror};
	public PowerupType powerupType;
	
	[Tooltip("Array of Interior meshes.")]
	[SerializeField] private GameObject[] meshArray = new GameObject[11];
	[Tooltip("Array of Audio Clips corresponding to Powerup Interiors.")]
	[SerializeField] private string[] audioClips = new string[10];
	
	private GameObject curMesh = null;
	private Paddle player;
	private int pointValue = 25;
	private AudioClip curAudioClip = null;
	
	#endregion
	#region MonoDevelop Functions
	
	void Start(){
		transform.localEulerAngles = new Vector3(0f,180f,0f);
		player = (Paddle)FindObjectOfType<Paddle>();
		#if UNITY_IOS || UNITY_ANDROID || UNITY_WSA
		transform.localScale = new Vector3(0.75f,0.75f,0.75f);
		#endif
		foreach(GameObject go in meshArray)
			go.SetActive(false);
		
		SetPowerup();
	}
	
	#if UNITY_EDITOR
	void Update(){
		SetPowerup();
	}
	#endif
	
	#endregion
	#region Utility

	//Set the Sprite and AudioClip for selected Powerup Type
	void SetPowerup()
	{
		if(curMesh)
			curMesh.SetActive(false);
		
		switch (powerupType) {
			case PowerupType.SpeedUp:
				curMesh = meshArray[0];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[0]);
				break;
			case PowerupType.SlowDown:
				curMesh = meshArray[1];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[1]);
				break;
			case PowerupType.Expand:
				curMesh = meshArray[2];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[2]);
				break;
			case PowerupType.Shrink:
				curMesh = meshArray[3];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[2]);
				break;
			case PowerupType.Lasers:
				curMesh = meshArray[4];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[3]);
				break;
			case PowerupType.Explode:
				curMesh = meshArray[5];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[4]);
				break;
			case PowerupType.Multiball:
				curMesh = meshArray[6];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[5]);
				break;
			case PowerupType.IronBall:
				curMesh = meshArray[7];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[6]);
				break;
			case PowerupType.FeatherBall:
				curMesh = meshArray[8];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[7]);
				break;
			case PowerupType.StickyBall:
				curMesh = meshArray[9];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[8]);
				break;
			case PowerupType.Mirror:
				curMesh = meshArray[10];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[9]);
				break;
			default:
				Debug.LogError ("No Type Chosen For " + gameObject);
				break;
		}
		
		ResourceManager.SetMaterialTextures(curMesh.gameObject);
		curMesh.SetActive(true);
	}
	
	//Powerup Collected
	void OnTriggerEnter(Collider col){
		if(col.tag == "Player"){
			if(curAudioClip != null)
				AudioSource.PlayClipAtPoint(curAudioClip,player.transform.position,PrefsManager.GetMasterSFXVolume());
			player.CollectPowerup(powerupType);
			Destroy(gameObject);
			GameMaster.GMInstance.playerManager.AddToPlayerScore(pointValue);
		}
			
	}
	
	#endregion
}