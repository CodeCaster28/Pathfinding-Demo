namespace DataStructures
{
    /// <summary>
    /// Node IHeapItem that represents grid cell.
    /// </summary>
    public class Node : IHeapItem<Node>
    {
        /// <summary>
        /// Parent node.
        /// </summary>
        public Node Parent { get; set; }

        /// <summary>
        /// Node coordinates.
        /// </summary>
        public Point Point { get; }

        /// <summary>
        /// Movement cost to move from the star point to this node.
        /// </summary>
        public int GCost { get; set; }

        /// <summary>
        /// Estimated (heuristic) movement cost to move from this node to goal point.
        /// </summary>
        public int HCost { get; set; }

        /// <summary>
        /// Index of a heap containing this node.
        /// </summary>
        public int HeapIndex { get; set; }

        /// <summary>
        /// Specifies whenever this node is traversable or not.
        /// </summary>
        public bool Traversable { get; set; }

        /// <summary>
        /// Sum of GCost and HCost. Node that has the lowest FCost will be selected to be the next node to move until it hits the target node.
        /// </summary>
        private int FCost => GCost + HCost;

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="traversable">Should this node be traversable.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Node(bool traversable, int x, int y)
        {
            Traversable = traversable;
            Point = new Point(x, y);
        }

        /// <summary>
        /// Compare node (nodeA) to nodeToCompare parameter (nodeB) prioritizing FCost, then HCost if FCost is the same in both Nodes.
        /// </summary>
        /// <param name="nodeToCompare"></param>
        /// <returns>A signed number indicating the relative values; 1 -> NodeA is less than NodeB,
        /// -1 -> NodeA is greater than NodeB, 0 -> both nodes FCost and HCost are the same.
        /// </returns>
        public int CompareTo(Node nodeToCompare)
        {
            var compare = FCost.CompareTo(nodeToCompare.FCost);

            // If FCost are the same in both nodes, check HCost.
            if (compare == 0)
            {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }
            return -compare;
        }
    }
}
