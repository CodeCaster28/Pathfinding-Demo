using System;

namespace DataStructures
{
    /// <summary>
    /// IHeapItem interface used by Heap and Node.
    /// </summary>
    public interface IHeapItem<T> : IComparable<T>
    {
        /// <summary>
        /// Heap index.
        /// </summary>
        public int HeapIndex { get; set; }
    }
}
