using UnityEngine;
using UnityEditor;

namespace ApplicationManagement.ResourceControl {
    [CustomEditor(typeof(AssetBundleManager))]
    public class AssetBundleManagerEditor : Editor {

        [MenuItem("Assets/AssetBundles/Build WebGL")]
        static void BuildWebGLAssetBundles() {
            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles"))
                AssetDatabase.CreateFolder("Assets", "AssetBundles");

            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/WebGL"))
                AssetDatabase.CreateFolder("Assets/AssetBundles", "WebGL");

            BuildPipeline.BuildAssetBundles("Assets/AssetBundles/WebGL", BuildAssetBundleOptions.None, BuildTarget.WebGL);
        }

        [MenuItem("Assets/AssetBundles/Build Standalone/Windows_x86")]
        static void BuildWinx86AssetBundles() {
            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles"))
                AssetDatabase.CreateFolder("Assets", "AssetBundles");

            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone"))
                AssetDatabase.CreateFolder("Assets/AssetBundles", "Standalone");

            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone/Win_x86"))
                AssetDatabase.CreateFolder("Assets/AssetBundles/Standalone", "Win_x86");

            BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Standalone/Win_x86", BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows);
        }

        [MenuItem("Assets/AssetBundles/Build Standalone/OSX")]
        static void BuildOSXAssetBundles() {
            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles"))
                AssetDatabase.CreateFolder("Assets", "AssetBundles");

            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone"))
                AssetDatabase.CreateFolder("Assets/AssetBundles", "Standalone");

            if(!AssetDatabase.IsValidFolder("Assets/AssetBundles/Standalone/OSX"))
                AssetDatabase.CreateFolder("Assets/AssetBundles/Standalone", "OSX");

            BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Standalone/OSX", BuildAssetBundleOptions.None,
                BuildTarget.StandaloneOSXUniversal);
        }

        [MenuItem("Assets/AssetBundles/Get AssetBundle Names")]
        static void GetBundleNames() {
            string[] names = AssetDatabase.GetAllAssetBundleNames();

            foreach(string name in names)
                Debug.Log("AssetBundle: " + name);
        }

        [MenuItem("Assets/AssetBundles/Get Bundle Contents")]
        static void GetBundleContents() {
            string[] names = AssetDatabase.GetAllAssetBundleNames();

            foreach(string name in names) {
                string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(name);
                Debug.Log("AssetBundle: " + name);
                foreach(string path in paths)
                    Debug.Log("Asset Path: " + path);
            }
        }
    }
}