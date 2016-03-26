using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour {

	[SerializeField] private Slider loadingSlider = null;

	void Start(){
		loadingSlider.value = 0f;
	}

	void Update(){
		if(GameMaster.instance.async != null){
			loadingSlider.value = GameMaster.instance.async.progress;
			if(GameMaster.instance.async.isDone){
				loadingSlider.value = 0f;
				gameObject.SetActive(false);
			}
		}
	}
}
