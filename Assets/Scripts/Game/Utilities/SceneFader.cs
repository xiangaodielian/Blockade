using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

	public float fadeInSpeed = 0.75f;
	public float fadeOutSpeed = 2f;
	public float pauseDuration = 2f;
	
	private bool sceneStarting = true;
	private Image fadeImage;
	private GameMaster gameMaster;
	private float pauseStart = 0f;
	
	void Awake(){
		fadeImage = GetComponent<Image>();
		gameMaster = FindObjectOfType<GameMaster>();
	}
	
	void Update(){
		if(sceneStarting)
			StartScene();
		
		if(Time.timeSinceLevelLoad - pauseStart >= pauseDuration)
			EndScene();
	}
	
	void StartScene(){
		FadeToWhite();
		
		if(fadeImage.color.r >= 0.95f){
			fadeImage.color = Color.white;
			sceneStarting = false;
			pauseStart = Time.timeSinceLevelLoad;
		}
	}
	
	public void EndScene(){
		FadeToBlack();
		
		if(fadeImage.color.r <= 0.05f){
			fadeImage.color = Color.black;
			gameMaster.ChangeToLevel("Next");
		}
	}
	
	void FadeToWhite(){
		fadeImage.color = Color.Lerp(fadeImage.color, Color.white, fadeInSpeed * Time.deltaTime);
	}
	
	void FadeToBlack(){
		fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeOutSpeed * Time.deltaTime);
	}
}
