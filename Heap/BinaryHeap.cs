using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Thaumaturgy.Heap
{

    public class BinaryHeap<T> : IEnumerable<T> where T : IHeapable<T>
    {
        protected List<T> _heap = new List<T>();
        public int LastIndex => _heap.Count - 1;
        public bool HasItems => _heap.Count > 0;
        public int Count => _heap.Count;

        protected int _tempInt;
        public virtual T Peek()
        {
            return !HasItems ? _heap[0] : default(T);
        }

        public virtual T Poll()
        {
            if (!HasItems) return default(T);
            var item = _heap[0];
            RemoveAt(0);
            return item;
        }
        public void Add(T item)
        {
            item.HeapIndex = _heap.Count;
            _heap.Add(item);
            SortUp(item);
        }
        public void RemoveAt(int heapIndex)
        {
            if (heapIndex < _heap.Count)
            {
                Swap(_heap[heapIndex], _heap[LastIndex]);
                _heap.RemoveAt(LastIndex);
                if (_heap.Count > 0)
                    SortDown(_heap[0]);
            }
        }
        public void Clear()
        {
            _heap.Clear();
        }

        public T PollAndClear()
        {
            var item = Poll();
            _heap.Clear();
            return item;
        }

        protected void SortUp(T item)
        {
            int parent;
            while (item.HeapIndex > 0)
            {
                parent = (item.HeapIndex - 1) / 2;
                if (item.CompareTo(_heap[parent]) > 0)
                    Swap(item, _heap[parent]);
                else break;
            }
        }
        protected void SortDown(T item)
        {
            int childLeft, childRight;
            while (item.HeapIndex < _heap.Count)
            {
                childLeft = item.HeapIndex * 2 + 1;
                childRight = item.HeapIndex * 2 + 2;
                if (childLeft < _heap.Count)
                {
                    if (childRight < _heap.Count)
                    {
                        if (_heap[childRight].CompareTo(_heap[childLeft]) > 0)
                        {
                            if (item.CompareTo(_heap[childRight]) < 0)
                            {
                                Swap(item, _heap[childRight]);
                                continue;
                            }
                        }
                    }
                    if (item.CompareTo(_heap[childLeft]) < 0)
                    {
                        Swap(item, _heap[childLeft]);
                        continue;
                    }
                    break;
                }
                break;
            }
        }
        protected void Swap(T itemA, T itemB)
        {
            _tempInt = itemA.HeapIndex;
            _heap[_tempInt] = itemB;
            _heap[itemB.HeapIndex] = itemA;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = _tempInt;
        }
        /// <summary>
        /// Tells the tree that the item has been
        /// updated and needs to be resorted
        /// </summary>
        /// <param name="heapIndex">
        /// the HeapIndex of the item to reevaluate
        /// </param>
        public void Update(int heapIndex)
        {
            if (heapIndex < _heap.Count)
            {
                var item = _heap[heapIndex];
                SortUp(item);
                if (item.HeapIndex == heapIndex)//we didn't move up so now we try down
                    SortDown(_heap[heapIndex]);
            }
        }

        /// <summary>
        /// Checks to see if item is crrently in the heap;
        /// </summary>
        public bool Contains(T item)
        {
            if (item.HeapIndex < _heap.Count)
                return _heap[item.HeapIndex].Equals(item);
            else return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (HasItems)
            {
                yield return Poll();
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">the type of item to be heaped</typeparam>
    public interface IHeapable<T> : IComparable<T>
    {
        /// <summary>
        /// Used to find an item on the heap.
        /// (Should only be set by the heap)
        /// </summary>
        public int HeapIndex { get; set; }
    }

}

