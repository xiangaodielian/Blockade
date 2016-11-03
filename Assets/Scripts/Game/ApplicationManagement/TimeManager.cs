using UnityEngine;

namespace ApplicationManagement {
    public class TimeManager : MonoBehaviour {

        #region Variables

        [HideInInspector] public static bool gamePaused;

        #endregion

        #region Time Management

        public static void Pause() {
            Time.timeScale = Mathf.Abs(Time.timeScale - 1f);
            gamePaused = !gamePaused;
        }

        #endregion
    }
}