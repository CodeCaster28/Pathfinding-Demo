using System;
using UnityEngine;

namespace Configs
{
    /// <summary>
    /// GridConfig scriptable object with data related to grid.
    /// </summary>
    [CreateAssetMenu(fileName = "GridConfig", menuName = "Configs/GridConfig")]
    public class GridConfig : ScriptableObject
    {
        private const int GridSizeMin = 8;
        private const int GridSizeMax = 200;

        [Header("Grid Size")]
        [Tooltip("Horizontal grid size")] [Range(8, 200)]
        [SerializeField] private int sizeX = 10;

        [Tooltip("Vertical grid size")] [Range(8, 200)]
        [SerializeField] private int sizeY = 10;

        [Tooltip("Height above the grid of objects like start/goal and obstacles")]
        [SerializeField] private float tileHeight = 0.01f;

        [Tooltip("Height above the grid of grid cursor object")]
        [SerializeField] private float cursorHeight = 0.02f;

        [NonSerialized] private int _specifiedSizeX = -1;
        [NonSerialized] private int _specifiedSizeY = -1;

        /// <summary>
        /// Horizontal grid size. Setting this will use custom value instead of size specified in inspector.
        /// </summary>
        public int SizeX
        {
            get => _specifiedSizeX < 0 ? sizeX : _specifiedSizeX;
            set => _specifiedSizeX = Mathf.Clamp(value, GridSizeMin, GridSizeMax);
        }

        /// <summary>
        /// Vertical grid size. Setting this will use custom value instead of size specified in inspector.
        /// </summary>
        public int SizeY
        {
            get => _specifiedSizeY < 0 ? sizeY : _specifiedSizeY;
            set => _specifiedSizeY = Mathf.Clamp(value, GridSizeMin, GridSizeMax);
        }

        /// <summary>
        /// Height above the grid of objects like start/goal and obstacles.
        /// </summary>
        public float TileHeight => tileHeight;

        /// <summary>
        /// Height above the grid of grid cursor object.
        /// </summary>
        public float CursorHeight => cursorHeight;
    }
}
