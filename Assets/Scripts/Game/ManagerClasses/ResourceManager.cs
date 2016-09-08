﻿/*----------------------------------/
  ResourceManager Class - Universal
  Manages the loading of Resources
  including setting Textures, Audio,
  Meshes, etc.
  Writen by Joe Arthur
  Latest Revision - 8 May, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ResourceManager{

	#region Materials & Textures

	//Sets Textures for GUI objects
	public static Sprite SetGUITexture(string objectName){
		if(objectName.Contains("Button"))
			objectName = objectName.Replace("Button", "");

		Texture2D guiTex = GameMaster.GMInstance.abManager.LoadGUIAsset<Texture2D>(objectName);

		return Sprite.Create(guiTex, new Rect(0f, 0f, guiTex.width, guiTex.height), new Vector2(0.5f, 0.5f));
	}

	//Sets Textures for obj Materials
	//Textures must follow same naming scheme as Material
	public static void SetMaterialTextures(GameObject obj){
		List<Material> usedMats = GetMaterialList(obj);

		foreach(Material mat in usedMats){
			//Path to stored Textures
			string texPath = "";

			//Get rid of (Instance) if present
			if(mat.name.Contains(" (Instance)"))
				texPath += mat.name.Replace(" (Instance)", "");
			else
				texPath += mat.name;

			//Set Textures for Custom/PBR/MetalRough Shader
			//Uses: AlbedoTransparency (RGB), MetalRoughMap (RGB-A),
			//BumpMap (RGB/Normal), and HeightMap (RGB)
			if(mat.shader.name == "Custom/PBR/MetalRough"){
				mat.mainTexture = GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency");
				mat.SetTexture("_MetalRoughMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_MetalRough"));
				mat.SetTexture("_BumpMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_HeightMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Height"));
			}
			//Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
			//Uses: AlbedoTransparency (RGB-A), MetalRoughMap (RGB-A),
			//BumpMap (RGB/Normal), and HeightMap (RGB)
			else if(mat.shader.name == "Custom/Transparent/MetalRoughAlphaBlend"){
				mat.mainTexture =  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency");
				mat.SetTexture("_MetalRoughMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_MetalRough"));
				mat.SetTexture("_BumpMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_HeightMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Height"));
			}
			//Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
			//Uses: BumpMap (RGB/Normal)
			else if(mat.shader.name == "Custom/Transparent/Glass"){
				mat.SetTexture("_BumpMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Normal"));
			}
			//Set Textures for Custom/Transparent/DissolveTransparent Shader
			//Uses: DiffuseTex (RGB), BumpMap (RGB/Normal),
			//and DissolveTex (RGB)
			else if(mat.shader.name == "Custom/Transparent/DissolveTransparent"){
				mat.SetTexture("_DiffuseTex",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Diffuse"));
				mat.SetTexture("_BumpMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_DissolveTex",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Dissolve"));
			}
			//Set Textures for Unity Standard Shader
			//Uses: AlbedoTransparency (RGB), MetallicGlossMap (RGBA),
			//BumpMap (RGB/Normal), and ParallaxMap (RGB)
			else if(mat.shader.name == "Standard"){
				mat.mainTexture =  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency");
				mat.SetTexture("_MetallicGlossMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_MetalRough"));
				mat.SetTexture("_BumpMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_ParallaxMap",  GameMaster.GMInstance.abManager.LoadAsset<Texture2D>(texPath + "_Height"));
			}
		}
	}

	//Gets a List of all Materials used on the GameObject (including in children)
	private static List<Material> GetMaterialList(GameObject obj){
		MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
		List<Material> mats = new List<Material>();

		foreach(MeshRenderer rend in renderers){
			if(rend.materials.Length > 1){
				for(int i=0; i<rend.materials.Length; i++)
					mats.Add(rend.materials[i]);
			} else
				mats.Add(rend.material);
		}

		return mats;
	}

	#endregion
	#region Audio

	public static AudioClip LoadAudioClip(string track){
		return GameMaster.GMInstance.abManager.LoadAsset<AudioClip>(track);
	}

	#endregion
	#region Prefab

	public static GameObject LoadPrefab(string bundleName, string assetName){
		GameObject asset = null;

		if(bundleName == "gui")
			asset = GameMaster.GMInstance.abManager.LoadGUIAsset<GameObject>(assetName);

		return asset;
	}

	#endregion

	public static IEnumerator UnloadUnusedResources(){
		yield return Resources.UnloadUnusedAssets();

		GameMaster.GMInstance.abManager.UnloadUnusedBundles();
	}
}
