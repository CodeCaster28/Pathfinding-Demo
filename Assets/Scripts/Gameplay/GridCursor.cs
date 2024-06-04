using System;
using Configs;
using DataStructures;
using UnityEngine;
using UnityEngine.EventSystems;
using Extensions;

namespace Gameplay
{
    /// <summary>
    /// Component for displaying 3D cursor on grid and capturing inputs from cursor position.
    /// </summary>
    public class GridCursor : MonoBehaviour
    {
        [Tooltip("Camera from which ray is casted to detect where player holds his cursor")]
        [SerializeField] private Camera gameCamera;
        [Tooltip("Layer mask to catch a ray and get position of cursor")]
        [SerializeField] private LayerMask raycastLayerMask;
        [Tooltip("Represents 3D cursor on grid in a place when ray hit a target")]
        [SerializeField] private GameObject cursorObject;
        [Tooltip("Grid config scriptable object")]
        [SerializeField] private GridConfig gridConfig;

        /// <summary>
        /// Event to invoke when input was received on different 3D cursor position.
        /// </summary>
        public event Action<Point> GridInputReceived;

        /// <summary>
        /// Event to invoke when mouse button was up.
        /// </summary>
        public event Action GridInputUp;

        private Point _cursorPoint;
        private Point _previousCursorPoint;
        private bool _positionChanged;

        private bool CursorOverUI => EventSystem.current.IsPointerOverGameObject();

        private void Update()
        {
            DisplayGridCursor();
            CheckClicking();
        }

        /// <summary>
        /// Display grid cursor based on ray casted from camera hitting all object with specified layer.
        /// </summary>
        private void DisplayGridCursor()
        {
            // Do nothing if cursor is over UI so inputs from UI and cursor won't intersect
            if (CursorOverUI)
                return;

            // Cast a ray from camera
            var ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            var point = new Point(-1, -1);

            if (Physics.Raycast(ray, out var hit, 1000, raycastLayerMask.value))
            {
                // Ray hit object, round vector values and display cursor at that position
                point = hit.point.Round().ToPoint();
                cursorObject.SetActive(true);
            }
            else
            {
                // Ray hit nothing, hide cursor
                cursorObject.SetActive(false);
            }

            SetCursorPosition(point);
        }

        /// <summary>
        /// Sets 3D cursor position.
        /// </summary>
        /// <param name="position">Position to place 3D cursor.</param>
        private void SetCursorPosition(Point position)
        {
            if (_previousCursorPoint == position)
                return;

            _previousCursorPoint = position;
            _positionChanged = true;
            _cursorPoint = position;

            cursorObject.transform.position = position.ToVector3(gridConfig.CursorHeight);
        }

        /// <summary>
        /// Handle clicking input and fire corresponding events.
        /// </summary>
        /// <remarks>
        /// GridInputReceived is invoked only when player clicks on new position OR
        /// when player moves his mouse while holding mouse button.
        /// Because we keep tracking of mouse position the events won't be invoked
        /// unless cursor moves to another tile.
        /// </remarks>
        private void CheckClicking()
        {
            // When mouse button is held down, player's cursor wasn't inside UI,
            // 3D cursor is active and it's position recently changed
            if (Input.GetMouseButton(0) && _positionChanged && cursorObject.activeSelf && !CursorOverUI)
            {
                _positionChanged = false;
                // Fire input event
                GridInputReceived?.Invoke(_cursorPoint);
            }

            // When mouse button was up wait for new cursor position change
            if (Input.GetMouseButtonUp(0))
            {
                // Fire input up event
                _positionChanged = true;
                GridInputUp?.Invoke();
            }
        }
    }
}
