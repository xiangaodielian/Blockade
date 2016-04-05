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
			if(GameMaster.instance.async != null){
				loadingSlider.value = GameMaster.instance.async.progress;
				if(GameMaster.instance.async.isDone){
					loadingSlider.value = 0f;
					gameObject.SetActive(false);
				}
			}
		} else{
			if(AssetBundleManager.instance.totalDownloadProgress > 0f){
				loadingSlider.value = AssetBundleManager.instance.totalDownloadProgress;
				if(AssetBundleManager.instance.totalDownloadProgress < 0.5f)
					loadingMessageText.text = MESSAGE_ONE;
				else if(AssetBundleManager.instance.totalDownloadProgress < 0.7f)
					loadingMessageText.text = MESSAGE_TWO;
				else if(AssetBundleManager.instance.totalDownloadProgress < 0.8f)
					loadingMessageText.text = MESSAGE_THREE;
				else if(AssetBundleManager.instance.totalDownloadProgress < 0.85f)
					loadingMessageText.text = MESSAGE_FOUR;
				else if(AssetBundleManager.instance.totalDownloadProgress == 1f)
					GetComponentInParent<SceneFader>().faderDetails.instantFadeAfterPause = true;
			}
		}
	}
}
