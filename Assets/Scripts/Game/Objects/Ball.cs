using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour {
	
	public Rigidbody2D rigidBody;
	public enum BallState {Normal, Sticky, Iron, Feather, Explosive};
	public BallState ballState = BallState.Normal;
	public bool isIron = false;
	public bool isFeather = false;
	public bool isExplosive = false;
	
	[SerializeField] private GameObject ballPrefab = null;
	[SerializeField] private Sprite[] spriteArray = new Sprite[5];
	[SerializeField] private AudioClip[] audioClips = new AudioClip[8];
	
	private GameMaster gameMaster;
	private Paddle paddle;
	private Vector3 paddleToBallVector;
	private bool isSticky = false;
	private bool stickOnPaddle = false;
	private float velMultiplier = 1f;
	private Color ballColor;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		gameMaster = GameObject.FindObjectOfType<GameMaster>().GetComponent<GameMaster>();
		ballColor = PrefsManager.GetBallColor();
		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = transform.position - paddle.transform.position;
		rigidBody = GetComponent<Rigidbody2D>();
		GetComponent<SpriteRenderer>().color = ballColor;
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();
		
		// Multiball Case
		if(paddle.hasStarted){
			Vector2 otherVel = new Vector2();
			Ball[] otherBalls = FindObjectsOfType<Ball>();
			foreach(Ball ball in otherBalls){
				if(ball.rigidBody.velocity.magnitude != otherVel.magnitude)
					otherVel = ball.rigidBody.velocity;
				if(ball.ballState != BallState.Normal){
					ballState = ball.ballState;
					ChangeSprite();
				}
			}
			
			rigidBody.velocity = new Vector2(Random.Range(0f,1f),Random.Range(0f,1f)).normalized*otherVel.magnitude;
		}
	}
	
	void Update(){
		if(audioSource.volume != PrefsManager.GetMasterSFXVolume())
			audioSource.volume = PrefsManager.GetMasterSFXVolume();
		
		if(!gameMaster)
			gameMaster = GameObject.FindObjectOfType<GameMaster>().GetComponent<GameMaster>();
		if(!paddle.hasStarted && !paddle.gamePaused){
			if(isSticky || isIron || isFeather || isExplosive){
				isSticky = false;
				isIron = false;
				isFeather = false;
				isExplosive = false;
				ballState = BallState.Normal;
				ChangeSprite();
			}
			// Lock Ball to Paddle until Mouse0 Pressed
			transform.position = paddle.transform.position + paddleToBallVector;
			#if UNITY_STANDALONE || UNITY_WSA || UNITY_WEBGL
			rigidBody.velocity = new Vector2 (0f,10f);
			if(Input.GetMouseButtonDown(0)){
				gameMaster.GameStart();
				paddle.hasStarted = true;
			}
			#elif UNITY_IOS || UNITY_ANDROID
			rigidBody.velocity = new Vector2 (0f,8f);
			if(Input.touchCount > 0){
				if(Input.GetTouch(0).phase == TouchPhase.Ended){
					gameMaster.GameStart();
					paddle.hasStarted = true;
				}
			}
			#endif
		}
		
		// Stick to Paddle when StickyBall active
		if(stickOnPaddle){
			rigidBody.velocity = Vector2.zero;
			transform.position = paddle.transform.position + paddleToBallVector;
			transform.position = new Vector3(transform.position.x,1.35f,transform.position.z);
			#if UNITY_STANDALONE || UNITY_WSA || UNITY_WEBGL
			rigidBody.velocity = new Vector2 (0f,10f)*velMultiplier;
			if(Input.GetMouseButtonDown(0)){
				gameMaster.GameStart();
				stickOnPaddle = false;
				audioSource.clip = audioClips[0];
			}
			#elif UNITY_IOS || UNITY_ANDROID
			rigidBody.velocity = new Vector2 (0f,8f)*velMultiplier;
			if(Input.touchCount > 0){	
				if(Input.GetTouch(0).phase == TouchPhase.Ended){
					gameMaster.GameStart();
					stickOnPaddle = false;
					audioSource.clip = audioClips[0];
				}
			}
			#endif
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		Vector2 tweak = new Vector2(Random.Range(0.25f,0.5f)*(transform.position.x-paddle.transform.position.x),0f);
		
		// Stick to Paddle when StickyBall active
		if(isSticky && collision.gameObject.tag == "Player"){
			audioSource.clip = audioClips[5];
			audioSource.Play();
			stickOnPaddle = true;
			rigidBody.velocity = Vector2.zero;
		}
		
		// Ball does not trigger Sound when hitting Bricks
		if(paddle.hasStarted){
			if(collision.gameObject.tag != "Breakable")
				audioSource.Play();
			if(collision.gameObject.tag == "Player")
				GetComponent<Rigidbody2D>().velocity += tweak;
		}
	}
	
	void ChangeSprite(){
		switch(ballState){
			case BallState.Normal:
				GetComponent<SpriteRenderer>().sprite = spriteArray[0];
				GetComponent<SpriteRenderer>().color = PrefsManager.GetBallColor();
				audioSource.clip = audioClips[0];
				break;
			
			case BallState.Sticky:
				GetComponent<SpriteRenderer>().sprite = spriteArray[1];
				break;
				
			case BallState.Iron:
				GetComponent<SpriteRenderer>().sprite = spriteArray[2];
				GetComponent<SpriteRenderer>().color = Color.white;
				audioSource.clip = audioClips[1];
				break;
				
			case BallState.Feather:
				GetComponent<SpriteRenderer>().sprite = spriteArray[3];
				GetComponent<SpriteRenderer>().color = Color.white;
				audioSource.clip = audioClips[2];
				break;
				
			case BallState.Explosive:
				GetComponent<SpriteRenderer>().sprite = spriteArray[4];
				GetComponent<SpriteRenderer>().color = Color.white;
				break;
				
			default:
				Debug.LogError("Ball Type Invalid!");
				break;
		}
	}
	
	// SpeedUp or SlowDown
	public void SetVelocity(float scale){
		rigidBody.velocity *= scale;
		velMultiplier *= scale;
	}
	
	// Split into two Balls
	public void MultiballSplit(){
		Instantiate(ballPrefab,transform.position,Quaternion.identity);
	}
	
	public void StickyBall(){
		ballState = BallState.Sticky;
		ChangeSprite();
		isSticky = true;
	}
	
	public void IronBall(){
		if(!isExplosive){
			if(!isFeather){
				ballState = BallState.Iron;
				ChangeSprite();
				isIron = true;
			} else{
				// Add Sound
				ballState = BallState.Normal;
				ChangeSprite();
				isFeather = false;
			}
		}
	}
	
	public void FeatherBall(){
		if(!isExplosive){
			if(!isIron){
				ballState = BallState.Feather;
				ChangeSprite();
				isFeather = true;
			} else{
				// Add Sound
				ballState = BallState.Normal;
				ChangeSprite();
				isIron = false;
			}
		}
	}
	
	public void ExplosiveBall(){
		if(!isIron || !isFeather){
			ballState = BallState.Explosive;
			ChangeSprite();
			isExplosive = true;
		}
	}
	
	public void BallExploded(){
		AudioSource.PlayClipAtPoint(audioClips[4],transform.position,PrefsManager.GetMasterSFXVolume());
		ballState = BallState.Normal;
		ChangeSprite();
		isExplosive = false;
	}
}