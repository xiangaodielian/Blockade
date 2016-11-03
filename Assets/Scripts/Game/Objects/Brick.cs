using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ApplicationManagement;
using ApplicationManagement.ResourceControl;

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

    public enum BrickType {
        One,
        Two,
        Three,
        Four,
        Five,
        Unbreakable
    };
	
	[SerializeField] private string[] audioClips = new string[5];
	[SerializeField] private MaterialArrays materialArrays = null;
	[SerializeField] private PowerupDetails powerupDetails = null;
	[Tooltip("Number of hits it takes to destroy the Brick.")]
	[SerializeField] private BrickType brickType = BrickType.One;

    private static int breakableCount;

    private Powerup powerup;
	private Color curColor = Color.white;
	private float curIntensity = 1f;
    private AudioClip curAudioClip;
    private int hitPoints;
    private int pointValue;
    private float timesHit;
    private bool isBreakable;
    private Ball collidingBall;
    private Material bodyMaterial;
	
	#endregion
	#region MonoDevelop Functions
	
	void Start(){
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
		ResourceManager.SetMaterialTextures(gameObject);

	    if(isBreakable)
	        breakableCount++;
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
			case BrickType.One:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[0];
				curIntensity = materialArrays.glowIntensities[0];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[0]);
				hitPoints = 1;
				break;
			case BrickType.Two:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[1];
				curIntensity = materialArrays.glowIntensities[1];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[1]);
				hitPoints = 2;
				break;
			case BrickType.Three:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[2];
				curIntensity = materialArrays.glowIntensities[2];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[2]);
				hitPoints = 3;
				break;
			case BrickType.Four:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[3];
				curIntensity = materialArrays.glowIntensities[3];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[3]);
				hitPoints = 4;
				break;
			case BrickType.Five:
				gameObject.tag = "Breakable";
				curColor = materialArrays.brickGlowColors[4];
				curIntensity = materialArrays.glowIntensities[4];
				curAudioClip = ResourceManager.LoadAudioClip(audioClips[4]);
				hitPoints = 5;
				break;
			case BrickType.Unbreakable:
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
		
		pointValue = 50 * hitPoints * (PrefsManager.GetDifficulty() + 1);
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
		curAudioClip = ResourceManager.LoadAudioClip(audioClips[colorIndex]);
	}
	
	void OnCollisionEnter(Collision collision){
		if(isBreakable){
			collidingBall = collision.collider.GetComponent<Ball>();
			HandleHits();
			collidingBall = null;
		}
	}
	
	void HandleHits(){
		AudioSource.PlayClipAtPoint(curAudioClip, transform.position, PrefsManager.GetMasterSFXVolume());

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
				EventManager.Instance.Raise(new GameObjectManager.SpawnPowerupEvent(transform.position, powerupDetails.powerupType));

            PlayerManager.Instance.AddToPlayerScore(pointValue);
            breakableCount--;
		    if(breakableCount == 0) {
		        PlayerManager.Instance.ActivePlayer.hasStarted = false;
		        GUIManager.Instance.InGameGui.ToggleEndLevelPanel(true);
		        GUIManager.Instance.InGameGui.CalculateTimeBonus();
		    }
		    
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
	
	public void Explode(){
		if(isBreakable)
			HandleHits();
	}
	
	#endregion
}
