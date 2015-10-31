using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public bool hasStarted = false;
	public bool gamePaused = false;
	
	[SerializeField] private GameObject laserPrefab = null;
	[SerializeField] private GameObject safetyNetPrefab = null;
	
	private bool hasLasers = false;
	public bool mirrored = false;
	private Vector3 targetScale = new Vector3();
	
	void Start(){
		targetScale = transform.localScale;
	}
	
	void Update(){
		if(!gamePaused){
			#if UNITY_STANDALONE
				MoveWithMouse();
			#endif
			
			#if UNITY_IOS || UNITY_ANDROID
				MoveWithTouch();
			#endif
		}

		
		// Expand/Shrink scale over time
		if(targetScale != transform.localScale)
			transform.localScale = Vector3.Lerp(transform.localScale,targetScale,3f*Time.deltaTime);
		
		// Control Laser Firing
		if(hasLasers){
			if(Input.GetKeyDown(KeyCode.Mouse0))
				FireLasers();
		}
	}
	
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
		// Add Sound
		float targetXScale = transform.localScale.x;
		targetXScale += 0.5f*direction;
		targetXScale = Mathf.Clamp(targetXScale,0.5f,1.5f);
		
		targetScale = new Vector3(targetXScale,targetScale.y,targetScale.z);
	}
	
	// Adds Lasers to Paddle
	void AddLasers(){
		// Add Animation + LaserShooters
		hasLasers = true;
	}
	
	// Fires Lasers
	void FireLasers(){
		// Add Sound
		float width = GetComponent<SpriteRenderer>().bounds.size.x;
		float height = GetComponent<SpriteRenderer>().bounds.size.y;
		Vector3 leftPos = new Vector3(transform.position.x-width/2.25f,transform.position.y+height/2f,transform.position.z);
		Vector3 rightPos = new Vector3(transform.position.x+width/2.25f,transform.position.y+height/2f,transform.position.z);

		Instantiate(laserPrefab,leftPos,Quaternion.identity);
		Instantiate(laserPrefab,rightPos,Quaternion.identity);
	}
	
	// Add a SafetyNet
	void CreateSafetyNet(){
		// Add Sound
		Instantiate(safetyNetPrefab);
	}
}
