/*-----------------------------------/
  SceneFader Class - Blockade
  Controls Fade Transitions between
  scenes and Menu Screens
  Writen by Joe Arthur
  Latest Revision - 27 Mar, 2016
/----------------------------------*/

using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {
	
	#region Variables
	
	[System.Serializable] private class FaderDetails{
		public enum TransitionType {SceneChange, MenuChange};
		[Tooltip("Dictates behavior after fade. Scene Change loads a new Scene, Menu Change loads a new Menu.")]
		public TransitionType transitionType = TransitionType.SceneChange;

		[Tooltip("Color to fade Image to.")]
		public Color fadeInColor = Color.white;
		public float fadeInSpeed = 1f;
		public float fadeOutSpeed = 1f;
		public float pauseDuration = 1f;
		[Tooltip("Image to fade.")]
		public Image fadeImage = null;
		[Tooltip("Start image fade instantly on Scene Load.")]
		public bool fadeInInstantly = false;
	}

	[SerializeField] private FaderDetails faderDetails = null;
	
	private bool fadingIn;
	private float pauseStart = 0f;
	private string screenToLoad = "";
	private bool screenLoaded = false;
	
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
	
	public void StartFade(string screen){
		fadingIn = true;
		screenToLoad = screen;
		screenLoaded = false;
	}

	private void FadeIn(){
		FadeToColor(faderDetails.fadeInColor);
		
		if(faderDetails.fadeImage.color.a >= 0.95f){
			faderDetails.fadeImage.color = faderDetails.fadeInColor;
			fadingIn = false;
			pauseStart = Time.timeSinceLevelLoad;
		}
	}
	
	private void FadeOut(){
		if(!screenLoaded){
			switch(screenToLoad){
				case "MainMenu":
					UIManager.instance.OpenMainMenu();
					break;

				case "OptionsMenu":
					UIManager.instance.OpenOptionsMenu();
					break;

				case "LevelSelectMenu":
					UIManager.instance.OpenLevelSelectMenu();
					break;

				case "HighScoresMenu":
					UIManager.instance.OpenHighScoreMenu();
					break;

				default:
					break;
			}

			screenLoaded = true;
		}

		FadeToClear();
		
		if(faderDetails.fadeImage.color.a <= 0.05f){
			pauseStart = 0f;
			faderDetails.fadeImage.color = Color.clear;
			if(faderDetails.transitionType == FaderDetails.TransitionType.SceneChange)
				GameMaster.instance.ChangeToLevel("MainMenu");
		}
	}
	
	private void FadeToColor(Color fadeColor){
		faderDetails.fadeImage.color = Color.Lerp(faderDetails.fadeImage.color, fadeColor, faderDetails.fadeInSpeed * Time.deltaTime);
	}
	
	private void FadeToClear(){
		faderDetails.fadeImage.color = Color.Lerp(faderDetails.fadeImage.color, Color.clear, faderDetails.fadeOutSpeed * Time.deltaTime);
	}
	
	#endregion
}
