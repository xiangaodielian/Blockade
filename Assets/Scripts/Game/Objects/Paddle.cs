/*----------------------------/
  Paddle Class - Blockade
  Controlling class for Paddle
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 2 Feb, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Paddle : MonoBehaviour {

	public bool hasStarted = false;
	public bool gamePaused = false;
	
	[SerializeField] private GameObject laserPrefab = null;			//Ref to Laser Prefab
	[SerializeField] private GameObject safetyNetPrefab = null;		//Ref to Safety Net Prefab
	
	private bool hasLasers = false;
	public bool mirrored = false;					//FALSE - normal motion TRUE - reversed motion
	private Vector3 targetScale = new Vector3();
	private AudioSource audioSource;
	private GameObject laserGuns;
	
	void Start(){
		targetScale = transform.localScale;
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();
		laserGuns = GameObject.Find("LaserGuns");
		laserGuns.transform.localScale = new Vector3(1f,0.2f,1f);
	}
	
	void Update(){
		if(!gamePaused){
			#if UNITY_STANDALONE
			MoveWithMouse();
			#elif UNITY_IOS || UNITY_ANDROID
			MoveWithTouch();
			#elif UNITY_WSA  || UNITY_WEBGL
			if(Input.mousePresent)
				MoveWithMouse();
			else
				MoveWithTouch();
			#endif
		}

		
		// Expand/Shrink scale over time
		if(targetScale != transform.localScale)
			transform.localScale = Vector3.Lerp(transform.localScale,targetScale,3f*Time.deltaTime);
		
		// Control LaserGuns deployment
		if(hasLasers && laserGuns.transform.localScale.y != 1f)
			laserGuns.transform.localScale = Vector3.Lerp(laserGuns.transform.localScale,new Vector3(1f,1f,1f),3f*Time.deltaTime);
		if(!hasLasers && laserGuns.transform.localScale.y != 0.2f)
			laserGuns.transform.localScale = Vector3.Lerp(laserGuns.transform.localScale,new Vector3(1f,0.2f,1f),3f*Time.deltaTime);
		
		// Control Laser Firing
		if(hasLasers){
			if(Input.GetMouseButtonDown(0))
				FireLasers();
		}
	}
	
	//Control Paddle motion using a mouse
	void MoveWithMouse(){
		Vector3 paddlePos = new Vector3(this.transform.position.x,this.transform.position.y,0f);
		Vector3 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		
		if(mirrored)
			mousePos.x = 16f-mousePos.x;
		
		Collider2D collider = GetComponent<Collider2D>();
		paddlePos.x = Mathf.Clamp(mousePos.x,0.5f+collider.bounds.extents.x,15.5f-collider.bounds.extents.x);
			
		this.transform.position = paddlePos;
	}
	
	//Control Paddle motion using finger on a touch device
	void MoveWithTouch(){
		if(Input.touchCount > 0){
			Vector3 paddlePos = new Vector3(transform.position.x,this.transform.position.y,0f);
			Vector3 touchPos = Input.GetTouch(0).position;
			touchPos.z = 0f;
			touchPos = Camera.main.ScreenToWorldPoint(touchPos);

			if(mirrored)
				touchPos.x = 16f-touchPos.x;
			
			Collider2D collider = GetComponent<Collider2D>();
			paddlePos.x = Mathf.Clamp(touchPos.x,0.5f+collider.bounds.extents.x,15.5f-collider.bounds.extents.x);
			
			this.transform.position = paddlePos;
		}
	}
	
	public void ResetBall(){
		mirrored = false;
		hasLasers = false;
		targetScale = new Vector3(1f,transform.localScale.y,transform.localScale.z);
		hasStarted = false;
	}
	
	public void CollectPowerup(Powerup.PowerupType powerupType){
	
		Ball[] ballArray = FindObjectsOfType<Ball>();
		
		switch(powerupType){
			case Powerup.PowerupType.Expand:
				Expand(1);
				break;
			
			case Powerup.PowerupType.Shrink:
				Expand(-1);
				break;
				
			case Powerup.PowerupType.Lasers:
				if(!hasLasers)
					AddLasers();
				break;
				
			case Powerup.PowerupType.Mirror:
				mirrored = !mirrored;
				break;
				
			case Powerup.PowerupType.SpeedUp:
				foreach(Ball b in ballArray)
					b.SetVelocity(1.5f);
				break;
				
			case Powerup.PowerupType.SlowDown:
				foreach(Ball b in ballArray)
					b.SetVelocity(1f/1.5f);
				break;
				
			case Powerup.PowerupType.Multiball:
				foreach(Ball b in ballArray)
					b.MultiballSplit();
				break;
				
			case Powerup.PowerupType.StickyBall:
				foreach(Ball b in ballArray)
					b.StickyBall();
				break;
				
			case Powerup.PowerupType.IronBall:
				foreach(Ball b in ballArray)
					b.IronBall();
				break;
				
			case Powerup.PowerupType.FeatherBall:
				foreach(Ball b in ballArray)
					b.FeatherBall();
				break;
				
			case Powerup.PowerupType.Explode:
				foreach(Ball b in ballArray)
					b.ExplosiveBall();
				break;
				
			case Powerup.PowerupType.SafetyNet:
				CreateSafetyNet();
				break;
			
			default:
				Debug.LogError("Powerup Doesn't Have Type!");
				break;
		}
	}
	
	// Expand or Shrink width of Paddle (1 for Expand, -1 for Shrink)
	void Expand(int direction){
		float targetXScale = transform.localScale.x;
		targetXScale += 0.5f*direction;
		targetXScale = Mathf.Clamp(targetXScale,0.5f,1.5f);
		
		targetScale = new Vector3(targetXScale,targetScale.y,targetScale.z);
	}
	
	// Adds Lasers to Paddle
	void AddLasers(){
		hasLasers = true;
	}
	
	// Fires Lasers
	void FireLasers(){
		audioSource.Play();
		float width = GetComponent<SpriteRenderer>().bounds.size.x;
		float height = GetComponent<SpriteRenderer>().bounds.size.y;
		Vector3 leftPos = new Vector3(transform.position.x-width/2.95f,transform.position.y+height/2f,transform.position.z);
		Vector3 rightPos = new Vector3(transform.position.x+width/2.95f,transform.position.y+height/2f,transform.position.z);

		Instantiate(laserPrefab,leftPos,Quaternion.identity);
		Instantiate(laserPrefab,rightPos,Quaternion.identity);
	}
	
	// Add a SafetyNet
	void CreateSafetyNet(){
		Instantiate(safetyNetPrefab);
	}
}