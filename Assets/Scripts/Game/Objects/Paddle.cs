using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public bool autoPlay = false;
	public bool hasStarted = false;
	
	[SerializeField] private GameObject laserPrefab = null;
	[SerializeField] private GameObject safetyNetPrefab = null;
	
	private Ball ball;
	private bool hasLasers = false;
	private bool mirrored = false;
	private Vector3 targetScale = new Vector3();
	
	void Start(){
		ball = GameObject.FindObjectOfType<Ball>();
		targetScale = transform.localScale;
	}
	
	void Update(){
		if(!autoPlay){
			MoveWithMouse();
		} else {
			AutoPlay();
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
		Vector3 paddlePos = new Vector3(0.5f,this.transform.position.y,0f);
		float mousePosInBlocks = Input.mousePosition.x / Screen.width * 16;
		
		if(mirrored)
			mousePosInBlocks = 16-mousePosInBlocks;
		
		paddlePos.x = Mathf.Clamp(mousePosInBlocks, 1f,15f);
			
		this.transform.position = paddlePos;
	}
	
	void AutoPlay(){
		Vector3 paddlePos = new Vector3(0.5f,this.transform.position.y,0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp(ballPos.x, 0.5f,15.5f);
		this.transform.position = paddlePos;
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
