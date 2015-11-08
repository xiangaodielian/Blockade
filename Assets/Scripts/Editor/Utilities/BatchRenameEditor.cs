using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BatchRename))]
public class NaiveTerrainEditor : Editor {
	string baseName = "";
	
	public override void OnInspectorGUI (){
		DrawDefaultInspector();
		
		BatchRename br = (BatchRename)target;
		baseName = EditorGUILayout.TextField("Object Base Name: ", baseName);
		if(GUILayout.Button("Rename")){
			br.Rename(baseName);
		}
	}
}
