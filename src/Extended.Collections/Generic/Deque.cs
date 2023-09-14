using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extended.Collections.Playground.Generic
{
    /// <summary>
    /// A Deque or 'double-ended queue` is collection used to allow for removing from the start or the end 
    /// </summary>
    public class Deque<T> 
    {
        private List<T> m_items;
        private int m_head;
        private int m_tail;
        private int m_count;

        /// <summary>
        /// Gets the total number of elements
        /// </summary>
        public int Count =>  m_head + m_tail;

        /// <summary>
        /// Gets the total size of the collection 
        /// </summary>
        public int Capacity => m_items.Capacity;

        /// <summary>
        /// Gets if there are any elements left in the queue 
        /// </summary>
        public bool IsEmpty => m_head == m_tail;

        public Deque()
        {
            m_items = new List<T>(10);
            int halfSize = m_items.Count / 2;
            m_head = halfSize - 1;
            m_tail = halfSize + 1;
        }


        public void PushFirst(T item)
        {
            m_head++;
            m_items[m_head] = item;
        }

        public void PushRangeFirst(IEnumerable<T> range)
        { }

        public void PushLast(T item)
        {
            m_tail--;
            m_items[m_tail] = item;
        }

        public void PushRangeLast(IEnumerable<T> range) 
        { }

        public T PopLast()
        {
            return default!;
        }

        public bool TryPopLast(out T? item)
        {
            item = default!;
            return false;
        }

        public T PopFirst()
        {
            return default!;
        }

        public bool TryPopFirst(out T? item)
        {
            item = default!;
            return false;
        }

        public T PeekFirst()
        {
            return default!;
        }

        public bool TryPeekFirst(out T? item)
        {
            item = default!;
            return false;
        }

        public T PeekLast()
        {
            return default!;
        }

        public bool TryPeekLast(out T? item)
        {
            item = default!;
            return false;
        }

        public bool Contains(T item)
        {
            for (int i = m_head; i < m_tail; i++)
            {
                if (EqualityComparer<T>.Default.Equals(item, m_items[i]))
                {
                    return true;
                }
            }

            return false;
        }
    
        private void Balance()
        {
            int remaining = Capacity - Count;
            int resizeThreshhold = (int)MathF.Round((float)Count / 4f, MidpointRounding.ToPositiveInfinity);

            if(m_tail < 0)
            {
                int remaining = Capacity - Count;
                if(remaining < )
            }
        }
    }
}
