using UnityEngine;

public class TutorialCollider : MonoBehaviour {

	private bool triggerTutorial = true;

	void OnTriggerEnter(Collider col){
		if(col.tag == "Powerup" && triggerTutorial && PrefsManager.GetLevelUnlocked() <= PrefsManager.GetCurrentLevel()){
			if(col.GetComponent<Powerup>().powerupType == Powerup.PowerupType.SpeedUp)
				InGameUI.instance.ToggleInGameTutorials(true, 1);
			else
				InGameUI.instance.ToggleInGameTutorials(true, 2);

			triggerTutorial = false;
		}
	}
}
