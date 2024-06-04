using System.Collections.Generic;
using Configs;
using DataStructures;
using UnityEngine;
using UnityEngine.Pool;
using Extensions;

namespace Gameplay
{
    /// <summary>
    /// Obstacle pool component for spawning obstacles at desired locations.
    /// </summary>
    public class ObstacleSpawner : MonoBehaviour
    {
        [Tooltip("Obstacle prefab used to represent non-traversable grid cell")]
        [SerializeField] private GameObject obstaclePrefab;
        [Tooltip("Parent that will hold all pooled obstacles")]
        [SerializeField] private Transform obstaclesParent;
        [Tooltip("Fraction of total cell count that will be used to determine pool size")] [Range(0.1f, 1f)]
        [SerializeField] private float poolSizeFraction;
        [Tooltip("Grid config scriptable object")]
        [SerializeField] private GridConfig gridConfig;

        private readonly Dictionary<Point, GameObject> _obstacleDict = new();
        private ObjectPool<GameObject> _obstaclePool;

        public void Awake()
        {
            var gridSize = gridConfig.SizeX * gridConfig.SizeY;
            var poolSize = gridSize * poolSizeFraction;
            _obstaclePool = new ObjectPool<GameObject> 
            (
                PoolOnCreate, 
                PoolOnGet, 
                PoolOnRelease, 
                PoolOnDestroy, 
                false, 
                (int)poolSize,
                gridSize
            );

            Prewarm((int)poolSize);
        }

        /// <summary>
        /// Toggle obstacle at given point.
        /// </summary>
        /// <remarks>
        /// We keep obstacle objects in dictionary, so if dictionary already have obstacle
        /// at given point then obstacle is removed instead of created.
        /// </remarks>
        /// <param name="point"></param>
        public void ToggleObstacleAt(Point point)
        {
            if (_obstacleDict.ContainsKey(point))
            {
                _obstaclePool.Release(_obstacleDict[point]);
                _obstacleDict.Remove(point);
            }
            else
            {
                var obstacle = _obstaclePool.Get();
                obstacle.transform.position = point.ToVector3(gridConfig.TileHeight);
                _obstacleDict.Add(point, obstacle);
            }
        }

        /// <summary>
        /// Initially create pool objects ready to be used.
        /// </summary>
        /// <remarks>
        /// Because there may be a lot of obstacles objects it is preferred to create
        /// a certain amount of them at start and keep them disabled on scene, ready
        /// to be used when needed.
        /// </remarks>
        /// <param name="defaultSize"></param>
        private void Prewarm(int defaultSize)
        {
            var obstacles = new GameObject[defaultSize];
            for (var i = 0; i < defaultSize; i++)
            {
                obstacles[i] = _obstaclePool.Get();
            }
            for (var i = 0; i < defaultSize; i++)
            {
                _obstaclePool.Release(obstacles[i]);
            }
        }

        #region Pool Operations
        /// <summary>
        /// Pool object creation method. Used by ObjectPool initializer.
        /// </summary>
        /// <remarks>
        /// New objects are instantiated and added to pool by returning a new object instance.
        /// </remarks>
        /// <returns>Newly instantiated Obstacle game object.</returns>
        private GameObject PoolOnCreate()
        {
            var obstacle = Instantiate(obstaclePrefab, Vector3.zero, Quaternion.identity, obstaclesParent);
            return obstacle;
        }

        /// <summary>
        /// Pool object get method. Used by ObjectPool initializer.
        /// </summary>
        /// <remarks>
        /// Everytime object is taken from pool it will be activated.
        /// </remarks>
        /// <param name="obstacle">Obstacle game object to get.</param>
        private void PoolOnGet(GameObject obstacle)
        {
            obstacle.SetActive(true);
        }

        /// <summary>
        /// Pool object release method. Used by ObjectPool initializer.
        /// </summary>
        /// <remarks>
        /// Everytime object is returned to pool it will be deactivated.
        /// </remarks>
        /// <param name="obstacle">Obstacle game object to release.</param>
        private void PoolOnRelease(GameObject obstacle)
        {
            obstacle.SetActive(false);
        }

        /// <summary>
        /// Pool object destroy method. Used by ObjectPool initializer.
        /// </summary>
        /// <remarks>
        /// Handles pool request of removing object. Object will be destroyed if it gets removed from pool completely.
        /// </remarks>
        /// <param name="obstacle">Obstacle game object to destroy.</param>
        private void PoolOnDestroy(GameObject obstacle)
        {
            Destroy(obstacle);
        }
        #endregion
    }
}
