/*----------------------------------/
  TutorialCollider Class - Blockade
  Controls TutorialCollider and 
  calls to display Tutorials
  Writen by Joe Arthur
  Latest Revision - 11 Apr, 2016
/----------------------------------*/

using UnityEngine;

public class TutorialCollider : MonoBehaviour {

	private bool triggerTutorial = true;

	void OnTriggerEnter(Collider col){
		if(col.tag == "Powerup" && triggerTutorial && PrefsManager.GetLevelUnlocked() <= PrefsManager.GetCurrentLevel()){
			if(col.GetComponent<Powerup>().powerupType == Powerup.PowerupType.SpeedUp)
				GUIManager.instance.inGameGUI.ToggleInGameTutorials(true, 1);
			else
				GUIManager.instance.inGameGUI.ToggleInGameTutorials(true, 2);

			triggerTutorial = false;
		}
	}
}
