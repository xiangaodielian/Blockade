using UnityEngine;
using UnityEngine.UI;
using ApplicationManagement;

public class SceneFader : MonoBehaviour {
	
	#region Variables
	
	[System.Serializable] public class FaderDetails{
		public enum TransitionType {SceneChange, MenuChange};
		[Tooltip("Dictates behavior after fade. Scene Change loads a new Scene, Menu Change loads a new Menu.")]
		public TransitionType transitionType = TransitionType.SceneChange;

		[Tooltip("Color to fade Image to.")]
		public Color fadeInColor = Color.white;
		public Color fadeOutColor = Color.white;
		public float fadeInSpeed = 1f;
		public float fadeOutSpeed = 1f;
		public float pauseDuration = 1f;
		[Tooltip("Image to fade.")]
		public Image fadeImage = null;
		[Tooltip("Start image fade instantly on Scene Load.")]
		public bool fadeInInstantly = false;
		public bool instantFadeAfterPause = true;
	}

	public FaderDetails faderDetails = null;

	private bool fadingOut = false;
	private bool fadingIn;
	private float pauseStart = 0f;
	private string screenToLoad = "";
	private bool screenLoaded = false;
	
	#endregion
	#region MonoDevelop Functions
	
	void Awake(){
		fadingIn = faderDetails.fadeInInstantly;
		faderDetails.fadeImage.color = faderDetails.fadeOutColor;
	}

	void Update(){
		if(fadingIn)
			FadeIn();
		else if(fadingOut)
			FadeOut();
		
		if(faderDetails.instantFadeAfterPause){
			if(Time.timeSinceLevelLoad - pauseStart >= faderDetails.pauseDuration && pauseStart != 0f)
				fadingOut = true;
		}
	}
	
	#endregion
	#region Fade Utility Functions
	
	public void StartFade(string screen){
		fadingIn = true;
		screenToLoad = screen;
		screenLoaded = false;
		fadingOut = false;
	}

	private void FadeIn(){
		FadeToInColor(faderDetails.fadeInColor);
		
		if(Mathf.Abs(faderDetails.fadeImage.color.a - faderDetails.fadeInColor.a) <= 0.01f){
			faderDetails.fadeImage.color = faderDetails.fadeInColor;
			fadingIn = false;
			pauseStart = Time.timeSinceLevelLoad;
		}
	}
	
	private void FadeOut(){
		if(!screenLoaded){
			switch(screenToLoad){
				case "MainMenu":
					StartCoroutine(GUIManager.Instance.SetTargetMenu(GUIManager.TargetMenuOptions.MainMenu));
					break;

				case "OptionsMenu":
					StartCoroutine(GUIManager.Instance.SetTargetMenu(GUIManager.TargetMenuOptions.OptionsMenu));
					break;

				case "LevelSelectMenu":
					StartCoroutine(GUIManager.Instance.SetTargetMenu(GUIManager.TargetMenuOptions.LevelSelect));
					break;

				case "HighScoresMenu":
					StartCoroutine(GUIManager.Instance.SetTargetMenu(GUIManager.TargetMenuOptions.HighScore));
					break;

				default:
					break;
			}

			screenLoaded = true;
		}

		FadeToOutColor();
		
		if(Mathf.Abs(faderDetails.fadeImage.color.a - faderDetails.fadeOutColor.a) <= 0.01f){
			pauseStart = 0f;
			faderDetails.fadeImage.color = faderDetails.fadeOutColor;
			if(faderDetails.transitionType == FaderDetails.TransitionType.SceneChange)
				LevelManager.Instance.ChangeToLevel("MainMenu");
		}
	}
	
	private void FadeToInColor(Color fadeColor){
		faderDetails.fadeImage.color = Color.Lerp(faderDetails.fadeImage.color, fadeColor, faderDetails.fadeInSpeed * Time.deltaTime);
	}
	
	private void FadeToOutColor(){
		faderDetails.fadeImage.color = Color.Lerp(faderDetails.fadeImage.color, faderDetails.fadeOutColor, faderDetails.fadeOutSpeed * Time.deltaTime);
	}
	#endregion
}
