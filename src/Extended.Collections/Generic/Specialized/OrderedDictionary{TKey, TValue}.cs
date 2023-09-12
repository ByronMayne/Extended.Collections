using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Extended.Collections.Generic.Specialized
{
    public class OrderedDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IList<KeyValuePair<TKey, TValue>>,
        IOrderedDictionary
    {
        private readonly object m_syncRoot;
        private readonly bool m_isFixedSize;
        private readonly bool m_isSynchronized;
        private readonly List<TKey> m_orderedKeys;
        private readonly Dictionary<TKey, TValue> m_values;
        private static readonly IEqualityComparer<TValue> s_valueComparer;

        static OrderedDictionary()
        {
            s_valueComparer = EqualityComparer<TValue>.Default;
        }

        public OrderedDictionary()
        {
            m_isFixedSize = false;
            m_syncRoot = new object();
            m_isSynchronized = false;
            m_orderedKeys = new List<TKey>();
            m_values = new Dictionary<TKey, TValue>();
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public TValue this[TKey key]
        {
            get => m_values[key];
            set
            {
                if (!m_orderedKeys.Contains(key))
                {
                    m_orderedKeys.Add(key);
                }
                m_values[key] = value;
            }
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public TValue this[int index]
        {
            get
            {
                TKey key = m_orderedKeys[index];
                return m_values[key];
            }
            set
            {
                TKey key = m_orderedKeys[index];
                m_values[key] = value;
            }
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public ICollection<TKey> Keys => m_orderedKeys;

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public ICollection<TValue> Values
            => GetOrderedValues()
                .Select(p => p.Value)
                .ToList();

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public int Count => m_values.Count;

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public bool IsReadOnly { get; }

        public bool IsFixedSize => m_isFixedSize;

        ICollection IDictionary.Keys => m_orderedKeys;

        ICollection IDictionary.Values => m_values;

        bool ICollection.IsSynchronized => m_isSynchronized;
        object ICollection.SyncRoot => m_syncRoot;

        KeyValuePair<TKey, TValue> IList<KeyValuePair<TKey, TValue>>.this[int index]
        {
            get
            {
                TKey key = m_orderedKeys[index];
                return new KeyValuePair<TKey, TValue>(key, this[key]);
            }
            set
            {
                m_orderedKeys[index] = value.Key;
                m_values[value.Key] = value.Value;
            }
        }

        object? IDictionary.this[object key]
        {
            get => this[ConvertKey(key)];
            set => this[ConvertKey(key)] = ConvertValue(value);
        }

        /// <inheritdoc cref="IOrderedDictionary"/>
        object? IOrderedDictionary.this[int index]
        {
            get => this[index];
            set => this[index] = ConvertValue(index);
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public void Add(TKey key, TValue value)
        {
            m_values.Add(key, value);
            m_orderedKeys.Add(key);
        }

        /// <inheritdoc cref=" ICollection{KeyValuePair{TKey, TValue}}"/>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public void Clear()
        {
            m_values.Clear();
            m_orderedKeys.Clear();
        }

        /// <inheritdoc cref=" ICollection{KeyValuePair{TKey, TValue}}"/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return m_values.ContainsKey(item.Key)
                && s_valueComparer.Equals(m_values[item.Key], item.Value);
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public bool ContainsKey(TKey key)
        {
            return m_values.ContainsKey(key);
        }

        /// <inheritdoc cref=" ICollection{KeyValuePair{TKey, TValue}}"/>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="IEnumerator{KeyValuePair{TKey, TValue}}"/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, TValue> pair in GetOrderedValues())
            {
                yield return pair;
            }
        }


        /// <summary>
        /// Removes the element at the given index
        /// </summary>
        /// <param name="index">The index to remove at</param>
        public void RemoveAt(int index)
        {
            TKey key = m_orderedKeys[index];
            m_orderedKeys.RemoveAt(index);
            m_values.Remove(key);
        }

        /// <summary>
        /// Removes a pair based on it's key
        /// </summary>
        public bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
                _ = m_orderedKeys.Remove(key);
                _ = m_values.Remove(key);
                return true;
            }
            return false;
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
        public bool TryGetValue(TKey key, out TValue value)
            => m_values.TryGetValue(key, out value);

        /// <summary>
        /// Converts this ordering dictionary into a normal dictionary
        /// </summary>
        /// <returns>The converted type</returns>
        public Dictionary<TKey, TValue> ToDictionary()
            => new Dictionary<TKey, TValue>(this);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        private KeyValuePair<TKey, TValue>[] GetOrderedValues()
        {
            KeyValuePair<TKey, TValue>[] values = new KeyValuePair<TKey, TValue>[Count];
            for (int i = 0; i < m_orderedKeys.Count; i++)
            {
                TKey key = m_orderedKeys[i];
                values[i] = new KeyValuePair<TKey, TValue>(key, m_values[key]);
            }
            return values;
        }

        /// <inheritdoc cref="IEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetOrderedValues()
                .GetEnumerator();

        /// <inheritdoc cref="IDictionaryEnumerator"/>
        IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
            => new DictionaryEnumerator<TKey, TValue>(this);

        void IOrderedDictionary.Insert(int index, object key, object value)
            => Insert(index, ConvertKey(key), ConvertValue(value));

        void IOrderedDictionary.RemoveAt(int index)
            => RemoveAt(index);

        void IDictionary.Add(object key, object value)
            => Add(ConvertKey(key), ConvertValue(value));

        /// <inheritdoc cref="IDictionary"/>
        bool IDictionary.Contains(object key)
            => ContainsKey(ConvertKey(key));

        /// <inheritdoc cref="IDictionary"/>
        IDictionaryEnumerator IDictionary.GetEnumerator()
            => new DictionaryEnumerator<TKey, TValue>(this);

        /// <inheritdoc cref="IDictionary"/>
        void IDictionary.Remove(object key)
            => Remove(ConvertKey(key));

        /// <inheritdoc cref="ICollection"/>
        void ICollection.CopyTo(Array array, int index)
        {
            foreach(KeyValuePair<TKey, TValue> pair in this)
            {
                array.SetValue(pair, index++);
            }
        }

        /// <summary>
        /// Casts an <see cref="object"/> to the <typeparamref name="TKey"/> and if it fails throws an exception
        /// </summary>
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TKey ConvertKey(object key)
        {
            return key is TKey asKey
                ? asKey
                : throw new InvalidCastException($"The required key type is {typeof(TKey).FullName} however a instance of {key.GetType().FullName} was provided");
        }

        /// <summary>
        /// Casts an <see cref="object"/> to the <typeparamref name="TValue"/> and if it fails throws an exception
        /// </summary>
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TValue ConvertValue(object? value)
        {
            return value is TValue castedValue
                ? castedValue
                : throw new InvalidCastException($"The required values type is {typeof(TValue).FullName} however a instance of {value?.GetType().FullName??"null"} was provided");
        }

        public int IndexOf(KeyValuePair<TKey, TValue> item)
            => m_orderedKeys.IndexOf(item.Key);

        /// <inheritdoc cref="IDictionary"/>
        public void Insert(int index, KeyValuePair<TKey, TValue> item)
            => Insert(index, item.Key, item.Value);

        /// <summary>
        /// Inserts an element at the given position
        /// </summary>
        /// <param name="index">The postion to insert at</param>
        /// <param name="key">The key to insert</param>
        /// <param name="value">The pair to insert</param>
        public void Insert(int index, TKey key, TValue value)
        {
            m_orderedKeys.Insert(index, key);
            m_values[key] = value;
        }

    }
}
