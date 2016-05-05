/*----------------------------/
  Ball Class - Blockade
  Controlling class for Ball
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 5 May, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour {
	
	#region Variables
	

	public enum BallState {Normal, Sticky, Iron, Feather, Explosive};
	[Tooltip("Current State that the ball is in (i.e. Normal, Iron, Explosive, etc).")]
	public BallState ballState = BallState.Normal;
	[HideInInspector] public Rigidbody rigidBody;
	[HideInInspector] public bool lockToPaddle = false;
	
	[Tooltip("Reference to own Prefab to handle Multiball splitting.")]
	[SerializeField] private GameObject ballPrefab = null;
	[Tooltip("Array of materials for each Ball State.")]
	[SerializeField] private Material[] materialArray = new Material[4];
	[Tooltip("Array of Audio for each Ball State.")]
	[SerializeField] private string[] audioClips = new string[5];
	[Tooltip("Speed of the Ball with no Powerup modification.")]
	[SerializeField] private float baseBallSpeed = 8f;
	[SerializeField] private ParticleSystem normalParticles = null;
	[SerializeField] private ParticleSystem explosiveParticles = null;

	private Color ballColor;
	private AudioSource audioSource;
	private bool isSticky = false;
	private float velMultiplier = 1f;
	private Material curMat = null;
	
	#endregion
	#region MonoDevelop Functions
	
	void Start(){
		ballColor = PrefsManager.GetBallColor();
		normalParticles = GetComponentInChildren<ParticleSystem>();
		normalParticles.startColor = ballColor;
		curMat = GetComponentInChildren<MeshRenderer>().material;
		curMat.SetColor("_FalloffColor", ballColor);
		GetComponentInChildren<MeshRenderer>().material = curMat;

		rigidBody = GetComponent<Rigidbody>();

		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();
		audioSource.clip = ResourceManager.LoadAudioClip(false, audioClips[0]);


		
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

		ResourceManager.SetMaterialTextures(this.gameObject);
	}
	
	void Update(){
		//Set Ball SFX Volume if changed
		if(audioSource.volume != PrefsManager.GetMasterSFXVolume())
			audioSource.volume = PrefsManager.GetMasterSFXVolume();
		
		//Pre-Launch
		if(!Paddle.instance.hasStarted && !GameMaster.instance.gamePaused && GameMaster.instance.allowStart)
			lockToPaddle = true;
		
		//Explosive Ball Pulse
		if(ballState == BallState.Explosive)
			ExplosiveGlowPulse();

		if(lockToPaddle){
			transform.position = Paddle.instance.transform.position + new Vector3(0f,0.35f,0f);
			rigidBody.velocity = Vector3.zero;
		}
	}

	void FixedUpdate(){
		if(!lockToPaddle){
			if(rigidBody.velocity.magnitude < baseBallSpeed*velMultiplier-1f || rigidBody.velocity.magnitude > baseBallSpeed*velMultiplier+1f){
				Vector3 newVel = rigidBody.velocity.normalized;
				newVel *= baseBallSpeed*velMultiplier;

				Vector3 returnVel = new Vector3();
				returnVel.x = Mathf.Lerp(rigidBody.velocity.x, newVel.x, 7f * Time.fixedDeltaTime);
				returnVel.y = Mathf.Lerp(rigidBody.velocity.y, newVel.y, 7f * Time.fixedDeltaTime);
				returnVel.z = Mathf.Lerp(rigidBody.velocity.z, newVel.z, 7f * Time.fixedDeltaTime);

				rigidBody.velocity = returnVel;
			}
		}
	}
	
	#endregion
	#region Utility Functions

	public void LaunchBall(){
		lockToPaddle = false;
		velMultiplier = 1f;
		rigidBody.velocity = new Vector3(0f, 1f, 0f);
		UIManager.instance.ToggleLaunchPrompt(false);

		if(ballState == BallState.Sticky)
			audioSource.clip = ResourceManager.LoadAudioClip(false, audioClips[0]);
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
		float tweak = 10f*(transform.position.x-Paddle.instance.transform.position.x);
		Vector3 tweakVector = new Vector3(tweak,0f,0f);
		
		// Stick to Paddle when StickyBall active
		if(isSticky && collision.gameObject.tag == "Player"){
			if(rigidBody.velocity != Vector3.zero){
				audioSource.clip = ResourceManager.LoadAudioClip(false, audioClips[3]);
				audioSource.Play();
				lockToPaddle = true;
			}
		}
		
		// Ball does not trigger Sound when hitting Bricks
		if(Paddle.instance.hasStarted){
			if(collision.gameObject.tag != "Breakable")
				audioSource.Play();
			if(collision.gameObject.tag == "Player")
				rigidBody.velocity += tweakVector;
		}
	}
	
	//Change current Sprite to reflect Ball State (Normal, Iron, etc)
	void ChangeMaterial(){
		switch(ballState){
			case BallState.Normal:
				isSticky = false;
				explosiveParticles.Stop();
				curMat = materialArray[0];
				curMat.SetColor("_FalloffColor", ballColor);
				normalParticles.Play();
				audioSource.clip = ResourceManager.LoadAudioClip(false, audioClips[0]);
				break;
			
			case BallState.Sticky:
				normalParticles.Stop();
				explosiveParticles.Stop();
				curMat = materialArray[1];
				break;
				
			case BallState.Iron:
				isSticky = false;
				normalParticles.Stop();
				explosiveParticles.Stop();
				curMat = materialArray[2];
				curMat.SetColor("_Color", Color.white);
				audioSource.clip = ResourceManager.LoadAudioClip(false, audioClips[1]);
				break;
				
			case BallState.Feather:
				isSticky = false;
				normalParticles.Stop();
				explosiveParticles.Stop();
				curMat = materialArray[3];
				curMat.SetColor("_Color", Color.white);
				audioSource.clip = ResourceManager.LoadAudioClip(false, audioClips[2]);
				break;
				
			case BallState.Explosive:
				isSticky = false;
				normalParticles.Stop();
				explosiveParticles.Play();
				curMat = materialArray[0];
				break;
				
			default:
				Debug.LogError("Ball Type Invalid!");
				break;
		}

		GetComponentInChildren<MeshRenderer>().material = curMat;
		ResourceManager.SetMaterialTextures(this.gameObject);
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
		AudioClip clip = ResourceManager.LoadAudioClip(false, audioClips[4]);
		AudioSource.PlayClipAtPoint(clip, transform.position, PrefsManager.GetMasterSFXVolume());
		ballState = BallState.Normal;
		ChangeMaterial();
	}
	
	#endregion
}