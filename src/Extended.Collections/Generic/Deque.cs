using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Extended.Collections.Generic
{

    /// <summary>
    /// A Deque or 'double-ended queue` is collection used to allow for removing from the start or the end 
    /// </summary>
    public class Deque<T> : ICollection<T>
    {
        private enum Position : int
        {
            First,
            Last
        }

        private T[] m_items;
        private int m_first;
        private int m_last;

        /// <summary>
        /// Gets the total number of elements
        /// </summary>
        public int Count => m_last == m_first
            ? 0
            : m_last - m_first - 1;

        /// <summary>
        /// Gets the total size of the collection 
        /// </summary>
        public int Capacity => m_items.Length;

        /// <summary>
        /// Gets if there are any elements left in the queue 
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Gets if it's readonly or not
        /// </summary>
        public bool IsReadOnly { get; }

        // -1 First --> Last +1
        public Deque() : this(10)
        { }

        public Deque(int capacity)
        {
            // Make the sides equal 
            IsReadOnly = false;
            m_items = new T[capacity];

            int halfSize = m_items.Length / 2;
            m_first = halfSize;
            m_last = halfSize;
        }

        public void PushFirst(T item)
            => Push(Position.First, item);

        public void PushRangeFirst(IEnumerable<T> range)
          => PushRange(Position.First, range);

        public void PushLast(T item)
            => Push(Position.First, item);

        public void PushRangeLast(IEnumerable<T> range)
            => PushRange(Position.Last, range);

        public T PopLast()
           => Pop(Position.Last);

        public bool TryPopLast(out T? item)
            => TryPop(Position.Last, out item);

        public T PopFirst()
            => Pop(Position.First);

        public bool TryPopFirst(out T? item)
            => TryPop(Position.First, out item);

        public T PeekFirst()
            => Peek(Position.First);

        public bool TryPeekFirst(out T? item)
            => TryPeek(Position.First, out item);

        public T PeekLast()
            => Peek(Position.Last);

        public bool TryPeekLast(out T? item)
            => TryPeek(Position.Last, out item);

        public bool Contains(T item)
        {
            for (int i = m_first; i < m_last; i++)
            {
                if (EqualityComparer<T>.Default.Equals(item, m_items[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private T Peek(Position position)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Deque is empty");
            }
            TryPeek(position, out T? item);
            return item!;
        }

        private bool TryPeek(Position position, out T? item)
        {
            item = default;

            if (IsEmpty)
            {
                return false;
            }

            switch (position)
            {
                default:
                    throw new ArgumentException("Invalid peek position");
                case Position.Last:
                    item = m_items[m_last - 1];
                    return true;
                case Position.First:
                    item = m_items[m_first + 1];
                    return true;
            }
        }

        /// <inheritdoc cref="ICollection{T}"/>
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICollection{T}"/>
        public void Clear()
        {
            for (int i = 0; i < m_items.Length; i++)
            {
                m_items[i] = default!;
            }
            int halfSize = Capacity / 2;
            m_first = halfSize;
            m_last = halfSize;
        }

        /// <inheritdoc cref="IEnumerator{T}"/>
        public IEnumerator<T> GetEnumerator()
        {
            int index = m_first + 1;
            int last = m_last + 1;

            for (int i = m_first + 1; i < last; i++)
            {
                yield return m_items[i];
            }
        }

        private void PushRange(Position position, IEnumerable<T> items)
        {
            ICollection<T> collection = items is ICollection<T> asCollection
                ? asCollection
                : items.ToList();

            bool rebalance = position == Position.First
                ? m_first - collection.Count < 0
                : m_last + collection.Count > Capacity;

            if (rebalance)
            {
                // Rebalance the tree so that we can have
                // all elements fit within one action
                BalanceTree(position, collection.Count);
            }

            foreach(T item in collection)
            {
                Push(position, item);
            }
        }

        private void Push(Position position, T item)
        {
            int insert;

            if (position == Position.First ? m_first - 1 < 0 : m_last + 1 >= Capacity)
            {
                BalanceTree(position, 1);
            }


            if (m_first == m_last)
            {
                insert = m_first;
                m_first--;
                m_last++;
            }
            else
            {
                switch (position)
                {
                    default:
                        return;
                    case Position.First:
                        insert = m_first;
                        m_first--;
                        break;
                    case Position.Last:
                        insert = m_last;
                        m_last++;
                        break;
                }
            }

            m_items[insert] = item;
        }

        /// <summary>
        /// This function is invoked when the index to be inserted is out of range. We have two options.
        /// 1. Shift the items within the same array to avoid another allocation. This happens if 1/4 of the total capacity if free on the other side
        /// 2. Resize the array and make it twice as big and shift all the element over to the new array 
        /// </summary>
        /// <param name="position">The postion that is going out of bounds</param>
        /// <param name="allocationCount">The number of entries to leave avaiable to be allocated</param>
        private int BalanceTree(Position position, int allocationCount)
        {
            lock (m_items)
            {
                int halfSize = Capacity / 2;
                int quarterSize = Capacity / 4;
                int remaing = Capacity - Count - allocationCount;

                if (remaing > quarterSize)
                {
                    // Re-distribute 
                    bool leftToRight = position == Position.First;
                    int increment = leftToRight ? -1 : 1;
                    int i = (leftToRight ? m_last : m_first) + increment;
                    int shift = (remaing / 2) * (increment * -1);

                    while (i >= 0 && i < Capacity)
                    {
                        int dst = i + shift;
                        m_items[dst] = m_items[i];
                        m_items[i] = default!;
                        i += increment;
                    }
                    m_last += shift;
                    m_first += shift;
                    return shift;
                }
                else
                {
                    // Resize 
                    T[] resizedItems = new T[Capacity * 2];
                    Array.Copy(m_items, 0, resizedItems, halfSize, Capacity);
                    m_items = resizedItems;
                    m_first += halfSize;
                    m_last += halfSize;
                    return halfSize;
                }
            }
        }

        private T Pop(Position position)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Deque is empty");
            }

            TryPop(position, out T? item);
            return item!;
        }

        private bool TryPop(Position position, out T? item)
        {
            item = default;

            if (IsEmpty)
            {
                return false;
            }

            switch (position)
            {
                default:
                    throw new ArgumentException("Invalid peek position");
                case Position.First:
                    m_last--;
                    item = m_items[m_last];
                    m_items[m_last] = default!;
                    break;
                case Position.Last:
                    m_first++;
                    item = m_items[m_first];
                    m_items[m_first] = default!;
                    break;
            }
            return true;
        }

        /// <inheritdoc cref="ICollection{T}"/>
        void ICollection<T>.Add(T item)
            => PushFirst(item);

        /// <inheritdoc cref="ICollection{T}"/>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(m_items, m_first + 1, array, arrayIndex, Count);
        }

        /// <inheritdoc cref="IEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }
    }
}
