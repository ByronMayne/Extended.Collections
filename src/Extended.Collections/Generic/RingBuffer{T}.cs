using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Extended.Collections.Generic
{
    public class RingBuffer<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        private readonly IEqualityComparer<T> m_equalityComparer;
        private readonly T?[] m_items;

        // The next slot to write to (may/maynot be allocated) 
        private int m_head;
        // The first slot (allocated)
        private int m_tail;

        /// <summary>
        /// Gets the total capacity of the buffer
        /// </summary>
        public int Capacity => m_items.Length;

        /// <summary>
        /// Gets the total number of elements in the buffer
        /// </summary>
        public int Count { get; private set; }

        /// <inheritdoc cref="ICollection"/>
        public bool IsReadOnly { get; }

        /// <summary>
        /// Creates a new ring buffer with a fix capacity
        /// </summary>
        /// <param name="capacity">The max size of the ring buffer</param>
        /// <param name="equalityComparer">The comparer to use to check for equaility</param>
        public RingBuffer(int capacity, IEqualityComparer<T>? equalityComparer = null)
        {
            m_head = 0;
            Count = 0;
            m_tail = 0;
            m_items = new T[capacity];
            IsReadOnly = false;
            m_equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Creates a new ring buffer with a fix capacity
        /// </summary>
        /// <param name="collection">A preexisting collection to use to populate the ring buffer. The capacity will be set to this element</param>
        /// <param name="equalityComparer">The comparer to use to check for equaility</param>
        public RingBuffer(IEnumerable<T> collection, IEqualityComparer<T>? equalityComparer = null)
        {
            m_items = collection.ToArray();
            m_head = 0;
            m_tail = 0;
            Count = m_items.Length;
            m_equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Adds a new item to the buffer
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(T item)
        {
            if (Count > 0 && m_head == m_tail)
            {
                m_tail = Increment(m_tail, 1);
            }

            m_items[m_head] = item;
            m_head = Increment(m_head, 1);
            Count++;

            // Clamp count 
            if (Count > Capacity)
            {
                Count = Capacity;
            }
        }

        /// <summary>
        /// Adds a range of items into this one
        /// </summary>
        /// <param name="items">The items to add</param>
        public void AddRange(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Clears the buffer of all elements
        /// </summary>
        public void Clear()
        {
            m_head = 0;
            Count = 0;
            m_tail = 0;
            for (int i = 0; i < Capacity; i++)
            {
                m_items[i] = default!;
            }
        }

        /// <summary>
        /// Returns back if a the biffer contains the item 
        /// </summary>
        /// <param name="item">The item to check if it contains</param>
        /// <returns>True if it is within otherwise false</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                T? current = m_items[i];
                if (current != null && m_equalityComparer.Equals(current, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Copies this buffer into another array
        /// </summary>
        /// <param name="array">The array to copy into</param>
        /// <param name="arrayIndex">The index to start the copying</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            IEnumerator<T> enumerator = GetEnumerator();

            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    array[arrayIndex++] = enumerator.Current;
                }
            }
        }

        /// <summary>
        /// Removes the item at the given index 
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if it exists otherwise false</returns>
        public bool Remove(T item)
        {
            int removalIndex = -1;

            int index = m_tail;

            for (int i = 0; i < Count; i++)
            {
                T? current = m_items[i];

                if(current == null)
                {
                    continue;
                }

                if (m_equalityComparer.Equals(current, item))
                {
                    removalIndex = i;
                    break;
                }

                index = Increment(m_tail, i);
            }

            // It was not found 
            if (removalIndex < 0)
            {
                return false;
            }

            int shiftCount = Count - (removalIndex + 1);
            for (int i = 0; i < shiftCount; i++)
            {
                int dst = Increment(removalIndex, i);
                int src = Increment(removalIndex, i + 1);


                m_items[dst] = m_items[src];
            }

            m_head = Increment(m_head, -1);
            m_items[m_head] = default;
            Count--;

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                int index = (i + m_tail) % Capacity;
                yield return m_items[index]!;
            }

        }

        private int Increment(int value, int increment)
        {
            increment %= Capacity;

            value += increment;

            if (value < 0)
            {
                value = Capacity + value;
            }
            else if (value >= Capacity)
            {
                value %= Capacity;
            }
            return value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
