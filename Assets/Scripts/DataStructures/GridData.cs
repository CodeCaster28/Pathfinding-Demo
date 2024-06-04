namespace DataStructures
{
    /// <summary>
    /// Grid data that holds all nodes, start and goal points.
    /// </summary>
    public struct GridData
    {
        /// <summary>
        /// Nodes array that represents cells of a grid.
        /// </summary>
        public Node[,] Nodes { get; }

        /// <summary>
        /// Start point coordinates.
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// Goal point coordinates.
        /// </summary>
        public Point GoalPoint { get; set; }

        /// <summary>
        /// Creates new grid data with specified nodes array.
        /// </summary>
        /// <param name="nodes">Nodes array that represents cells of a grid.</param>
        public GridData(Node[,] nodes)
        {
            Nodes = nodes;
            StartPoint = new Point(-1, -1);
            GoalPoint = new Point(-1, -1);
        }
    }
}
