using System.Collections.Generic;
using UnityEngine;
using DataStructures;

namespace Pathfinding
{
    /// <summary>
    /// Pathfinding class that uses A* algorithm with heap optimization (Open Set list).
    /// Algorithm is altered to computes movement between tiles in four directions instead of eight.
    /// Because of four directions used, Manhattan distance calculation is used for Heuristics.
    /// </summary>
    public class Pathfinder
    {
        private readonly int _gridSizeX;
        private readonly int _gridSizeY;
        private readonly int _heapMaxSize;

        private Node[,] _grid;

        /// <summary>
        /// Creates new Pathfinder object with given grid size.
        /// Heap is created automatically with size based on grid size.
        /// </summary>
        /// <param name="gridSizeX">Horizontal grid size</param>
        /// <param name="gridSizeY">Vertical grid size.</param>
        public Pathfinder(int gridSizeX, int gridSizeY)
        {
            _gridSizeX = gridSizeX;
            _gridSizeY = gridSizeY;
            _heapMaxSize = gridSizeX * gridSizeY;
        }

        /// <summary>
        /// Find and returns shortest path from start point position to goal point position.
        /// </summary>
        /// <param name="grid">Array of nodes representing grid.</param>
        /// <param name="startPos">Start point position.</param>
        /// <param name="goalPos">Goal point position.</param>
        /// <returns>Path in a form of list of nodes.</returns>
        public List<Node> FindPath(Node[,] grid, Point startPos, Point goalPos)
        {
            _grid = grid;
            var startNode = _grid[startPos.X, startPos.Y];
            var goalNode = _grid[goalPos.X, goalPos.Y];

            var openSet = new Heap<Node>(_heapMaxSize);
            var closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == goalNode)
                {
                    return RetracePath(startNode, goalNode);
                }

                foreach (var neighbour in GetNeighbours(currentNode))
                {
                    if (!neighbour.Traversable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, goalNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Build a list of nodes traversing through each node's parent.
        /// </summary>
        /// <param name="startNode">Starting node.</param>
        /// <param name="endNode">Goal node.</param>
        /// <returns>Path in a form of list of nodes.</returns>
        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// Build a list of nodes that are adjacent to specified node in four direction.
        /// </summary>
        /// <param name="node">Node from which adjacent nodes will be returned.</param>
        /// <returns>Adjacent neighbour nodes.</returns>
        private List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();
            var point = node.Point;

            if (point.X - 1 >= 0)
                neighbours.Add(_grid[point.X - 1, point.Y]);

            if (point.X + 1 < _gridSizeX)
                neighbours.Add(_grid[point.X + 1, point.Y]);

            if (point.Y - 1 >= 0)
                neighbours.Add(_grid[point.X, point.Y - 1]);

            if (point.Y + 1 < _gridSizeY)
                neighbours.Add(_grid[point.X, point.Y + 1]);

            return neighbours;
        }

        /// <summary>
        /// Manhattan distance calculation between nodes.
        /// </summary>
        /// <param name="nodeFrom">Node from which distance will be calculated.</param>
        /// <param name="nodeTo">Node to which distance will be calculated.</param>
        /// <returns></returns>
        private int GetDistance(Node nodeFrom, Node nodeTo)
        {
            var distanceX = Mathf.Abs(nodeFrom.Point.X - nodeTo.Point.X);
            var distanceY = Mathf.Abs(nodeFrom.Point.Y - nodeTo.Point.Y);

            return distanceX + distanceY;
        }
    }
}
