namespace ApplicationManagement.ResourceControl {
    public enum BundleType {
        Levels,
        Textures,
        Gui,
        Audio
    }

    public interface IAssetBundler {
        void UnloadUnusedBundles();
        T LoadAsset<T>(string assetName, BundleType bundleType) where T : UnityEngine.Object;
        void GetScenePath(string requestedScene);
    }
}