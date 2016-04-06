/*----------------------------/
  Playspace Class - Blockade
  Controlling class for Playspace
  object (Walls, Background, etc)
  and its functions
  Writen by Joe Arthur
  Latest Revision - 6 Apr, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlaySpace : MonoBehaviour {
	
	#region Variables
	
	//Singleton instance of Playspace
	public static PlaySpace instance {get; private set;}
	
	[Tooltip("Reference to the Shields childed to the Playspace.")]
	[SerializeField] private GameObject[] shields = new GameObject[25];				//Array of all Shields (children of Playspace)
	[Tooltip("The Long Colliders that are active when Shields aren't rotating.")]
	[SerializeField] private GameObject[] longColliders = new GameObject[3];		//Array of Long Box Colliders
	
	private float timer = 0;
	private Animator animator;
	
	#endregion
	#region Mono Functionc
	
	void Awake(){
		if(instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
	}
	
	void Start(){
		animator = GetComponent<Animator>();
		
		foreach(GameObject shield in shields)
			shield.AddComponent<Shield>();

		ResourceManager.SetMaterialTextures(this.gameObject);
	}
	
	void Update(){
		//Deploy ShieldProjectors when Time is reached
		if(timer > 0f){
			if(Time.time - timer > 1f){
				animator.SetTrigger("deployTrigger");
				timer = 0f;
			}
		}
	}
	
	#endregion
	#region Utility Functions
	
	public void StartTimer(){	
		timer = Time.time;
	}
	
	//Dissolve Shields into scene
	public void ShowShields(){
		foreach(GameObject shield in shields)
			shield.GetComponent<Shield>().DissolveIn();
			
		if(LevelManager.GetLevelNum() >= 5)
			SetMovingShields(true);
		else
			SetMovingShields(false);
	}
	
	//Set Shields as capable of rotation on hit
	void SetMovingShields(bool b){
		foreach(GameObject longCollider in longColliders)
			longCollider.SetActive(!b);
			
		foreach(GameObject shield in shields)
			shield.GetComponent<Shield>().ToggleMovingShields(b);
	}
	
	#endregion
}
