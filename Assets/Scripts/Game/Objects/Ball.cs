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
				if(ball.rigidBody.velocity.magnitude > otherVel.magnitude)
					otherVel = ball.rigidBody.velocity;
				if(ball.ballState != BallState.Normal){
					ballState = ball.ballState;
					ChangeSprite();
				}
			}
			
			rigidBody.velocity = new Vector2(Random.Range(0f,1f),Random.Range(0f,1f))*otherVel.magnitude;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!gameMaster)
			gameMaster = GameObject.FindObjectOfType<GameMaster>().GetComponent<GameMaster>();
		if(!paddle.hasStarted && !paddle.gamePaused){
			// Lock Ball to Paddle until Mouse0 Pressed
			transform.position = paddle.transform.position + paddleToBallVector;
			if(Input.GetMouseButtonDown(0)){
				gameMaster.GameStart();
				paddle.hasStarted = true;
				rigidBody.velocity = new Vector2 (0f,10f);
			}
		}
		
		// Stick to Paddle when StickyBall active
		if(stickOnPaddle){
			rigidBody.velocity = Vector2.zero;
			transform.position = paddle.transform.position + paddleToBallVector;
			transform.position = new Vector3(transform.position.x,0.85f,transform.position.z);
			if(Input.GetKeyDown (KeyCode.Mouse0)){
				rigidBody.velocity = new Vector2 (0f,15f)*velMultiplier;
				stickOnPaddle = false;
			}
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		Vector2 tweak = new Vector2(Random.Range(0.25f,0.5f)*(transform.position.x-paddle.transform.position.x),0f);
		
		// Stick to Paddle when StickyBall active
		if(isSticky && collision.gameObject.tag == "Player"){
			stickOnPaddle = true;
			rigidBody.velocity = Vector2.zero;
		}
		
		// Ball does not trigger Sound when Brick is Destroyed
		if(paddle.hasStarted){
			audioSource.Play();
			if(collision.gameObject.tag == "Player")
				GetComponent<Rigidbody2D>().velocity += tweak;
		}
	}
	
	void ChangeSprite(){
		switch(ballState){
			case BallState.Normal:
				GetComponent<SpriteRenderer>().sprite = spriteArray[0];
				break;
			
			case BallState.Sticky:
				GetComponent<SpriteRenderer>().sprite = spriteArray[1];
				break;
				
			case BallState.Iron:
				GetComponent<SpriteRenderer>().sprite = spriteArray[2];
				break;
				
			case BallState.Feather:
				GetComponent<SpriteRenderer>().sprite = spriteArray[3];
				break;
				
			case BallState.Explosive:
				GetComponent<SpriteRenderer>().sprite = spriteArray[4];
				break;
				
			default:
				Debug.LogError("Ball Type Invalid!");
				break;
		}
	}
	
	// SpeedUp or SlowDown
	public void SetVelocity(float scale){
		// Add Sounds
		rigidBody.velocity *= scale;
		velMultiplier *= scale;
	}
	
	// Split into two Balls
	public void MultiballSplit(){
		// Add Sound
		Instantiate(ballPrefab,transform.position,Quaternion.identity);
	}
	
	public void StickyBall(){
		// Add Sound
		ballState = BallState.Sticky;
		ChangeSprite();
		isSticky = true;
	}
	
	public void IronBall(){
		if(!isExplosive){
			if(!isFeather){
				// Add Sound
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
				// Add Sound
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
			// Add Sound
			ballState = BallState.Explosive;
			ChangeSprite();
			isExplosive = true;
		}
	}
	
	public void BallExploded(){
		// Add Sound
		ballState = BallState.Normal;
		ChangeSprite();
		isExplosive = false;
	}
}
