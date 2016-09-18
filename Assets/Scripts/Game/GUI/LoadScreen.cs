using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour {

	private const string MessageOne = "LOADING ASSETS...";
    private const string MessageTwo = "POSITIONING BLOCKS...";
    private const string MessageThree = "Aasjhyemfhlkhmal";
    private const string MessageFour = "REMOVING CAT FROM KEYBOARD...";

	[SerializeField] private Slider loadingSlider = null;
	[SerializeField] private Text loadingMessageText = null;
	[SerializeField] private bool isLevelLoadScreen = true;

	void Start(){
		loadingSlider.value = 0f;
	}

	void Update(){
		if(isLevelLoadScreen){
		    if(LevelManager.Instance.asyncOp == null)
                return;

		    loadingSlider.value = LevelManager.Instance.asyncOp.progress;

            if(LevelManager.Instance.asyncOp.isDone){
		        loadingSlider.value = 0f;
		        gameObject.SetActive(false);
		    }
		} else{
			loadingSlider.value = AssetBundleManager.TotalDownloadProgress;
			if(AssetBundleManager.TotalDownloadProgress < 0.5f)
				loadingMessageText.text = MessageOne;
			else if(AssetBundleManager.TotalDownloadProgress < 0.7f)
				loadingMessageText.text = MessageTwo;
			else if(AssetBundleManager.TotalDownloadProgress < 0.8f)
				loadingMessageText.text = MessageThree;
			else if(AssetBundleManager.TotalDownloadProgress < 0.85f)
				loadingMessageText.text = MessageFour;
			else if(Math.Abs(AssetBundleManager.TotalDownloadProgress - 1f) < 0.025f)
				GetComponentInParent<SceneFader>().faderDetails.instantFadeAfterPause = true;
		}
	}
}
