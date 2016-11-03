using UnityEngine;

namespace ApplicationManagement {
    public class CameraManager : MonoBehaviour {

        public static CameraManager Instance { get; private set; }

        [SerializeField] private GameObject gameCameraPrefab = null;

        private Camera activeCamera;

        void Awake() {
            if(Instance != null && Instance != this)
                Destroy(gameObject);
            Instance = this;
        }

        void Start() {
            InitCamera();
        }

        void InitCamera() {
            activeCamera = FindObjectOfType<Camera>();
            if(activeCamera != null)
                Destroy(activeCamera.gameObject);

            activeCamera = Instantiate(gameCameraPrefab).GetComponent<Camera>();
            activeCamera.transform.SetParent(transform, true);
        }
    }
}