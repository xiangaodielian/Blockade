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
	[SerializeField] private GameObject powerupPrefab;
	
	private Powerup powerup;
	private Sprite curSprite = null;
	private int hitPoints = 0;
	private int timesHit;
	private LevelManager levelManager;
	private bool isBreakable = true;
	
	void Start() {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		timesHit = 0;
		SetBrick();
		
		if(isBreakable)
			levelManager.breakableCount++;
	}
	
	#if UNITY_EDITOR
	void Update(){
		if(!EditorApplication.isPlaying){
			SetBrick();
			Debug.Log("MOO!");
		}
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
		
		GetComponent<SpriteRenderer>().sprite = curSprite;
		
		if(randomPowerup)
			powerupType = (Powerup.PowerupType)UnityEngine.Random.Range(0,System.Enum.GetNames(typeof(Powerup.PowerupType)).Length);
	}
	
	void ChangeBrickSprite(){
		int spriteIndex = hitPoints - (timesHit+1);
		
		if(spriteIndex < 0)
			spriteIndex = 0;
			
		GetComponent<SpriteRenderer>().sprite = brickSprites[spriteIndex];
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		if(isBreakable)
			HandleHits();
	}
	
	void HandleHits(){
		timesHit++;
		if(timesHit >= hitPoints && hitPoints > 0){
			AudioSource.PlayClipAtPoint(crack, transform.position, 0.6f);
			if(hasPowerup)
				DropPowerup();
				
			levelManager.BrickDestroyed();
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
}
