namespace DataStructures
{
    /// <summary>
    /// Heap class that holds generic IHeapItem items.
    /// </summary>
    public class Heap<T> where T : IHeapItem<T>
    {
        private T[] Items { get; }

        /// <summary>
        /// Elements count inside this heap.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Creates new heap.
        /// </summary>
        /// <param name="maxHeapSize">Maximum size of a heap.</param>
        public Heap(int maxHeapSize)
        {
            Items = new T[maxHeapSize];
        }

        /// <summary>
        /// Adds new item to a heap.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            item.HeapIndex = Count;
            Items[Count] = item;
            SortUp(item);
            Count++;
        }

        /// <summary>
        /// Checks if heap contains specified item.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return Equals(Items[item.HeapIndex], item);
        }

        /// <summary>
        /// Removes first item and sorts a heap.
        /// </summary>
        /// <returns>Returns item removed.</returns>
        public T RemoveFirst()
        {
            var firstItem = Items[0];
            Count--;
            Items[0] = Items[Count];
            Items[0].HeapIndex = 0;
            SortDown(Items[0]);
            return firstItem;
        }

        /// <summary>
        /// Sorts heap items with Heapify Down method.
        /// </summary>
        /// <param name="item"></param>
        private void SortDown(T item)
        {
            while (true)
            {
                var childIndexLeft = item.HeapIndex * 2 + 1;
                var childIndexRight = item.HeapIndex * 2 + 2;

                if (childIndexLeft < Count)
                {
                    var swapIndex = childIndexLeft;

                    if (childIndexRight < Count)
                    {
                        if (Items[childIndexLeft].CompareTo(Items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(Items[swapIndex]) < 0)
                    {
                        Swap(item, Items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Sorts heap items with Heapify Up method.
        /// </summary>
        /// <param name="item"></param>
        private void SortUp(T item)
        {
            var parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                var parentItem = Items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        /// <summary>
        /// Swaps two items in the heap.
        /// </summary>
        /// <param name="itemA">Item A to swap with item B.</param>
        /// <param name="itemB">Item B to swap with item A.</param>
        private void Swap(T itemA, T itemB)
        {
            (Items[itemA.HeapIndex], Items[itemB.HeapIndex]) = (itemB, itemA);
            (itemA.HeapIndex, itemB.HeapIndex) = (itemB.HeapIndex, itemA.HeapIndex);
        }
    }
}
