using UnityEngine;
using ApplicationManagement;

public class TutorialCollider : MonoBehaviour {

	private bool triggerTutorial = true;

	void OnTriggerEnter(Collider col){
		if(col.tag == "Powerup" && triggerTutorial && PrefsManager.GetLevelUnlocked() <= PrefsManager.GetCurrentLevel()){
			if(col.GetComponent<Powerup>().powerupType == Powerup.PowerupType.SpeedUp)
				GUIManager.Instance.InGameGui.ToggleInGameTutorials(true, 1);
			else
				GUIManager.Instance.InGameGui.ToggleInGameTutorials(true, 2);

			triggerTutorial = false;
		}
	}
}
