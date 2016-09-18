using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ResourceManager {

    #region Materials & Textures

    //Sets Textures for GUI objects
    public static Sprite SetGuiTexture(string objectName) {
        if(objectName.Contains("Button"))
            objectName = objectName.Replace("Button", "");

        Texture2D guiTex = new Texture2D(0, 0);

        try {
            guiTex = AssetBundleManager.Instance.LoadAsset<Texture2D>(objectName, AssetBundleManager.BundleType.Gui);
        } catch(ArgumentException e) {
            Debug.LogError(string.Format("ERROR in {0} with {1}: {2}", e.Source, e.ParamName, e.Message));
        } catch(KeyNotFoundException e) {
            Debug.LogError(string.Format("ERROR in {0}: {1}", e.Source, e.Message));
        }

        return Sprite.Create(guiTex, new Rect(0f, 0f, guiTex.width, guiTex.height), new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// Sets Textures for obj Materials. Textures must follow same naming scheme as Material.
    /// </summary>
    public static void SetMaterialTextures(GameObject obj) {
        IEnumerable<Material> usedMats = GetMaterialList(obj);

        foreach(Material mat in usedMats) {
            string texPath = "";

            if(mat.name.Contains(" (Instance)"))
                texPath += mat.name.Replace(" (Instance)", "");
            else
                texPath += mat.name;

            try {
                //Set Textures for Custom/PBR/MetalRough Shader
                //Uses: AlbedoTransparency (RGB), MetalRoughMap (RGB-A),
                //BumpMap (RGB/Normal), and HeightMap (RGB)
                if(mat.shader.name == "Custom/PBR/MetalRough") {
                    mat.mainTexture = AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency", AssetBundleManager.BundleType.Textures);
                    mat.SetTexture("_MetalRoughMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_MetalRough", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_BumpMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Normal", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_HeightMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Height", AssetBundleManager.BundleType.Textures));
                }
                //Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
                //Uses: AlbedoTransparency (RGB-A), MetalRoughMap (RGB-A),
                //BumpMap (RGB/Normal), and HeightMap (RGB)
                else if(mat.shader.name == "Custom/Transparent/MetalRoughAlphaBlend") {
                    mat.mainTexture = AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency", AssetBundleManager.BundleType.Textures);
                    mat.SetTexture("_MetalRoughMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_MetalRough", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_BumpMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Normal", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_HeightMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Height", AssetBundleManager.BundleType.Textures));
                }
                //Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
                //Uses: BumpMap (RGB/Normal)
                else if(mat.shader.name == "Custom/Transparent/Glass") 
                    mat.SetTexture("_BumpMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Normal", AssetBundleManager.BundleType.Textures));

                //Set Textures for Custom/Transparent/DissolveTransparent Shader
                //Uses: DiffuseTex (RGB), BumpMap (RGB/Normal),
                //and DissolveTex (RGB)
                else if(mat.shader.name == "Custom/Transparent/DissolveTransparent") {
                    mat.SetTexture("_DiffuseTex", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Diffuse", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_BumpMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Normal", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_DissolveTex", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Dissolve", AssetBundleManager.BundleType.Textures));
                }
                //Set Textures for Unity Standard Shader
                //Uses: AlbedoTransparency (RGB), MetallicGlossMap (RGBA),
                //BumpMap (RGB/Normal), and ParallaxMap (RGB)
                else if(mat.shader.name == "Standard") {
                    mat.mainTexture = AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency", AssetBundleManager.BundleType.Textures);
                    mat.SetTexture("_MetallicGlossMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_MetalRough", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_BumpMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Normal", AssetBundleManager.BundleType.Textures));
                    mat.SetTexture("_ParallaxMap", AssetBundleManager.Instance.LoadAsset<Texture2D>(texPath + "_Height", AssetBundleManager.BundleType.Textures));
                }
            } catch(ArgumentException e) {
                Debug.LogError(string.Format("ERROR in {0} with {1}: {2}", e.Source, e.ParamName, e.Message));
            } catch(KeyNotFoundException e) {
                Debug.LogError(string.Format("ERROR in {0}: {1}", e.Source, e.Message));
            }
        }
    }

    //Gets a List of all Materials used on the GameObject (including in children)
    private static IEnumerable<Material> GetMaterialList(GameObject obj) {
        MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
        List<Material> mats = new List<Material>();

        foreach(MeshRenderer rend in renderers) {
            if(rend.materials.Length > 1) {
                foreach(Material mat in rend.materials)
                    mats.Add(mat);
            } else
                mats.Add(rend.material);
        }

        return mats;
    }

    #endregion

    #region Audio

    public static AudioClip LoadAudioClip(string track) {
        AudioClip audioClip = new AudioClip();

        try {
            audioClip = AssetBundleManager.Instance.LoadAsset<AudioClip>(track, AssetBundleManager.BundleType.Audio);
        } catch(ArgumentException e) {
            Debug.LogError(string.Format("ERROR in {0} with {1}: {2}", e.Source, e.ParamName, e.Message));
        } catch(KeyNotFoundException e) {
            Debug.LogError(string.Format("ERROR in {0}: {1}", e.Source, e.Message));
        }

        return audioClip;
    }

    #endregion

    #region Prefab

    public static GameObject LoadPrefab(string bundleName, string assetName) {
        GameObject asset = null;

        if(bundleName == "gui")
            asset = AssetBundleManager.Instance.LoadAsset<GameObject>(assetName, AssetBundleManager.BundleType.Gui);

        return asset;
    }

    #endregion

    public static IEnumerator UnloadUnusedResources() {
        yield return Resources.UnloadUnusedAssets();

        try {
            AssetBundleManager.Instance.UnloadUnusedBundles();
        } catch(KeyNotFoundException e) {
            Debug.LogError(string.Format("ERROR in {0}: {1}", e.Source, e.Message));
        }
	}
}
