using System.Collections;
using System.Collections.Generic;

namespace Extended.Collections
{
    /// <summary>
    /// Implemention of <see cref="IDictionaryEnumerator"/>
    /// </summary>
    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> m_enumerable;

        /// <summary>
        /// Gets the current key we are pointing at
        /// </summary>
        public TKey? Key { get; private set; }

        /// <summary>
        /// Gets the current value 
        /// </summary>
        public TValue? Value { get; private set; }

        /// <summary>
        /// Gets the current key value pair that we are pointing at
        /// </summary>
        public KeyValuePair<TKey, TValue> Current { get; private set; }

        /// <inheritdoc cref="IEnumerator"/>
        DictionaryEntry IDictionaryEnumerator.Entry
            => new(Key, Value);

        /// <inheritdoc cref="IDictionaryEnumerator"/>
        object? IDictionaryEnumerator.Key => Key;

        /// <inheritdoc cref="IDictionaryEnumerator"/>
        object? IDictionaryEnumerator.Value => Value;

        /// <inheritdoc cref="IDictionaryEnumerator"/>
        object? IEnumerator.Current => Current;

        public DictionaryEnumerator(IDictionary<TKey, TValue> target)
        {
            Key = default;
            Value = default;
            Current = default;
            m_enumerable = target.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerator"/>
        public bool MoveNext()
        {
            Key = default;
            Value = default;
            Current = default;

            if (m_enumerable.MoveNext())
            {
                Current = m_enumerable.Current;
                Key = Current.Key;
                Value = Current.Value;
                return true;
            }
            return false;
        }

        /// <inheritdoc cref="IEnumerator"/>
        public void Reset()
        {
            m_enumerable.Reset();
        }

        /// <inheritdoc cref="IEnumerator"/>
        bool IEnumerator.MoveNext()
        {
            return MoveNext();
        }

        /// <inheritdoc cref="IEnumerator"/>
        void IEnumerator.Reset()
        {
            _ = MoveNext();
        }
    }
}
