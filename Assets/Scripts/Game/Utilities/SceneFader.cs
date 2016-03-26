/*-----------------------------------/
  SceneFader Class - Blockade
  Controls Fade Transitions between
  scenes
  Writen by Joe Arthur
  Latest Revision - 25 Mar, 2016
/----------------------------------*/

using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {
	
	#region Variables
	
	[System.Serializable] private class FaderDetails{
		public float fadeInSpeed = 0.75f;
		public float fadeOutSpeed = 2f;
		public float pauseDuration = 2f;
		public Image fadeImage = null;
		public bool fadeInInstantly = false;
	}

	[SerializeField] private FaderDetails faderDetails = null;
	
	private bool fadingIn;
	private float pauseStart = 0f;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		fadingIn = faderDetails.fadeInInstantly;
		faderDetails.fadeImage.color = Color.clear;
	}

	void Update(){
		if(fadingIn)
			FadeIn();
		
		if(Time.timeSinceLevelLoad - pauseStart >= faderDetails.pauseDuration && pauseStart != 0f)
			FadeOut();
	}
	
	#endregion
	#region Fade Utility Functions
	
	private void FadeIn(){
		FadeToWhite();
		
		if(faderDetails.fadeImage.color.a >= 0.95f){
			faderDetails.fadeImage.color = Color.white;
			fadingIn = false;
			pauseStart = Time.timeSinceLevelLoad;
		}
	}
	
	public void FadeOut(){
		FadeToClear();
		
		if(faderDetails.fadeImage.color.a <= 0.05f){
			faderDetails.fadeImage.color = Color.clear;
			GameMaster.instance.ChangeToLevel("Next");
		}
	}
	
	private void FadeToWhite(){
		faderDetails.fadeImage.color = Color.Lerp(faderDetails.fadeImage.color, Color.white, faderDetails.fadeInSpeed * Time.deltaTime);
	}
	
	private void FadeToClear(){
		faderDetails.fadeImage.color = Color.Lerp(faderDetails.fadeImage.color, Color.clear, faderDetails.fadeOutSpeed * Time.deltaTime);
	}
	
	#endregion
}
