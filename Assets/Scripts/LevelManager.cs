using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public int breakableCount = 0;
	
	public void LoadLevel(string name){
		Application.LoadLevel(name);
		breakableCount = 0;
	}
	
	public void QuitRequest(){
		Application.Quit();
	}
	
	public void LoadNextLevel(){
		Application.LoadLevel(Application.loadedLevel+1);
		breakableCount = 0;
	}
	
	public void BrickDestroyed(){
		breakableCount--;
		if(breakableCount <= 0)
			LoadNextLevel();
	}
}
