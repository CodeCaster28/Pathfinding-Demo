using System;
using Configs;
using DataStructures;
using Extensions;
using UI;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Component for painting obstacles on grid, placing start and goal point and inverting a grid.
    /// </summary>
    public class GridPainter : MonoBehaviour
    {
        [Tooltip("Tools panel MonoBehaviour")]
        [SerializeField] private ToolsPanel toolsPanel;
        [Tooltip("Grid cursor MonoBehaviour")]
        [SerializeField] private GridCursor gridCursor;
        [Tooltip("Obstacle spawner MonoBehaviour")]
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [Tooltip("Object that will represent start point on grid")]
        [SerializeField] private GameObject startPointObject;
        [Tooltip("Object that will represent goal point on grid")]
        [SerializeField] private GameObject goalPointObject;
        [Tooltip("Grid config scriptable object")]
        [SerializeField] private GridConfig gridConfig;

        /// <summary>
        /// Event to invoke when grid has changed.
        /// </summary>
        public event Action<GridData> GridChanged;

        private GridData _gridData;
        private PlacingMode _placingMode;

        private bool _paintOnlyObstacles;
        private bool _paintOnlyTraversable;

        #region MonoBehaviour
        private void Start()
        {
            startPointObject.SetActive(false);
            goalPointObject.SetActive(false);
            CreateGrid();
        }

        private void OnEnable()
        {
            gridCursor.GridInputUp += OnGridInputUp;
            gridCursor.GridInputReceived += OnGridInputReceived;
            toolsPanel.PlacingModeChanged += OnPlacingModeChanged;
            toolsPanel.InvertGridPressed += OnInvertGridPressed;
        }

        private void OnDisable()
        {
            gridCursor.GridInputUp -= OnGridInputUp;
            gridCursor.GridInputReceived -= OnGridInputReceived;
            toolsPanel.PlacingModeChanged -= OnPlacingModeChanged;
            toolsPanel.InvertGridPressed -= OnInvertGridPressed;
        }
        #endregion

        /// <summary>
        /// Check whenever provided point is start or goal.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>TRUE if point lays on start or goal point.</returns>
        private bool IsStartOrGoal(Point point)
        {
            return _gridData.StartPoint == point || _gridData.GoalPoint == point;
        }

        /// <summary>
        /// Change placing mode as a reaction to corresponding UI button press.
        /// </summary>
        /// <param name="placingMode">Placing mode after change.</param>
        private void OnPlacingModeChanged(int placingMode)
        {
            _placingMode = (PlacingMode)placingMode;
        }

        /// <summary>
        /// Create new, empty (traversable) grid.
        /// </summary>
        private void CreateGrid()
        {
            var nodes = new Node[gridConfig.SizeX, gridConfig.SizeY];

            for (var x = 0; x < gridConfig.SizeX; x++)
            {
                for (var y = 0; y < gridConfig.SizeY; y++)
                {
                    nodes[x, y] = new Node(true, x, y);
                }
            }

            _gridData = new GridData(nodes);
        }

        /// <summary>
        /// Invert grid nodes traversable values when invert grid button was pressed.
        /// </summary>
        private void OnInvertGridPressed()
        {
            for (var x = 0; x < gridConfig.SizeX; x++)
            {
                for (var y = 0; y < gridConfig.SizeY; y++)
                {
                    if (_gridData.StartPoint.Equals(x, y) || _gridData.GoalPoint.Equals(x, y))
                        continue;

                    _gridData.Nodes[x, y].Traversable = !_gridData.Nodes[x, y].Traversable;
                    obstacleSpawner.ToggleObstacleAt(_gridData.Nodes[x, y].Point);
                }
            }

            GridChanged?.Invoke(_gridData);
        }

        #region Handle Input
        /// <summary>
        /// React to grid input from grid cursor.
        /// </summary>
        /// <remarks>
        /// Getting input from grid means two things:
        /// - reacting to player single click,
        /// - reacting to changing grid cursor position while player held button down.
        /// Thank to this, grid can be "painted on" while holding mouse button.
        /// </remarks>
        /// <param name="point">Point where the cursor was during input.</param>
        private void OnGridInputReceived(Point point)
        {
            // Based on grid placing mode choose proper action.
            switch (_placingMode)
            {
                case PlacingMode.Obstacle:
                    ToggleObstacle(point);
                    break;
                case PlacingMode.StartPoint:
                    PlaceStartPoint(point);
                    break;
                case PlacingMode.GoalPoint:
                    PlaceGoalPoint(point);
                    break;
            }
        }

        /// <summary>
        /// Reset _paintOnly variables.
        /// </summary>
        /// <remarks>
        /// ToggleObstacle is called whenever GridInputReceived is called which is not only
        /// a situation where player do a single click on a grid, but also when grid cursor
        /// position changes while holding mouse button.
        /// Because of this ToggleObstacle must know if moving the mouse over board should
        /// only paint obstacle or clearing them. _paintOnly variables are used for that.
        /// </remarks>
        private void OnGridInputUp()
        {
            _paintOnlyObstacles = false;
            _paintOnlyTraversable = false;
        }
        #endregion

        #region Paint
        /// <summary>
        /// Toggle obstacle in node at given point.
        /// If node at this point is traversable it will become an obstacle and vice versa.
        /// </summary>
        /// <remarks>
        /// ToggleObstacle is called whenever GridInputReceived is called which is not only
        /// a situation where player do a single click on a grid, but also when grid cursor
        /// position changes while holding mouse button.
        /// Because of this ToggleObstacle must know if moving the mouse over board should
        /// only paint obstacle or clearing them. _paintOnly variables are used for that.
        /// </remarks>
        /// <param name="point">Point to place obstacle or remove it.</param>
        private void ToggleObstacle(Point point)
        {
            if (IsStartOrGoal(point))
                return;

            var paintedNode = _gridData.Nodes[point.X, point.Y];

            if (!_paintOnlyObstacles && !_paintOnlyTraversable)
            {
                _paintOnlyObstacles = paintedNode.Traversable;
                _paintOnlyTraversable = !paintedNode.Traversable;
            }

            if ((!_paintOnlyObstacles || !paintedNode.Traversable) && (!_paintOnlyTraversable || paintedNode.Traversable))
                return;

            paintedNode.Traversable = !paintedNode.Traversable;
            obstacleSpawner.ToggleObstacleAt(point);

            GridChanged?.Invoke(_gridData);
        }

        /// <summary>
        /// Place start point at given point coordinates.
        /// </summary>
        /// <param name="point">Point to place start point.</param>
        private void PlaceStartPoint(Point point)
        {
            // Can't place start point on obstacles
            if (!_gridData.Nodes[point.X, point.Y].Traversable)
                return;

            // Can't place start point on goal point.
            // Also don't place on already placed start point because there is no sense
            // to call GridChanged when nothing really changed.
            if (IsStartOrGoal(point))
                return;

            startPointObject.SetActive(true);
            _gridData.StartPoint = point;
            startPointObject.transform.position = point.ToVector3(gridConfig.TileHeight);

            GridChanged?.Invoke(_gridData);
        }

        /// <summary>
        /// Place end point at given point coordinates.
        /// </summary>
        /// <param name="point">Point to place goal point.</param>
        private void PlaceGoalPoint(Point point)
        {
            // Can't place start point on obstacles
            if (!_gridData.Nodes[point.X, point.Y].Traversable)
                return;

            // Can't place goal point on start point.
            // Also don't place on already placed goal point because there is no sense
            // to call GridChanged when nothing really changed.
            if (IsStartOrGoal(point))
                return;

            goalPointObject.SetActive(true);
            _gridData.GoalPoint = point;
            goalPointObject.transform.position = point.ToVector3(gridConfig.TileHeight);

            GridChanged?.Invoke(_gridData);
        }
        #endregion
    }
}
