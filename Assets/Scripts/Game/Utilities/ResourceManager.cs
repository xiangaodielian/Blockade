/*----------------------------------/
  ResourceManager Class - Universal
  Manages the loading of Resources
  including setting Textures, Audio,
  Meshes, etc.
  Writen by Joe Arthur
  Latest Revision - 2 Apr, 2016
/----------------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ResourceManager{

	#region Materials & Textures

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
				mat.mainTexture = AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency");
				mat.SetTexture("_MetalRoughMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_MetalRough"));
				mat.SetTexture("_BumpMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_HeightMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Height"));
			}
			//Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
			//Uses: AlbedoTransparency (RGB-A), MetalRoughMap (RGB-A),
			//BumpMap (RGB/Normal), and HeightMap (RGB)
			else if(mat.shader.name == "Custom/Transparent/MetalRoughAlphaBlend"){
				mat.mainTexture =  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency");
				mat.SetTexture("_MetalRoughMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_MetalRough"));
				mat.SetTexture("_BumpMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_HeightMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Height"));
			}
			//Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
			//Uses: BumpMap (RGB/Normal)
			else if(mat.shader.name == "Custom/Transparent/Glass"){
				mat.SetTexture("_BumpMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Normal"));
			}
			//Set Textures for Custom/Transparent/DissolveTransparent Shader
			//Uses: DiffuseTex (RGB), BumpMap (RGB/Normal),
			//and DissolveTex (RGB)
			else if(mat.shader.name == "Custom/Transparent/DissolveTransparent"){
				mat.SetTexture("_DiffuseTex",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Diffuse"));
				mat.SetTexture("_BumpMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_DissolveTex",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Dissolve"));
			}
			//Set Textures for Unity Standard Shader
			//Uses: AlbedoTransparency (RGB), MetallicGlossMap (RGBA),
			//BumpMap (RGB/Normal), and ParallaxMap (RGB)
			else if(mat.shader.name == "Standard"){
				mat.mainTexture =  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency");
				mat.SetTexture("_MetallicGlossMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_MetalRough"));
				mat.SetTexture("_BumpMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Normal"));
				mat.SetTexture("_ParallaxMap",  AssetBundleManager.instance.LoadAsset<Texture2D>(texPath + "_Height"));
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

	public static AudioClip LoadAudioClip(bool music, string track){
		AudioClip clip = null;

		if(music)
			clip =  AssetBundleManager.instance.LoadAsset<AudioClip>(track);
		else
			clip =  AssetBundleManager.instance.LoadAsset<AudioClip>(track);

		return clip;
	}

	#endregion

	public static IEnumerator UnloadAll(){
		yield return Resources.UnloadUnusedAssets();
	}
}
