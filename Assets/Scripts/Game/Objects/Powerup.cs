using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody2D))]
public class Powerup : MonoBehaviour {

	public enum PowerupType 		{SpeedUp, SlowDown, Expand, Shrink, Lasers, Explode, 
							 		 Multiball, SafetyNet, IronBall, FeatherBall, StickyBall, Mirror};
	public PowerupType powerupType;
	
	[SerializeField] private Sprite[] spriteArray = new Sprite[12];
	[SerializeField] private AudioClip[] audioClips = new AudioClip[3];
	
	private GameMaster gameMaster;
	private Sprite curSprite = null;
	private Paddle player;
	private int pointValue = 25;
	private AudioClip curAudioClip = null;
	
	void Start(){
		gameMaster = (GameMaster)FindObjectOfType<GameMaster>();
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
				break;
			case PowerupType.Explode:
				curSprite = spriteArray [5];
				break;
			case PowerupType.Multiball:
				curSprite = spriteArray [6];
				break;
			case PowerupType.SafetyNet:
				curSprite = spriteArray [7];
				break;
			case PowerupType.IronBall:
				curSprite = spriteArray [8];
				break;
			case PowerupType.FeatherBall:
				curSprite = spriteArray [9];
				break;
			case PowerupType.StickyBall:
				curSprite = spriteArray [10];
				break;
			case PowerupType.Mirror:
				curSprite = spriteArray [11];
				break;
			default:
				Debug.LogError ("No Type Chosen For " + gameObject);
				break;
		}
		
		GetComponent<SpriteRenderer> ().sprite = curSprite;
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			if(curAudioClip != null)
				AudioSource.PlayClipAtPoint(curAudioClip,player.transform.position,PrefsManager.GetMasterSFXVolume());
			player.CollectPowerup(powerupType);
			Destroy(gameObject);
			gameMaster.totalScore += pointValue;
		}
			
	}
}