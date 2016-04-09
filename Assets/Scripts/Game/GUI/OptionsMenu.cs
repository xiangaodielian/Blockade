﻿/*----------------------------------/
  OptionsMenu Class - Blockade
  Controls all GUI visible in
  Options Menus (including in-game)
  Writen by Joe Arthur
  Latest Revision - 9 Apr, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	[System.Serializable] private class Buttons{
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

	[System.Serializable] private class Images{
		public Image ballColorImage = null;
	}

	[SerializeField] private Buttons buttons = null;
	[SerializeField] private Sliders sliders = null;
	[SerializeField] private Toggles toggles = null;
	[SerializeField] private Images images = null;
	[Tooltip("Is this an in-game menu?")]
	[SerializeField] private bool inGameMenu = false;

	void Awake(){
		SetListeners();
	}

	void Start(){
		SetSliderValues();
		SetToggleValues();

		if(!inGameMenu)
			SetBallColorImage();
	}

	void Update(){
		if(!OptionsController.instance.SliderCheck(sliders.musicVolumeSlider.value, sliders.sfxVolumeSlider.value))
			SetSliderValues();

		if(toggles.useCursorToggle.isOn != InputManager.instance.useCursorMovement)
			SetToggleValues();
	}

	void SetListeners(){
		if(!inGameMenu){
			buttons.defaultsButton.onClick.AddListener(() => {
				OptionsController.instance.ResetDefaults();
				ResetDefaultValues();
			});
			buttons.backButton.onClick.AddListener(() => {
				UIManager.instance.MenuFadeTransition("MainMenu");
				OptionsController.instance.SaveOptions();
			});
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
		toggles.useCursorToggle.isOn = InputManager.instance.useCursorMovement;
	}

	void SetBallColorImage(){
		Color ballColor = new Color();
		ballColor.r = sliders.ballColorRedSlider.value;
		ballColor.g = sliders.ballColorGreenSlider.value;
		ballColor.b = sliders.ballColorBlueSlider.value;
		ballColor.a = 1f;

		images.ballColorImage.color = ballColor;
	}

	void ResetDefaultValues(){
		sliders.musicVolumeSlider.value = sliders.musicVolumeSlider.maxValue;
		sliders.sfxVolumeSlider.value = sliders.sfxVolumeSlider.maxValue;
		sliders.ballColorRedSlider.value = sliders.ballColorRedSlider.maxValue;
		sliders.ballColorGreenSlider.value = sliders.ballColorGreenSlider.minValue;
		sliders.ballColorBlueSlider.value = sliders.ballColorBlueSlider.minValue;

		SetBallColorImage();
	}

	void SetDescriptiveText(bool useCursor){
		if(useCursor)
			toggles.descriptiveText.text = "USE MOUSE CURSOR TO MOVE";
		else
			toggles.descriptiveText.text = "USE ARROWS OR 'A' AND 'D' KEYS TO MOVE";
	}
}
