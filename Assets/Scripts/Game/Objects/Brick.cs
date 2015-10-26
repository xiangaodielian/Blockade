using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Brick : MonoBehaviour {

	public AudioClip crack;
	public enum BrickType {ONE, TWO, THREE, FOUR, FIVE, UNBREAKABLE};
	
	[SerializeField] private Sprite[] brickSprites = new Sprite[6];
	[SerializeField] private BrickType brickType = BrickType.ONE;
	[SerializeField] private bool hasPowerup = false;
	[SerializeField] private bool randomPowerup = true;
	[SerializeField] private Powerup.PowerupType powerupType;
	[SerializeField] private GameObject powerupPrefab = null;
	[SerializeField] private ParticleSystem explosion = null;
	
	private Powerup powerup;
	private Sprite curSprite = null;
	private int hitPoints = 0;
	private int pointValue = 0;
	private float timesHit;
	private GameMaster gameMaster;
	private bool isBreakable = true;
	private Ball collidingBall = null;
	private AudioSource audioSource;
	
	void Start() {
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();
		gameMaster = GameObject.FindObjectOfType<GameMaster>();
		timesHit = 0f;
		SetBrick();
		
		if(isBreakable)
			gameMaster.breakableCount++;
	}
	
	#if UNITY_EDITOR
	void Update(){
		if(!EditorApplication.isPlaying)
			SetBrick();
	}
	#endif

	void SetBrick()
	{
		switch (brickType) {
			case BrickType.ONE:
				curSprite = brickSprites[0];
				hitPoints = 1;
				break;
			case BrickType.TWO:
				curSprite = brickSprites[1];
				hitPoints = 2;
				break;
			case BrickType.THREE:
				curSprite = brickSprites[2];
				hitPoints = 3;
				break;
			case BrickType.FOUR:
				curSprite = brickSprites[3];
				hitPoints = 4;
				break;
			case BrickType.FIVE:
				curSprite = brickSprites[4];
				hitPoints = 5;
				break;
			case BrickType.UNBREAKABLE:
				curSprite = brickSprites[5];
				hitPoints = -1;
				isBreakable = false;
				break;
			default:
				Debug.LogError ("No Brick Type Set For " + gameObject);
				break;
		}
		
		pointValue = 50*hitPoints;
		GetComponent<SpriteRenderer>().sprite = curSprite;
		
		if(randomPowerup)
			powerupType = (Powerup.PowerupType)UnityEngine.Random.Range(0,System.Enum.GetNames(typeof(Powerup.PowerupType)).Length);
	}
	
	void ChangeBrickSprite(){
		int spriteIndex = hitPoints - ((int)timesHit+1);
		
		if(spriteIndex < 0)
			spriteIndex = 0;
			
		GetComponent<SpriteRenderer>().sprite = brickSprites[spriteIndex];
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		if(isBreakable){
			collidingBall = collision.collider.GetComponent<Ball>();
			HandleHits();
			collidingBall = null;
		}
	}
	
	void HandleHits(){
		if(collidingBall){
			if(collidingBall.isExplosive){
				Instantiate(explosion,transform.position,Quaternion.identity);
				collidingBall.BallExploded();
				
				float explosionDistanceX = 1f;
				float explosionDistanceY = 0.31f;
				Brick[] activeBricks = FindObjectsOfType<Brick>();
				foreach(Brick brick in activeBricks){
					Vector3 brickDistance = brick.transform.position - transform.position;
					brickDistance = new Vector3(Mathf.Abs(brickDistance.x),Mathf.Abs(brickDistance.y),brickDistance.z);
					if(brickDistance.x <= explosionDistanceX && brickDistance.y <= explosionDistanceY && brick.gameObject != this.gameObject)
						brick.Explode();
				}
				
				timesHit++;
			} else{
				if(collidingBall.isIron)
					timesHit += 2f;
				else if(collidingBall.isFeather)
					timesHit += 0.5f;
				else
					timesHit++;
			}
		} else{
			timesHit++;
		}
		
		if(timesHit >= hitPoints && hitPoints > 0){
			audioSource.Play();
			if(hasPowerup)
				DropPowerup();
				
			gameMaster.BrickDestroyed(pointValue);
			Destroy(gameObject);
		} else {
			ChangeBrickSprite();
		}
	}
	
	void DropPowerup(){
		powerup = powerupPrefab.GetComponent<Powerup>();
		powerup.powerupType = powerupType;
		
		Instantiate(powerupPrefab,transform.position,Quaternion.identity);
	}
	
	public void Explode(){
		if(isBreakable)
			HandleHits();
	}
}
