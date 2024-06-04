using System;
using System.Collections.Generic;
using DataStructures;
using Configs;
using UnityEngine;
using Pathfinding;
using UI;
using Extensions;

namespace Gameplay
{
    /// <summary>
    /// Main component for managing grid including displaying shortest path.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        [Tooltip("Tools panel MonoBehaviour")]
        [SerializeField] private ToolsPanel toolsPanel;
        [Tooltip("Grid painter MonoBehaviour")]
        [SerializeField] private GridPainter gridPainter;
        [Tooltip("Character MonoBehaviour")]
        [SerializeField] private Character character;
        [Tooltip("Grid plane transform")]
        [SerializeField] private Transform gridPlane;
        [Tooltip("Grid plane renderer")]
        [SerializeField] private Renderer gridRenderer;
        [Tooltip("Line renderer that will display shortest path")]
        [SerializeField] private LineRenderer pathLineRenderer;
        [Tooltip("Grid config scriptable object")]
        [SerializeField] private GridConfig gridConfig;

        /// <summary>
        /// Event to invoke when path info has changed.
        /// </summary>
        public event Action<PathInfo, int> PathInfoChanged;

        /// <summary>
        /// Event to invoke when spawn settings for spawn character button has changed.
        /// </summary>
        public event Action<bool, bool> SpawnSettingsChanged;

        private List<Node> _path;
        private Pathfinder _pathfinder;
        private bool _haveValidPath;
        private bool _spawnCharacter;

        #region MonoBehaviour
        private void Awake()
        {
            _pathfinder = new Pathfinder(gridConfig.SizeX, gridConfig.SizeY);
            character.gameObject.SetActive(false);
            PathInfoChanged?.Invoke(PathInfo.NoStartNoGoal, 0);
            _haveValidPath = false;
        }

        private void Start()
        {
            gridPlane.localScale = new Vector3(gridConfig.SizeX * 0.1f, 1f, gridConfig.SizeY * 0.1f);

            var offsetX = gridConfig.SizeX * 0.5f - 0.5f;
            var offsetZ = gridConfig.SizeY * 0.5f - 0.5f;

            gridPlane.transform.position = new Vector3(offsetX, 0f, offsetZ);

            gridRenderer.materials[0].mainTextureScale = new Vector2(gridConfig.SizeX, gridConfig.SizeY);
        }

        private void OnEnable()
        {
            gridPainter.GridChanged += OnGridChanged;
            toolsPanel.SpawnCharacterPressed += OnSpawnCharacterPressed;
        }

        private void OnDisable()
        {
            gridPainter.GridChanged -= OnGridChanged;
            toolsPanel.SpawnCharacterPressed -= OnSpawnCharacterPressed;
        }
        #endregion

        /// <summary>
        /// Whenever grid changed, try to display a new path and stop character.
        /// </summary>
        /// <param name="gridData">Grid data after change.</param>
        private void OnGridChanged(GridData gridData)
        {
            StopCharacter();
            TryDisplayPath(gridData);
        }

        #region Character Control
        /// <summary>
        /// Spawn or stop character when spawn character button is pressed.
        /// </summary>
        private void OnSpawnCharacterPressed()
        {
            if (_spawnCharacter)
            {
                SpawnCharacter();
            }
            else
            {
                StopCharacter();
            }
        }

        /// <summary>
        /// Spawn character.
        /// </summary>
        private void SpawnCharacter()
        {
            _spawnCharacter = false;
            SpawnSettingsChanged?.Invoke(false, _haveValidPath);
            character.gameObject.SetActive(true);
        }

        /// <summary>
        /// Stop character.
        /// </summary>
        private void StopCharacter()
        {
            _spawnCharacter = true;
            SpawnSettingsChanged?.Invoke(true, _haveValidPath);
            character.gameObject.SetActive(false);
        }
        #endregion

        /// <summary>
        /// Try to find and display shortest path. This will fail if path is obstructed or spawn/goal point aren't placed.
        /// </summary>
        /// <param name="gridData">Grid data to use when searching for shortest path nodes.</param>
        private void TryDisplayPath(GridData gridData)
        {
            var startPoint = gridData.StartPoint;
            var goalPoint = gridData.GoalPoint;
            var nodes = gridData.Nodes;
            _haveValidPath = false;

            // Start and goal point aren't placed.
            if (startPoint.Negative() && goalPoint.Negative())
            {
                PathInfoChanged?.Invoke(PathInfo.NoStartNoGoal, 0);
                return;
            }

            // Start point isn't placed.
            if (startPoint.Negative())
            {
                PathInfoChanged?.Invoke(PathInfo.NoStart, 0);
                return;
            }

            // Goal point isn't placed.
            if (goalPoint.Negative())
            {
                PathInfoChanged?.Invoke(PathInfo.NoGoal, 0);
                return;
            }

            var path = _pathfinder.FindPath(nodes, startPoint, goalPoint);
            if (path == null)
            {
                // Path wasn't found because it was obstructed
                PathInfoChanged?.Invoke(PathInfo.NoValidPath, 0);
                SpawnSettingsChanged?.Invoke(true, false);
                pathLineRenderer.enabled = false;
                return;
            }

            PathInfoChanged?.Invoke(PathInfo.ValidPath, path.Count);

            // This creates Vector3 list, each element is used to draw a path on a grid
            var positions = new List<Vector3>()
            {
                // Add start point to list so path will be drawn from start point.
                // Without this the first position would be the one adjacent to start point.
                nodes[startPoint.X, startPoint.Y].Point.ToVector3()
            };

            // Create vectors from each node's X and Y position.
            foreach (var node in path)
            {
                positions.Add(node.Point.ToVector3());
            }

            _haveValidPath = true;

            // Update character with this path.
            character.SetPath(positions);
            SpawnSettingsChanged?.Invoke(true, true);
            pathLineRenderer.enabled = true;
            pathLineRenderer.positionCount = positions.Count;
            pathLineRenderer.SetPositions(positions.ToArray());
        }
    }
}
