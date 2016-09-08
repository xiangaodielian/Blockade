/*-----------------------------------/
  LoadScreen Class - Blockade
  Controls Loading Bar and Text for
  Splash Screen loading
  Writen by Joe Arthur
  Latest Revision - 3 Apr, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour {

	const string MESSAGE_ONE = "LOADING ASSETS...";
	const string MESSAGE_TWO = "POSITIONING BLOCKS...";
	const string MESSAGE_THREE = "Aasjhyemfhlkhmal";
	const string MESSAGE_FOUR = "REMOVING CAT FROM KEYBOARD...";

	[SerializeField] private Slider loadingSlider = null;
	[SerializeField] private Text loadingMessageText = null;
	[SerializeField] private bool isLevelLoadScreen = true;

	void Start(){
		loadingSlider.value = 0f;
	}

	void Update(){
		if(isLevelLoadScreen){
			if(LevelManager.instance.asyncOp != null){
				loadingSlider.value = LevelManager.instance.asyncOp.progress;
				if(LevelManager.instance.asyncOp.isDone){
					loadingSlider.value = 0f;
					gameObject.SetActive(false);
				}
			}
		} else{
			if(AssetBundleManager.totalDownloadProgress > 0f){
				loadingSlider.value = AssetBundleManager.totalDownloadProgress;
				if(AssetBundleManager.totalDownloadProgress < 0.5f)
					loadingMessageText.text = MESSAGE_ONE;
				else if(AssetBundleManager.totalDownloadProgress < 0.7f)
					loadingMessageText.text = MESSAGE_TWO;
				else if(AssetBundleManager.totalDownloadProgress < 0.8f)
					loadingMessageText.text = MESSAGE_THREE;
				else if(AssetBundleManager.totalDownloadProgress < 0.85f)
					loadingMessageText.text = MESSAGE_FOUR;
				else if(AssetBundleManager.totalDownloadProgress == 1f)
					GetComponentInParent<SceneFader>().faderDetails.instantFadeAfterPause = true;
			}
		}
	}
}
