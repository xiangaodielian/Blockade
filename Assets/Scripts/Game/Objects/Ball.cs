/*----------------------------/
  Ball Class - Blockade
  Controlling class for Ball
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 24 Mar, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour {
	
	#region Variables
	
	[HideInInspector] public Rigidbody rigidBody;
	public enum BallState {Normal, Sticky, Iron, Feather, Explosive};
	public BallState ballState = BallState.Normal;
	
	[SerializeField] private GameObject ballPrefab = null;
	[SerializeField] private Material[] materialArray = new Material[5];
	[SerializeField] private AudioClip[] audioClips = new AudioClip[5];

	private Color ballColor;
	private AudioSource audioSource;
	private bool isSticky = false;
	private bool stickOnPaddle = false;
	private float velMultiplier = 1f;
	private Material curMat = null;
	
	#endregion
	#region MonoDevelop Functions
	
	void Start(){
		ballColor = PrefsManager.GetBallColor();
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();
		curMat = GetComponentInChildren<MeshRenderer>().material;
		curMat.SetColor("_Color", ballColor);
		GetComponentInChildren<MeshRenderer>().material = curMat;
		
		//Multiball Case
		if(Paddle.instance.hasStarted){
			Vector2 otherVel = new Vector2();
			Ball[] otherBalls = FindObjectsOfType<Ball>();
			foreach(Ball ball in otherBalls){
				if(ball.rigidBody.velocity.magnitude != otherVel.magnitude)
					otherVel = ball.rigidBody.velocity;
				if(ball.ballState != BallState.Normal){
					ballState = ball.ballState;
					ChangeMaterial();
				}
			}
			
			rigidBody.velocity = new Vector2(Random.Range(0f,1f),Random.Range(0f,1f)).normalized*otherVel.magnitude;
		}
	}
	
	void Update(){
		//Set Ball SFX Volume if changed
		if(audioSource.volume != PrefsManager.GetMasterSFXVolume())
			audioSource.volume = PrefsManager.GetMasterSFXVolume();
		
		//Pre-Launch
		if(!Paddle.instance.hasStarted && !GameMaster.instance.gamePaused && GameMaster.instance.allowStart)
			LaunchBall(false);
		
		//Explosive Ball Pulse
		if(ballState == BallState.Explosive)
			ExplosiveGlowPulse();
		
		//Stick to Paddle when StickyBall active
		if(stickOnPaddle)
			LaunchBall(true);
	}

	void FixedUpdate(){
		if(rigidBody.velocity.magnitude < 10f*velMultiplier-1f || rigidBody.velocity.magnitude > 10f*velMultiplier+1f){
			Vector3 newVel = rigidBody.velocity.normalized;
			newVel *= 10f*velMultiplier;
			rigidBody.velocity = newVel;
		}
	}
	
	#endregion
	#region Utility Functions

	void LaunchBall(bool sticky){
		//Set Ball to Normal if at Game Start or NOT Sticky
		if(!sticky){
			if (ballState != BallState.Normal) {
				ballState = BallState.Normal;
				ChangeMaterial();
			}
		}
		
		//Lock Ball to Paddle until LMB Pressed (or Touch Released)
		rigidBody.velocity = Vector3.zero;
		transform.position = Paddle.instance.transform.position + new Vector3(0f,0.35f,0f);
		#if UNITY_STANDALONE || UNITY_WSA || UNITY_WEBGL
			if(Input.GetMouseButtonDown(0)){
				velMultiplier = 1f;
				rigidBody.velocity = new Vector3(0f, 10f, 0f);
				UIManager.instance.ToggleLaunchPrompt (false);
				if(sticky){
					stickOnPaddle = false;
					audioSource.clip = audioClips[0];
				} else
					Paddle.instance.hasStarted = true;
			}
		#elif UNITY_IOS || UNITY_ANDROID
			rigidBody.velocity = new Vector2 (0f,8f);
			if(Input.touchCount > 0){
				if(Input.GetTouch(0).phase == TouchPhase.Ended){
					GameMaster.instance.GameStart();
					paddle.hasStarted = true;
				}
			}
		#endif
	}

	//Pulse Emissive Glow when Explosive
	void ExplosiveGlowPulse(){
		float maxGlow = 1.75f;
		float minGlow = 1.25f;
		float glowSpeed = 2f;
		float curGlow = minGlow + Mathf.PingPong(Time.time * glowSpeed, maxGlow-minGlow);
		
		GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.red * curGlow);
	}
	
	void OnCollisionEnter(Collision collision){
		Vector3 tweak = new Vector3(Random.Range(0.25f,0.5f)*(transform.position.x-Paddle.instance.transform.position.x),0f,0f);
		
		// Stick to Paddle when StickyBall active
		if(isSticky && collision.gameObject.tag == "Player"){
			if(rigidBody.velocity != Vector3.zero){
				audioSource.clip = audioClips[3];
				audioSource.Play();
				stickOnPaddle = true;
			}
		}
		
		// Ball does not trigger Sound when hitting Bricks
		if(Paddle.instance.hasStarted){
			if(collision.gameObject.tag != "Breakable")
				audioSource.Play();
			if(collision.gameObject.tag == "Player")
				rigidBody.velocity += tweak;
		}
	}
	
	//Change current Sprite to reflect Ball State (Normal, Iron, etc)
	void ChangeMaterial(){
		switch(ballState){
			case BallState.Normal:
				isSticky = false;
				curMat = materialArray[0];
				curMat.SetColor("_Color", ballColor);
				curMat.SetColor("_EmissionColor", Color.black);
				GetComponentInChildren<MeshRenderer>().material = curMat;
				audioSource.clip = audioClips[0];
				break;
			
			case BallState.Sticky:
				curMat = materialArray[1];
				break;
				
			case BallState.Iron:
				curMat = materialArray[2];
				curMat.SetColor("_Color", Color.white);
				GetComponentInChildren<MeshRenderer>().material = curMat;
				audioSource.clip = audioClips[1];
				break;
				
			case BallState.Feather:
				curMat = materialArray[3];
				curMat.SetColor("_Color", Color.white);
				GetComponentInChildren<MeshRenderer>().material = curMat;
				audioSource.clip = audioClips[2];
				break;
				
			case BallState.Explosive:
				curMat = materialArray[4];
				GetComponentInChildren<MeshRenderer>().material = curMat;
				break;
				
			default:
				Debug.LogError("Ball Type Invalid!");
				break;
		}
	}
	
	#endregion
	#region Powerup/Ball State Functions
	
	// SpeedUp or SlowDown
	public void SetVelocity(float scale){
		rigidBody.velocity *= scale;
		velMultiplier *= scale;
	}
	
	// Split into two Balls
	public void MultiballSplit(){
		Instantiate(ballPrefab,transform.position,Quaternion.identity);
	}
	
	//Change to Sticky
	public void StickyBall(){
		ballState = BallState.Sticky;
		ChangeMaterial();
		isSticky = true;
	}
	
	//Change to Iron
	public void IronBall(){
		if(ballState != BallState.Explosive){
			if(ballState != BallState.Feather){
				ballState = BallState.Iron;
				ChangeMaterial();
			} else{
				ballState = BallState.Normal;
				ChangeMaterial();
			}
		}
	}
	
	//Change to Feather
	public void FeatherBall(){
		if(ballState != BallState.Explosive){
			if(ballState != BallState.Iron){
				ballState = BallState.Feather;
				ChangeMaterial();
			} else{
				ballState = BallState.Normal;
				ChangeMaterial();
			}
		}
	}
	
	//Change to Explosive
	public void ExplosiveBall(){
		if(ballState != BallState.Iron || ballState != BallState.Feather){
			ballState = BallState.Explosive;
			ChangeMaterial();
		}
	}
	
	//Explosive Ball hit Brick
	public void BallExploded(){
		AudioSource.PlayClipAtPoint(audioClips[4],transform.position,PrefsManager.GetMasterSFXVolume());
		ballState = BallState.Normal;
		ChangeMaterial();
	}
	
	#endregion
}