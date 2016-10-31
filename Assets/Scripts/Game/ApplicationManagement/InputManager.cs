using UnityEngine;

namespace ApplicationManagement {
    public class InputManager : MonoBehaviour {

        #region Variables

        /*----EVENTS----*/
        public class BallLaunchEvent : GameEvent { }
        /*--------------*/

        public static bool useCursorMovement = false; //Defaults to movement using Keyboard
        public bool allowPlayerMovement = false;

        #endregion

        #region Mono Functions

        void Awake() {
            useCursorMovement = PrefsManager.GetMouseControl();
        }

        void OnEnable() {
            EventManager.Instance.AddListener<LevelManager.LevelFinishEvent>(OnLevelFinish);
            EventManager.Instance.AddListener<Shield.DissolveFInishEvent>(OnDissolveFinish);
        }

        void onDisable() {
            EventManager.Instance.RemoveListener<LevelManager.LevelFinishEvent>(OnLevelFinish);
            EventManager.Instance.RemoveListener<Shield.DissolveFInishEvent>(OnDissolveFinish);
        }

        void Update() {
            //InGame Menu
            if(Input.GetKeyDown(KeyCode.Escape) && LevelManager.GetCurrentLevel().Contains("Level"))
                GUIManager.Instance.InGameGui.ToggleMenu();

            //Debug Console
            if(Input.GetKeyDown(KeyCode.F10))
                GUIManager.Instance.DebugUi.ToggleDebugConsole();

            //Movement
            if(!TimeManager.gamePaused && allowPlayerMovement) {
                if(GameMaster.Instance.PlayerManager.ActivePlayer != null) {
                    if(useCursorMovement) {
                        if(Input.mousePresent)
                            MoveWithMouse();
                        else
                            MoveWithTouch();
                    } else
                        MoveWithKeyboard();

                    // Control Laser Firing
                    if(GameMaster.Instance.PlayerManager.ActivePlayer.hasLasers) {
                        if(Input.GetButtonDown("Fire"))
                            GameMaster.Instance.PlayerManager.ActivePlayer.FireLasers();
                    }
                }
            }
        }

        #endregion

        #region Utility Functions

        //Control Paddle motion using mouse
        void MoveWithMouse() {
            Vector3 newPos = GameMaster.Instance.PlayerManager.ActivePlayer.transform.position;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            if(GameMaster.Instance.PlayerManager.ActivePlayer.mirroredMovement)
                newPos.x += 16f - mousePos.x - newPos.x;
            else
                newPos.x += mousePos.x - newPos.x;

            GameMaster.Instance.PlayerManager.ActivePlayer.MovePaddle(newPos);

            if(Input.GetMouseButtonDown(0))
                EventManager.Instance.Raise(new BallLaunchEvent());
        }

        //Control Paddle motion using finger on a touch device
        void MoveWithTouch() {
            if(Input.touchCount > 0) {
                Vector3 touchPos = Input.GetTouch(0).position;
                touchPos.z = 10f;
                touchPos = Camera.main.ScreenToWorldPoint(touchPos);

                GameMaster.Instance.PlayerManager.ActivePlayer.MovePaddle(touchPos);
            }
        }

        //Control Paddle motion using Keyboard (A,D,<-,-> for motion, hold Shift for dash)
        void MoveWithKeyboard() {
            Vector3 newPos = GameMaster.Instance.PlayerManager.ActivePlayer.transform.position;
            float movementMultiplier = 1f;

            if(Input.GetButton("Dash"))
                movementMultiplier = 3f;

            if(Input.GetAxis("Horizontal") < 0f) {
                if(GameMaster.Instance.PlayerManager.ActivePlayer.mirroredMovement)
                    newPos.x += 0.15f*movementMultiplier;
                else
                    newPos.x -= 0.15f*movementMultiplier;
            }

            if(Input.GetAxis("Horizontal") > 0f) {
                if(GameMaster.Instance.PlayerManager.ActivePlayer.mirroredMovement)
                    newPos.x -= 0.15f*movementMultiplier;
                else
                    newPos.x += 0.15f*movementMultiplier;
            }

            GameMaster.Instance.PlayerManager.ActivePlayer.MovePaddle(newPos);

            if(Input.GetButtonDown("Launch"))
                EventManager.Instance.Raise(new BallLaunchEvent());
        }

        public void ProcessInputString(string input) {
            GUIManager.Instance.DebugUi.ProcessCommand(input);
        }

        #endregion

        #region Delegate Listeners

        void OnDissolveFinish(Shield.DissolveFInishEvent e) {
            allowPlayerMovement = true;
        }

        void OnLevelFinish(LevelManager.LevelFinishEvent e) {
            allowPlayerMovement = false;
        }

        #endregion
    }
}