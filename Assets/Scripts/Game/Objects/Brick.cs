/*----------------------------/
  Brick Class - Blockade
  Controlling class for Brick
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 6 Apr, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Brick : MonoBehaviour {

	#region Variables

	[System.Serializable] private class MaterialArrays{
		public Color[] brickGlowColors = new Color[6];
		public float[] glowIntensities = new float[6];
	}

	[System.Serializable] private class PowerupDetails{
		public bool hasPowerup = false;
		public bool randomPowerup = false;
		public Powerup.PowerupType powerupType;
		public GameObject powerupPrefab = null;
		public GameObject explosionPrefab = null;
	}

	public enum BrickType {ONE, TWO, THREE, FOUR, FIVE, UNBREAKABLE};
	
	[SerializeField] private string[] audioClips = new string[5];
	[SerializeField] private MaterialArrays materialArrays = null;
	[SerializeField] private PowerupDetails powerupDetails = null;
	[Tooltip("Number of hits it takes to destroy the Brick.")]
	[SerializeField] private BrickType brickType = BrickType.ONE;

	private Powerup powerup = null;
	private Color curColor = Color.white;
	private float curIntensity = 1f;
	private AudioClip curAudioClip = null;
	private int hitPoints = 0;
	private int pointValue = 0;
	private float timesHit = 0;
	private bool isBreakable = false;
	private Ball collidingBall = null;
	private Material bodyMaterial = null;
	
	#endregion
	#region MonoDevelop Functions
	
	void Start() {
		#if UNITY_EDITOR
		if(!EditorApplication.isPlaying)
			bodyMaterial = GetComponent<MeshRenderer>().sharedMaterials[1];
		else
			bodyMaterial = GetComponent<MeshRenderer>().materials[1];
		#else
		bodyMaterial = GetComponent<MeshRenderer>().materials[1];
		#endif

		timesHit = 0f;
		SetBrick();
		ResourceManager.SetMaterialTextures(this.gameObject);
		
		if(isBreakable)
			GameMaster.instance.AddBrickToList(this.gameObject);
	}
	
	void Update(){
		#if UNITY_EDITOR
		if(!EditorApplication.isPlaying)
			SetBrick();
		#endif
	}
	
	#endregion
	#region Brick Utility Functions
	
	//Set initial values for Brick Sprite, Audio, and HP
	void SetBrick()
	{
		switch (brickType) {
			case BrickType.ONE:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[0];
				curIntensity = materialArrays.glowIntensities[0];
				curAudioClip = ResourceManager.LoadAudioClip(false, audioClips[0]);
				hitPoints = 1;
				break;
			case BrickType.TWO:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[1];
				curIntensity = materialArrays.glowIntensities[1];
				curAudioClip = ResourceManager.LoadAudioClip(false, audioClips[1]);
				hitPoints = 2;
				break;
			case BrickType.THREE:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[2];
				curIntensity = materialArrays.glowIntensities[2];
				curAudioClip = ResourceManager.LoadAudioClip(false, audioClips[2]);
				hitPoints = 3;
				break;
			case BrickType.FOUR:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[3];
				curIntensity = materialArrays.glowIntensities[3];
				curAudioClip = ResourceManager.LoadAudioClip(false, audioClips[3]);
				hitPoints = 4;
				break;
			case BrickType.FIVE:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[4];
				curIntensity = materialArrays.glowIntensities[4];
				curAudioClip = ResourceManager.LoadAudioClip(false, audioClips[4]);
				hitPoints = 5;
				break;
			case BrickType.UNBREAKABLE:
				curColor = materialArrays.brickGlowColors[5];
				curIntensity = materialArrays.glowIntensities[5];
				hitPoints = -1;
				break;
			default:
				Debug.LogError ("No Brick Type Set For " + gameObject);
				break;
		}

		if(gameObject.tag == "Breakable")
			isBreakable = true;
		
		pointValue = 50*hitPoints;
		bodyMaterial.SetColor("_EmissionColor", curColor*curIntensity);
		
		if(powerupDetails.randomPowerup)
			powerupDetails.powerupType = (Powerup.PowerupType)UnityEngine.Random.Range(0,System.Enum.GetNames(typeof(Powerup.PowerupType)).Length);
	}
	
	//Change Glow Color when hit
	void ChangeBrickColor(){
		int colorIndex = hitPoints - ((int)timesHit+1);
		
		if(colorIndex < 0)
			colorIndex = 0;
			
		curColor = materialArrays.brickGlowColors[colorIndex];
		curIntensity = materialArrays.glowIntensities[colorIndex];
			
		bodyMaterial.SetColor("_EmissionColor", curColor*curIntensity);
		curAudioClip = ResourceManager.LoadAudioClip(false, audioClips[colorIndex]);
	}
	
	void OnCollisionEnter(Collision collision){
		if(isBreakable){
			collidingBall = collision.collider.GetComponent<Ball>();
			HandleHits();
			collidingBall = null;
		}
	}
	
	void HandleHits(){
		AudioSource.PlayClipAtPoint(curAudioClip,transform.position,PrefsManager.GetMasterSFXVolume());
		if(collidingBall){
			if(collidingBall.ballState == Ball.BallState.Explosive){
				Vector3 explosionPos = transform.position;
				explosionPos.z -= 2f;
				Instantiate(powerupDetails.explosionPrefab,explosionPos,Quaternion.identity);
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
				if(collidingBall.ballState == Ball.BallState.Iron)
					timesHit += 2f;
				else if(collidingBall.ballState == Ball.BallState.Feather)
					timesHit += 0.5f;
				else
					timesHit++;
			}
		} else{
			timesHit++;
		}
		
		if(timesHit >= hitPoints && hitPoints > 0){
			if(powerupDetails.hasPowerup)
				DropPowerup();
				
			GameMaster.instance.BrickDestroyed(pointValue, this.gameObject);
			Destroy(gameObject);
		} else {
			ChangeBrickColor();
		}
	}

	//Sets Powerup passed in argument
	public void SetPowerup(Powerup.PowerupType type){
		powerupDetails.hasPowerup = true;
		powerupDetails.randomPowerup = false;
		powerupDetails.powerupType = type;
	}

	//Sets Random Powerup with some random chance
	public void SetPowerup(){
		int chanceForPowerup = UnityEngine.Random.Range(0,1000);

		if(chanceForPowerup > 750){
			powerupDetails.hasPowerup = true;
			powerupDetails.randomPowerup = true;
			powerupDetails.powerupType = (Powerup.PowerupType)UnityEngine.Random.Range(0,System.Enum.GetNames(typeof(Powerup.PowerupType)).Length);
		}
	}

	public void ShowPowerup(bool show){
		if(powerupDetails.hasPowerup){
			if(show){
				GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.red);
				bodyMaterial.SetColor("_EmissionColor", Color.red);
			} else{
				GetComponent<MeshRenderer>().materials[0].SetColor("_Color", Color.white);
				bodyMaterial.SetColor("_EmissionColor", curColor*curIntensity);
			}
		}
	}
	
	void DropPowerup(){
		powerup = powerupDetails.powerupPrefab.GetComponent<Powerup>();
		powerup.powerupType = powerupDetails.powerupType;
		
		Instantiate(powerupDetails.powerupPrefab,transform.position,Quaternion.identity);
	}
	
	public void Explode(){
		if(isBreakable)
			HandleHits();
	}
	
	#endregion
}
