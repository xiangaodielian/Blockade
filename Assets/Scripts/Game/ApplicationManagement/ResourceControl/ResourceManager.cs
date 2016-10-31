using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ApplicationManagement.ResourceControl {
    public static class ResourceManager {

        private static IAssetBundler assetBundler;

        public static void SetAssetBundler(IAssetBundler bundler) {
            if(assetBundler == null)
                assetBundler = bundler;
            else
                throw new InvalidOperationException("IAssetBundler in ResourceManager has already been set...");
        }

        #region Materials & Textures

        //Sets Textures for GUI objects
        public static Sprite SetGuiTexture(string objectName) {
            if(objectName.Contains("Button"))
                objectName = objectName.Replace("Button", "");

            Texture2D guiTex = new Texture2D(0, 0);

            try {
                guiTex = assetBundler.LoadAsset<Texture2D>(objectName, BundleType.Gui);
            } catch(ArgumentException e) {
                string error = string.Format("ERROR in {0} with {1}: {2}", e.Source, e.ParamName, e.Message);
                GameMaster.Logger.LogError(0, error);
            } catch(KeyNotFoundException e) {
                string error = string.Format("ERROR in {0}: {1}", e.Source, e.Message);
                GameMaster.Logger.LogError(0, error);
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
                        mat.mainTexture = assetBundler.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency",
                            BundleType.Textures);
                        mat.SetTexture("_MetalRoughMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_MetalRough", BundleType.Textures));
                        mat.SetTexture("_BumpMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Normal", BundleType.Textures));
                        mat.SetTexture("_HeightMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Height", BundleType.Textures));
                    }
                    //Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
                    //Uses: AlbedoTransparency (RGB-A), MetalRoughMap (RGB-A),
                    //BumpMap (RGB/Normal), and HeightMap (RGB)
                    else if(mat.shader.name == "Custom/Transparent/MetalRoughAlphaBlend") {
                        mat.mainTexture = assetBundler.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency",
                            BundleType.Textures);
                        mat.SetTexture("_MetalRoughMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_MetalRough", BundleType.Textures));
                        mat.SetTexture("_BumpMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Normal", BundleType.Textures));
                        mat.SetTexture("_HeightMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Height", BundleType.Textures));
                    }
                    //Set Textures for Custom/Transparent/MetalRoughAlphaBlend Shader
                    //Uses: BumpMap (RGB/Normal)
                    else if(mat.shader.name == "Custom/Transparent/Glass")
                        mat.SetTexture("_BumpMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Normal", BundleType.Textures));

                    //Set Textures for Custom/Transparent/DissolveTransparent Shader
                    //Uses: DiffuseTex (RGB), BumpMap (RGB/Normal),
                    //and DissolveTex (RGB)
                    else if(mat.shader.name == "Custom/Transparent/DissolveTransparent") {
                        mat.SetTexture("_DiffuseTex",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Diffuse", BundleType.Textures));
                        mat.SetTexture("_BumpMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Normal", BundleType.Textures));
                        mat.SetTexture("_DissolveTex",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Dissolve", BundleType.Textures));
                    }
                    //Set Textures for Unity Standard Shader
                    //Uses: AlbedoTransparency (RGB), MetallicGlossMap (RGBA),
                    //BumpMap (RGB/Normal), and ParallaxMap (RGB)
                    else if(mat.shader.name == "Standard") {
                        mat.mainTexture = assetBundler.LoadAsset<Texture2D>(texPath + "_AlbedoTransparency",
                            BundleType.Textures);
                        mat.SetTexture("_MetallicGlossMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_MetalRough", BundleType.Textures));
                        mat.SetTexture("_BumpMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Normal", BundleType.Textures));
                        mat.SetTexture("_ParallaxMap",
                            assetBundler.LoadAsset<Texture2D>(texPath + "_Height", BundleType.Textures));
                    }
                } catch(ArgumentException e) {
                    string error = string.Format("ERROR in {0} with {1}: {2}", e.Source, e.ParamName, e.Message);
                    GameMaster.Logger.LogError(0, error);
                } catch(KeyNotFoundException e) {
                    string error = string.Format("ERROR in {0}: {1}", e.Source, e.Message);
                    GameMaster.Logger.LogError(0, error);
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
                audioClip = assetBundler.LoadAsset<AudioClip>(track, BundleType.Audio);
            } catch(ArgumentException e) {
                string error = string.Format("ERROR in {0} with {1}: {2}", e.Source, e.ParamName, e.Message);
                GameMaster.Logger.LogError(0, error);
            } catch(KeyNotFoundException e) {
                string error = string.Format("ERROR in {0}: {1}", e.Source, e.Message);
                GameMaster.Logger.LogError(0, error);
            }

            return audioClip;
        }

        #endregion

        #region Prefab

        public static GameObject LoadPrefab(string bundleName, string assetName) {
            GameObject asset = null;

            if(bundleName == "gui")
                asset = assetBundler.LoadAsset<GameObject>(assetName, BundleType.Gui);

            return asset;
        }

        #endregion

        public static IEnumerator UnloadUnusedResources() {
            yield return Resources.UnloadUnusedAssets();

            try {
                assetBundler.UnloadUnusedBundles();
            } catch(KeyNotFoundException e) {
                string error = string.Format("ERROR in {0}: {1}", e.Source, e.Message);
                GameMaster.Logger.LogError(0, error);
            }
        }
    }
}
