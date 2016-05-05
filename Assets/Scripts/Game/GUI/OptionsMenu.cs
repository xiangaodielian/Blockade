/*----------------------------------/
  OptionsMenu Class - Blockade
  Controls all GUI visible in
  Options Menus (including in-game)
  Writen by Joe Arthur
  Latest Revision - 4 May, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	#region Variables

	[System.Serializable] private class Panels{
		public GameObject gameplayPanel = null;
		public GameObject videoPanel = null;
		public GameObject audioPanel = null;
	}

	[System.Serializable] private class Buttons{
		public Button gameplayButton = null;
		public Button videoButton = null;
		public Button audioButton = null;
		public Button defaultsButton = null;
		public Button backButton = null;
	}

	[System.Serializable] private class Sliders{
		public Slider musicVolumeSlider = null;
		public Slider sfxVolumeSlider = null;
		public Slider ballColorRedSlider = null;
		public Slider ballColorGreenSlider = null;
		public Slider ballColorBlueSlider = null;
	}

	[System.Serializable] private class Toggles{
		public Toggle useCursorToggle = null;
		public Text descriptiveText = null;
	}

	[System.Serializable] private class Dropdowns{
		public Dropdown textureResDropdown = null;
	}

	[System.Serializable] private class Images{
		public Image ballColorImage = null;
	}

	[SerializeField] private Panels panels = null;
	[SerializeField] private Buttons buttons = null;
	[SerializeField] private Sliders sliders = null;
	[SerializeField] private Toggles toggles = null;
	[SerializeField] private Dropdowns dropdowns = null;
	[SerializeField] private Images images = null;
	[Tooltip("Is this an in-game menu?")]
	[SerializeField] private bool inGameMenu = false;

	private int curPanelFocus = 0;
	private float panelOffset = 0f;

	#endregion
	#region Mono Functions

	void Awake(){
		SetListeners();
	}

	void Start(){
		if(!inGameMenu)
			SetPanelPositions();

		SetSliderValues();
		SetToggleValues();

		if(!inGameMenu)
			SetBallColorImage();
	}

	void Update(){
		if(!OptionsController.instance.SliderCheck(sliders.musicVolumeSlider.value, sliders.sfxVolumeSlider.value))
			SetSliderValues();

		if(toggles.useCursorToggle.isOn != PrefsManager.GetMouseControl())
			SetToggleValues();

		if(inGameMenu && dropdowns.textureResDropdown.value != PrefsManager.GetTextureRes())
			dropdowns.textureResDropdown.value = PrefsManager.GetTextureRes();

		CheckPanelFocus();
	}

	#endregion
	#region Utility

	void SetListeners(){
		if(!inGameMenu){
			buttons.defaultsButton.onClick.AddListener(() => {
				OptionsController.instance.ResetDefaults();
				ResetDefaultValues();
			});
			buttons.backButton.onClick.AddListener(() => {
				curPanelFocus = 0;
				UIManager.instance.MenuFadeTransition("MainMenu");
				OptionsController.instance.SaveOptions();
			});
			buttons.gameplayButton.onClick.AddListener(() => curPanelFocus = 0);
			buttons.videoButton.onClick.AddListener(() => curPanelFocus = 1);
			buttons.audioButton.onClick.AddListener(() => curPanelFocus = 2);

			sliders.ballColorRedSlider.onValueChanged.AddListener(value => {
				OptionsController.instance.SetBallColor(0, value);
				SetBallColorImage();
			});
			sliders.ballColorGreenSlider.onValueChanged.AddListener(value => {
				OptionsController.instance.SetBallColor(1, value);
				SetBallColorImage();
			});
			sliders.ballColorBlueSlider.onValueChanged.AddListener(value => {
				OptionsController.instance.SetBallColor(2, value);
				SetBallColorImage();
			});

			dropdowns.textureResDropdown.onValueChanged.AddListener(value => OptionsController.instance.SetTextureRes(value));
		} else
			buttons.backButton.onClick.AddListener(() => {
				InGameUI.instance.InGameMain();
				OptionsController.instance.SaveOptions();
			});

		sliders.musicVolumeSlider.onValueChanged.AddListener(value => OptionsController.instance.SetMusicVolume(value));
		sliders.sfxVolumeSlider.onValueChanged.AddListener(value => OptionsController.instance.SetSFXVolume(value));
		toggles.useCursorToggle.onValueChanged.AddListener(value => { OptionsController.instance.SetUseCursor(value);
																	  SetDescriptiveText(value);}); 
	}

	void SetPanelPositions(){
		panels.gameplayPanel.transform.localPosition = new Vector3(0f, 30f, 0f);
		panels.videoPanel.transform.localPosition = panels.gameplayPanel.transform.localPosition + 
													new Vector3(panels.gameplayPanel.GetComponent<RectTransform>().rect.width,0f,0f);
		panels.audioPanel.transform.localPosition = panels.videoPanel.transform.localPosition + 
													new Vector3(panels.videoPanel.GetComponent<RectTransform>().rect.width,0f,0f);

		panelOffset = panels.videoPanel.transform.localPosition.x;
	}

	void SetSliderValues(){
		sliders.musicVolumeSlider.value = PrefsManager.GetMasterMusicVolume();
		sliders.sfxVolumeSlider.value = PrefsManager.GetMasterSFXVolume();

		if(!inGameMenu){
			sliders.ballColorRedSlider.value = PrefsManager.GetBallColor().r;
			sliders.ballColorGreenSlider.value = PrefsManager.GetBallColor().g;
			sliders.ballColorBlueSlider.value = PrefsManager.GetBallColor().b;
		}
	}

	void SetToggleValues(){
		toggles.useCursorToggle.isOn = PrefsManager.GetMouseControl();
	}

	void SetBallColorImage(){
		Color ballColor = new Color();
		ballColor.r = sliders.ballColorRedSlider.value;
		ballColor.g = sliders.ballColorGreenSlider.value;
		ballColor.b = sliders.ballColorBlueSlider.value;
		ballColor.a = 1f;

		images.ballColorImage.color = ballColor;
	}

	//Moves Panels if Focus has changed
	void CheckPanelFocus(){
		float panelXPos = 0f;

		switch(curPanelFocus){
			//Gameplay Focus
			case 0:
				panelXPos = panels.gameplayPanel.transform.localPosition.x;
				if(panelXPos != 0f){
					panelXPos = Mathf.Lerp(panelXPos, 0f, Time.deltaTime * 3f);
					panels.gameplayPanel.transform.localPosition = new Vector3(panelXPos, panels.gameplayPanel.transform.localPosition.y, 0f);

					panelXPos = panels.videoPanel.transform.localPosition.x;
					panelXPos = Mathf.Lerp(panelXPos, panelOffset, Time.deltaTime * 3f);
					panels.videoPanel.transform.localPosition = new Vector3(panelXPos, panels.videoPanel.transform.localPosition.y, 0f);

					panelXPos = panels.audioPanel.transform.localPosition.x;
					panelXPos = Mathf.Lerp(panelXPos, panelOffset * 2f, Time.deltaTime * 3f);
					panels.audioPanel.transform.localPosition = new Vector3(panelXPos, panels.audioPanel.transform.localPosition.y, 0f);
				}
				break;

			//Video Focus
			case 1:
				panelXPos = panels.videoPanel.transform.localPosition.x;
				if(panelXPos != 0f){
					panelXPos = Mathf.Lerp(panelXPos, 0f, Time.deltaTime * 3f);
					panels.videoPanel.transform.localPosition = new Vector3(panelXPos, panels.videoPanel.transform.localPosition.y, 0f);

					panelXPos = panels.gameplayPanel.transform.localPosition.x;
					panelXPos = Mathf.Lerp(panelXPos, panelOffset * -1f, Time.deltaTime * 3f);
					panels.gameplayPanel.transform.localPosition = new Vector3(panelXPos, panels.gameplayPanel.transform.localPosition.y, 0f);

					panelXPos = panels.audioPanel.transform.localPosition.x;
					panelXPos = Mathf.Lerp(panelXPos, panelOffset, Time.deltaTime * 3f);
					panels.audioPanel.transform.localPosition = new Vector3(panelXPos, panels.audioPanel.transform.localPosition.y, 0f);
				}
				break;

			//Audio Focus
			case 2:
				panelXPos = panels.audioPanel.transform.localPosition.x;
				if(panelXPos != 0f){
					panelXPos = Mathf.Lerp(panelXPos, 0f, Time.deltaTime * 3f);
					panels.audioPanel.transform.localPosition = new Vector3(panelXPos, panels.audioPanel.transform.localPosition.y, 0f);

					panelXPos = panels.gameplayPanel.transform.localPosition.x;
					panelXPos = Mathf.Lerp(panelXPos, panelOffset * -2f, Time.deltaTime * 3f);
					panels.gameplayPanel.transform.localPosition = new Vector3(panelXPos, panels.gameplayPanel.transform.localPosition.y, 0f);

					panelXPos = panels.videoPanel.transform.localPosition.x;
					panelXPos = Mathf.Lerp(panelXPos, panelOffset * -1f, Time.deltaTime * 3f);
					panels.videoPanel.transform.localPosition = new Vector3(panelXPos, panels.videoPanel.transform.localPosition.y, 0f);
				}
				break;

			//Invalid
			default:
				Debug.LogError("Invalid Options Menu Panel!");
				break;
		}
	}

	void ResetDefaultValues(){
		sliders.musicVolumeSlider.value = sliders.musicVolumeSlider.maxValue;
		sliders.sfxVolumeSlider.value = sliders.sfxVolumeSlider.maxValue;
		sliders.ballColorRedSlider.value = sliders.ballColorRedSlider.maxValue;
		sliders.ballColorGreenSlider.value = sliders.ballColorGreenSlider.minValue;
		sliders.ballColorBlueSlider.value = sliders.ballColorBlueSlider.minValue;

		toggles.useCursorToggle.isOn = false;
		SetDescriptiveText(false);

		SetBallColorImage();
	}

	void SetDescriptiveText(bool useCursor){
		if(useCursor)
			toggles.descriptiveText.text = "USE MOUSE CURSOR TO MOVE";
		else
			toggles.descriptiveText.text = "USE ARROWS OR 'A' AND 'D' KEYS TO MOVE";
	}

	#endregion
}
