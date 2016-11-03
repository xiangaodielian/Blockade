using UnityEngine;

namespace ApplicationManagement {
    public class InputManager : MonoBehaviour {

        #region Variables

        /*----EVENTS----*/
        public class BallLaunchEvent : GameEvent { }
        /*--------------*/

        public static InputManager Instance { get; private set; }

        private bool useCursorMovement;   //Defaults to movement using Keyboard
        private bool canPlayerMove;

        #endregion

        #region Mono Functions

        private void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;

            useCursorMovement = PrefsManager.GetMouseControl();
        }

        private void OnEnable() {
            EventManager.Instance.AddListener<LevelManager.LevelFinishEvent>(OnLevelFinish);
            EventManager.Instance.AddListener<Shield.DissolveFInishEvent>(OnDissolveFinish);
        }

        private void onDisable() {
            EventManager.Instance.RemoveListener<LevelManager.LevelFinishEvent>(OnLevelFinish);
            EventManager.Instance.RemoveListener<Shield.DissolveFInishEvent>(OnDissolveFinish);
        }

        private void Update() {
            //InGame Menu
            if(Input.GetKeyDown(KeyCode.Escape) && LevelManager.GetCurrentLevel().Contains("Level"))
                GUIManager.Instance.InGameGui.ToggleMenu();

            //Debug Console
            if(Input.GetKeyDown(KeyCode.F10))
                GUIManager.Instance.DebugUi.ToggleDebugConsole();

            //Movement
            if(!TimeManager.gamePaused && canPlayerMove) {
                if(PlayerManager.Instance.ActivePlayer != null) {
                    if(useCursorMovement) {
                        if(Input.mousePresent)
                            MoveWithMouse();
                        else
                            MoveWithTouch();
                    } else
                        MoveWithKeyboard();

                    // Control Laser Firing
                    if(PlayerManager.Instance.ActivePlayer.hasLasers) {
                        if(Input.GetButtonDown("Fire"))
                            PlayerManager.Instance.ActivePlayer.FireLasers();
                    }
                }
            }
        }

        #endregion

        #region Utility Functions

        public bool GetMovementAllowed() {
            return canPlayerMove;
        }

        public void SetMovementAllowed(bool movementAllowed) {
            canPlayerMove = movementAllowed;
        }

        public bool GetMouseMovement() {
            return useCursorMovement;
        }

        public void SetMouseMovement(bool useMouse) {
            useCursorMovement = useMouse;
        }

        //Control Paddle motion using mouse
        private void MoveWithMouse() {
            Vector3 newPos = PlayerManager.Instance.ActivePlayer.transform.position;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            if(PlayerManager.Instance.ActivePlayer.mirroredMovement)
                newPos.x += 16f - mousePos.x - newPos.x;
            else
                newPos.x += mousePos.x - newPos.x;

            PlayerManager.Instance.ActivePlayer.MovePaddle(newPos);

            if(Input.GetMouseButtonDown(0))
                EventManager.Instance.Raise(new BallLaunchEvent());
        }

        //Control Paddle motion using finger on a touch device
        private void MoveWithTouch() {
            if(Input.touchCount > 0) {
                Vector3 touchPos = Input.GetTouch(0).position;
                touchPos.z = 10f;
                touchPos = Camera.main.ScreenToWorldPoint(touchPos);

                PlayerManager.Instance.ActivePlayer.MovePaddle(touchPos);
            }
        }

        //Control Paddle motion using Keyboard (A,D,<-,-> for motion, hold Shift for dash)
        private void MoveWithKeyboard() {
            Vector3 newPos = PlayerManager.Instance.ActivePlayer.transform.position;
            float movementMultiplier = 1f;

            if(Input.GetButton("Dash"))
                movementMultiplier = 3f;

            if(Input.GetAxis("Horizontal") < 0f) {
                if(PlayerManager.Instance.ActivePlayer.mirroredMovement)
                    newPos.x += 0.15f*movementMultiplier;
                else
                    newPos.x -= 0.15f*movementMultiplier;
            }

            if(Input.GetAxis("Horizontal") > 0f) {
                if(PlayerManager.Instance.ActivePlayer.mirroredMovement)
                    newPos.x -= 0.15f*movementMultiplier;
                else
                    newPos.x += 0.15f*movementMultiplier;
            }

            PlayerManager.Instance.ActivePlayer.MovePaddle(newPos);

            if(Input.GetButtonDown("Launch"))
                EventManager.Instance.Raise(new BallLaunchEvent());
        }

        public void ProcessInputString(string input) {
            GUIManager.Instance.DebugUi.ProcessCommand(input);
        }

        #endregion

        #region Delegate Listeners

        private void OnDissolveFinish(Shield.DissolveFInishEvent e) {
            canPlayerMove = true;
        }

        private void OnLevelFinish(LevelManager.LevelFinishEvent e) {
            canPlayerMove = false;
        }

        #endregion
    }
}