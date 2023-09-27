using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Extended.Collections.Generic.Specialized
{
    /// <summary>
    /// An implemention of a <see cref="Dictionary{TKey, TValue}"/> that instead of returning 
    /// <see cref="KeyNotFoundException"/> it will invoke the <see cref="InitializeDelegate"/> to
    /// create a value, save that value, then return the result. Once a value is created it will 
    /// be returned every time unless the key is removed. 
    /// </summary>
    /// <typeparam name="TKey">The key to access the dictionary</typeparam>
    /// <typeparam name="TValue">The value that the dictionary contains</typeparam>
    [Serializable]
    public class LazyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public delegate TValue InitializeDelegate(TKey key);

        private readonly InitializeDelegate m_initializer;
        private readonly Dictionary<TKey, TValue> m_backingField;

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public TValue this[TKey key]
        {
            set => m_backingField[key] = value;
            get
            {
                if(!TryGetValue(key, out TValue value))
                {
                    value = m_initializer(key);
                    m_backingField[key] = value;
                }
                return value;
            }
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public ICollection<TKey> Keys => m_backingField.Keys;

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public ICollection<TValue> Values => m_backingField.Values;

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public int Count
            => m_backingField.Count;

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public bool IsReadOnly
            => false;

        public LazyDictionary(InitializeDelegate initializer) : this(initializer, equalityComparer: null)
        { }

        public LazyDictionary(InitializeDelegate initializer, IEqualityComparer<TKey>? equalityComparer) : this(initializer, 0, equalityComparer)
        { }

        public LazyDictionary(InitializeDelegate initializer, IDictionary<TKey, TValue> dictionary) : this(initializer, dictionary.Count, null)
        {
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public LazyDictionary(InitializeDelegate initializer, int capacity, IEqualityComparer<TKey>? equalityComparer)
        {
            m_initializer = initializer;
            equalityComparer ??= EqualityComparer<TKey>.Default;
            m_backingField = new Dictionary<TKey, TValue>(capacity, equalityComparer);
        }


        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public void Add(TKey key, TValue value)
        {
            m_backingField.Add(key, value);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public void Clear()
        {
            m_backingField.Clear();
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public bool ContainsKey(TKey key)
        {
            return m_backingField.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_backingField.GetEnumerator();
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_backingField.TryGetValue(key, out value);
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        public bool Remove(TKey key)
        {
            return m_backingField.Remove(key);
        }

        /// <summary>
        /// Creates a copy of the data and returns it as a normal <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        public Dictionary<TKey, TValue> ToDictionary()
            => new Dictionary<TKey, TValue>(m_backingField);

        /// <inheritdoc cref="IDictionary{TKey,TValue}"/>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ICollection<KeyValuePair<TKey, TValue>> @this = this;
            @this.Add(item);
        }

        /// <inheritdoc cref="ICollection{KeyValuePair{TKey, TValue}}"/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            ICollection<KeyValuePair<TKey, TValue>> @this = this;
            return @this.Contains(item);
        }

        /// <inheritdoc cref="ICollection{KeyValuePair{TKey, TValue}}"/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            ICollection<KeyValuePair<TKey, TValue>> @this = this;
            return @this.Remove(item);
        }

        /// <inheritdoc cref="ICollection{KeyValuePair{TKey, TValue}}"/>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ICollection<KeyValuePair<TKey, TValue>> @this = this;
            @this.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="IEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_backingField.GetEnumerator();
        }
    }
}
