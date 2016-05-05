/*----------------------------/
  Shield Class - Blockade
  Controlling class for Shield
  object acting as the Playspace
  border and its functions
  Writen by Joe Arthur
  Latest Revision - 4 May, 2016
/-----------------------------*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Light))]
public class Shield : MonoBehaviour {
	
	#region Variables

	[Tooltip("Angle of Constraint for rotation of Shield when allowed.")]
	[SerializeField] private float rotateConstraintAngle = 20f;
	[SerializeField] private float maxLightIntensity = 7f;
	[SerializeField] private float restingLightIntensity = 3f;

	private BoxCollider boxCollider;
	private Rigidbody rigidBody;
	private Material shieldMat;
	private float blendAmount = 0f;					//Controls Dissolve amount for Shader
	private float edgeWidth = 83f;					//Edge Width of Dissolve for Shader
	private bool dissolve = false;
	private Light shieldLight;
	private Vector3 shieldAnchorPos = Vector3.zero;
	
	#endregion
	#region Mono Functions
	
	void Awake(){
		ShieldSetup();
	}
	
	//Deals with Rotation of Shield when allowed (constrains rotation to +/- rotateContrainAngle)
	//and locking Shield to it's position
	void FixedUpdate(){
		float curRot = transform.localEulerAngles.y;
		if(curRot > rotateConstraintAngle && curRot < 180f)
			curRot = rotateConstraintAngle;
		if(curRot > 180f && curRot < 360f-rotateConstraintAngle)
			curRot = 360f - rotateConstraintAngle;
		transform.localEulerAngles = new Vector3(0f,curRot,0f);

		if(transform.localPosition != shieldAnchorPos && shieldAnchorPos != Vector3.zero)
			transform.localPosition = shieldAnchorPos;
	}
	
	void Update(){
		if(dissolve && blendAmount < 1f){
			blendAmount += 0.75f*Time.deltaTime;
			shieldMat.SetFloat("_BlendAmount", blendAmount);
			
			if(shieldMat.GetFloat("_BlendAmount") > 0.95f){
				blendAmount = 1f;
				shieldMat.SetFloat("_BlendAmount", blendAmount);
				shieldMat.SetFloat("_EdgeWidth", 0f);
				shieldAnchorPos = transform.localPosition;
				UIManager.instance.BeginGame();
			}
		}
		
		AdjustLight();
		
	}
	
	#endregion
	#region Utility Functions

	void ShieldSetup()
	{
		boxCollider = GetComponent<BoxCollider> ();
		boxCollider.isTrigger = true;

		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.mass = 0.0001f;
		rigidBody.useGravity = false;
		rigidBody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

		shieldMat = GetComponent<MeshRenderer> ().material;
		shieldMat.SetFloat ("_BlendAmount", blendAmount);
		shieldMat.SetFloat ("_EdgeWidth", edgeWidth);

		shieldLight = GetComponent<Light> ();
		shieldLight.type = LightType.Point;
		shieldLight.renderMode = LightRenderMode.ForcePixel;
		shieldLight.color = new Color (137f / 255f, 241f / 255f, 246f / 255f);
		shieldLight.intensity = 0f;
		shieldLight.range = 0f;
	}
	
	//Light Shield segment when hit by Ball
	void OnTriggerEnter(Collider col){
		if(col.tag != "Wall"){
			#if UNITY_WEBGL
			shieldLight.intensity = maxLightIntensity * 1.3f;
			#else
			shieldLight.intensity = maxLightIntensity;
			#endif
		}
	}

	//Light Shield segment when hit by Ball
	void OnCollisionEnter(Collision col){
		if(col.collider.tag != "Wall"){
			#if UNITY_WEBGL
			shieldLight.intensity = maxLightIntensity * 1.3f;
			#else
			shieldLight.intensity = maxLightIntensity;
			#endif
		}
	}
	
	//Toggles ability for Shield to rotate when hit
	public void ToggleMovingShields(bool b){	
		boxCollider.isTrigger = !b;
	}

	//Pulse Range and Reset Intensity
	void AdjustLight(){
		float minRange = 1.4f;
		float maxRange = 1.5f;
		float pulseSpeed = 0.1f;

		#if UNITY_WEBGL
		maxRange = 1.7f;
		pulseSpeed = 0.3f;
		#endif
		
		shieldLight.range = minRange + Mathf.PingPong(Time.time * pulseSpeed, maxRange-minRange);
		
		//Drop Intensity to restingLightIntensity (* 1.5 on WebGL) if above (i.e. after segment is hit)
		#if UNITY_WEBGL
		if(lightIntensity == maxLightIntensity * 1.3f)
			lightIntensity = 5f;
		else{
			if(shieldLight.intensity > lightIntensity)
				shieldLight.intensity -= 0.25f;
		}
		#else
		if(shieldLight.intensity != restingLightIntensity && dissolve)
			shieldLight.intensity = Mathf.Lerp(shieldLight.intensity, restingLightIntensity, Time.deltaTime * 2f);
		#endif
	}
	
	public void DissolveIn(){
		dissolve = true;
	}
	
	#endregion
}
