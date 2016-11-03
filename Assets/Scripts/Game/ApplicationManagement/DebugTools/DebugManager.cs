using UnityEngine;

namespace ApplicationManagement.DebugTools {
    public class DebugManager : MonoBehaviour {
        #region Variables

        public static DebugManager Instance { get; private set; }
        public static IDebugLogger Logger { get; private set; }

        #endregion

        #region Mono Functions

        private void Awake() {
            if((Instance != null) && (Instance != this))
                Destroy(gameObject);
            Instance = this;
        }

        #endregion

        #region Utility

        public void InitLogger(bool writeLogToFile) {
            if(Logger == null)
                Logger = new DebugLogger(writeLogToFile);
        }

        #endregion
    }
}