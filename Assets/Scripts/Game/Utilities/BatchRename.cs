using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
#if UNITY_EDITOR
public class BatchRename : MonoBehaviour {

	private GameObject[] allObjs = null;
	private GameObject[] renamedObjs = null;
	private int totalObjsToName = 0;
	
	public void Rename(string baseName){
		totalObjsToName = 0;
		allObjs = FindObjectsOfType<GameObject>();
		for(int i=0;i<allObjs.Length;i++){
			if(allObjs[i].name.Contains(baseName) && allObjs[i].gameObject != this.gameObject){
				totalObjsToName++;
			}
		}
		
		renamedObjs = new GameObject[totalObjsToName];
		int renameIndex = 0;
		
		for(int i=0;i<allObjs.Length;i++){
			if(allObjs[i].name.Contains(baseName) && allObjs[i].gameObject != this.gameObject){
				renamedObjs[renameIndex] = allObjs[i];
				renameIndex++;
			}
		}
		
		for(int i=0;i<renamedObjs.Length;i++){
			renamedObjs[i].name = baseName+" ("+(i+1).ToString()+")";
		}
	}
}
#endif
