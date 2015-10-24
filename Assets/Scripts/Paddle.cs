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
	
	void Start(){
		ball = GameObject.FindObjectOfType<Ball>();
	}
	
	void Update(){
		if(!autoPlay){
			MoveWithMouse();
		} else {
			AutoPlay();
		}
		
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
		
		paddlePos.x = Mathf.Clamp(mousePosInBlocks, 0.75f,15.25f);
			
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
					b.SetVelocity(2f);
				break;
				
			case Powerup.PowerupType.SlowDown:
				foreach(Ball b in ballArray)
					b.SetVelocity(0.5f);
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
		// Add Animation
		float xScale = transform.localScale.x;
		xScale += 0.5f*direction;
		xScale = Mathf.Clamp(xScale,0.5f,1.5f);
		
		transform.localScale = new Vector3(xScale,1f,1f);
	}
	
	// Adds Lasers to Paddle
	void AddLasers(){
		hasLasers = true;
	}
	
	// Fires Lasers
	void FireLasers(){
		// Add Animation + LaserShooters
		float width = GetComponent<SpriteRenderer>().bounds.size.x;
		float height = GetComponent<SpriteRenderer>().bounds.size.y;
		Vector3 leftPos = new Vector3(transform.position.x-width/2.25f,transform.position.y+height/2f,transform.position.z);
		Vector3 rightPos = new Vector3(transform.position.x+width/2.25f,transform.position.y+height/2f,transform.position.z);

		Instantiate(laserPrefab,leftPos,Quaternion.identity);
		Instantiate(laserPrefab,rightPos,Quaternion.identity);
	}
	
	// Add a SafetyNet
	void CreateSafetyNet(){
		// Add Animation
		Instantiate(safetyNetPrefab);
	}
}
