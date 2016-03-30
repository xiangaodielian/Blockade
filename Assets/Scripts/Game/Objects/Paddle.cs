/*--------------------------------------/
  Paddle Class - Blockade
  Controlling class for Paddle (Player)
  object and its functions
  Writen by Joe Arthur
  Latest Revision - 29 Mar, 2016
/--------------------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Paddle : MonoBehaviour {
	
	#region Variables

	//Singleton instance for Paddle
	public static Paddle instance {get; private set;}

	[HideInInspector] public bool hasStarted = false;
	public bool hasLasers = false;
	public bool mirroredMovement = false;									//FALSE - normal motion TRUE - reversed motion

	[SerializeField] private GameObject laserPrefab = null;			//Ref to Laser Prefab
	[Tooltip("Transform for Left Laser parent group.")]
	[SerializeField] private Transform leftLaserPos = null;			//Position of Left LaserTurret
	[Tooltip("Transform for Right Laser parent group.")]
	[SerializeField] private Transform rightLaserPos = null;		//Position of Right LaserTurret

	private Vector3 targetScale = new Vector3();
	private AudioSource audioSource;
	private Animator animator;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;

		DontDestroyOnLoad(gameObject);
	}

	void Start(){
		targetScale = transform.localScale;
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PrefsManager.GetMasterSFXVolume();
		audioSource.clip = ResourceManager.LoadAudioClip(false, "Laser");
		animator = GetComponent<Animator>();
		ResourceManager.SetMaterialTextures(this.gameObject);
	}
	
	void Update(){
		// Expand/Shrink scale over time
		if(targetScale != transform.localScale)
			transform.localScale = Vector3.Lerp(transform.localScale,targetScale,3f*Time.deltaTime);
	}
	
	#endregion
	#region Utility

	//Move Paddle based on inputPos from MousePos or TouchPos
	public void MovePaddle(Vector3 inputPos){
		if(mirroredMovement)
			inputPos.x = 16f-inputPos.x;
		
		Collider collider = GetComponent<Collider>();
		Vector3 paddlePos = this.transform.position;
		paddlePos.x = Mathf.Clamp(inputPos.x,0.5f+collider.bounds.extents.x,15.5f-collider.bounds.extents.x);
			
		this.transform.position = paddlePos;
	}
	
	public void ResetBall(){
		mirroredMovement = false;
		//Retract Lasers
		if(hasLasers){
			animator.SetTrigger("laserStateTrigger");
			hasLasers = false;
		}
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
				mirroredMovement = !mirroredMovement;
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
			
			default:
				Debug.LogError("Powerup Doesn't Have Type!");
				break;
		}
	}
	
	#endregion
	#region Powerup Functions
	
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
		animator.SetTrigger("laserStateTrigger");
	}
	
	//Fires Lasers, does nothing if already in FireLasers animation state
	public void FireLasers(){
		int hash = Animator.StringToHash("FireLasers");
		if(animator.GetCurrentAnimatorStateInfo(0).shortNameHash != hash){
			animator.SetTrigger("fireLasersTrigger");
			audioSource.Play();

			Instantiate(laserPrefab,leftLaserPos.position,Quaternion.identity);
			Instantiate(laserPrefab,rightLaserPos.position,Quaternion.identity);
		}
	}
	
	#endregion
}