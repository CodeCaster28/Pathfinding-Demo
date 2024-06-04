using UnityEngine;
using Configs;
using Extensions;
using UI;

namespace Gameplay
{
    /// <summary>
    /// Component that controls camera movement based on received inputs.
    /// </summary>
    public class CameraMovement : MonoBehaviour
    {
        [Tooltip("Main camera used")]
        [SerializeField] private ToolsPanel toolsPanel;
        [Tooltip("Main camera used")]
        [SerializeField] private Camera gameCamera;
        [Tooltip("Camera movement speed")]
        [SerializeField] private float movementSpeed = 15f;
        [Tooltip("Camera rotation speed")]
        [SerializeField] private float rotationSpeed = 0.6f;
        [Tooltip("Camera movement speed multiplier when holding shift")]
        [SerializeField] private float shiftMultiplier = 2.5f;
        [Tooltip("Camera zoom at the start of the game")]
        [SerializeField] private float initialCameraZoom = 0.8f;
        [Tooltip("Camera pitch at the start of the game (rotation on X axis)")]
        [SerializeField] private float defaultPitch = 85f;
        [Tooltip("Camera can't go lower on Y world axis than provided value")]
        [SerializeField] private float minimumCameraHeight = 1f;
        [Tooltip("Camera can't go higher on Y world axis than provided value")]
        [SerializeField] private float maximumCameraHeight = 100f;
        [Tooltip("Grid config scriptable object")]
        [SerializeField] private GridConfig gridConfig;

        private Quaternion _anchorRotation;
        private Vector3 _anchorPoint;

        #region MonoBehaviour
        private void Awake()
        {
            ResetCamera();
        }

        private void OnEnable()
        {
            toolsPanel.ResetCameraPressed += OnResetCameraPressed;
        }

        private void Update()
        {
            CameraMove();
            CameraRotate();
        }

        private void OnDisable()
        {
            toolsPanel.ResetCameraPressed -= OnResetCameraPressed;
        }
        #endregion

        #region Camera Reset
        /// <summary>
        /// Call for camera reset when reset camera button was pressed.
        /// </summary>
        private void OnResetCameraPressed()
        {
            ResetCamera();
        }

        /// <summary>
        /// Reset camera rotation and position.
        /// </summary>
        private void ResetCamera()
        {
            // Center camera on grid
            gameCamera.transform.position = Vector3.zero.WithX(gridConfig.SizeX * 0.5f).WithZ(gridConfig.SizeY * 0.5f);

            // Bend camera a little so it will not look straight down to grid
            gameCamera.transform.eulerAngles = Vector3.zero.WithX(defaultPitch);

            // Set camera position based on grid size so grid will always fit camera view
            var zoomAmount = (gridConfig.SizeX > gridConfig.SizeY ? gridConfig.SizeX : gridConfig.SizeY) * initialCameraZoom;
            gameCamera.transform.Translate(Vector3.back * zoomAmount);
        }
        #endregion

        #region Update Camera
        /// <summary>
        /// Check input and move camera accordingly.
        /// </summary>
        private void CameraMove()
        {
            var move = Vector3.zero;

            // Take SHIFT button into consideration to speed up camera movement when it's pressed.
            var speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime;

            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.forward * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                move += Vector3.back * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                move += Vector3.left * speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += Vector3.right * speed;
            }

            transform.Translate(move);
            var position = transform.position;

            // Clamp camera height so it won't go under the grid
            var height = Mathf.Clamp(position.y, minimumCameraHeight, maximumCameraHeight);
            transform.position = position.WithY(height);
        }

        /// <summary>
        /// Check input and rotate camera.
        /// </summary>
        private void CameraRotate()
        {
            // Register camera anchor when player starts pressing right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                _anchorRotation = transform.rotation;
                _anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            }

            // Rotate camera continuously when right mouse button is held down
            if (Input.GetMouseButton(1))
            {
                Quaternion rotation = _anchorRotation;
                Vector3 rotationAmount = _anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
                rotation.eulerAngles += rotationAmount * rotationSpeed;
                transform.rotation = rotation;
            }
        }
        #endregion
    }
}
