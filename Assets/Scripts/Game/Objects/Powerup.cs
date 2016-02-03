/*----------------------------/
  Powerup Class - Blockade
  Controlling class for Powerup
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody2D))]
public class Powerup : MonoBehaviour {

	public enum PowerupType 		{SpeedUp, SlowDown, Expand, Shrink, Lasers, Explode, 
							 		 Multiball, SafetyNet, IronBall, FeatherBall, StickyBall, Mirror};
	public PowerupType powerupType;
	
	[SerializeField] private Sprite[] spriteArray = new Sprite[12];
	[SerializeField] private AudioClip[] audioClips = new AudioClip[11];
	
	private Sprite curSprite = null;
	private Paddle player;
	private int pointValue = 25;
	private AudioClip curAudioClip = null;
	
	void Start(){
		player = (Paddle)FindObjectOfType<Paddle>();
		#if UNITY_IOS || UNITY_ANDROID || UNITY_WSA
		transform.localScale = new Vector3(0.75f,0.75f,1f);
		#endif
		SetPowerup();
	}
	
	#if UNITY_EDITOR
	void Update(){
		SetPowerup();
	}
	#endif

	//Set the Sprite and AudioClip for selected Powerup Type
	void SetPowerup()
	{
		switch (powerupType) {
			case PowerupType.SpeedUp:
				curSprite = spriteArray [0];
				curAudioClip = audioClips[0];
				break;
			case PowerupType.SlowDown:
				curSprite = spriteArray [1];
				curAudioClip = audioClips[1];
				break;
			case PowerupType.Expand:
				curSprite = spriteArray [2];
				curAudioClip = audioClips[2];
				break;
			case PowerupType.Shrink:
				curSprite = spriteArray [3];
				curAudioClip = audioClips[2];
				break;
			case PowerupType.Lasers:
				curSprite = spriteArray [4];
				curAudioClip = audioClips[7];
				break;
			case PowerupType.Explode:
				curSprite = spriteArray [5];
				curAudioClip = audioClips[9];
				break;
			case PowerupType.Multiball:
				curSprite = spriteArray [6];
				curAudioClip = audioClips[5];
				break;
			case PowerupType.SafetyNet:
				curSprite = spriteArray [7];
				curAudioClip = audioClips[10];
				break;
			case PowerupType.IronBall:
				curSprite = spriteArray [8];
				curAudioClip = audioClips[6];
				break;
			case PowerupType.FeatherBall:
				curSprite = spriteArray [9];
				curAudioClip = audioClips[4];
				break;
			case PowerupType.StickyBall:
				curSprite = spriteArray [10];
				curAudioClip = audioClips[3];
				break;
			case PowerupType.Mirror:
				curSprite = spriteArray [11];
				curAudioClip = audioClips[8];
				break;
			default:
				Debug.LogError ("No Type Chosen For " + gameObject);
				break;
		}
		
		GetComponent<SpriteRenderer> ().sprite = curSprite;
	}
	
	//Powerup Collected
	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			if(curAudioClip != null)
				AudioSource.PlayClipAtPoint(curAudioClip,player.transform.position,PrefsManager.GetMasterSFXVolume());
			player.CollectPowerup(powerupType);
			Destroy(gameObject);
			GameMaster.instance.totalScore += pointValue;
		}
			
	}
}