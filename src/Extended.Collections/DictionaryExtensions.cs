using Extended.Collections.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Extended.Collections
{
    /// <summary>
    /// Contains extension methods for working with <see cref="IDictionary{TKey, TValue}"/>
    /// </summary>
    public static class BaseDictionaryExtension
    {
        [ExcludeFromCodeCoverage]
        public static IDictionary<TKey,TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IEqualityComparer<TKey>? equalityComparer, IDictionary<TKey, TValue> target)
           => Merge(source, MergeMethod.KeepLast, equalityComparer, new[] { target });

        [ExcludeFromCodeCoverage]
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> target)
            => Merge(source, MergeMethod.KeepLast, null, new[] { target });

        [ExcludeFromCodeCoverage]
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, MergeMethod method, IDictionary<TKey, TValue> target) 
            => Merge(source, method, null, new IDictionary<TKey, TValue>[] { target });


        [ExcludeFromCodeCoverage]
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, params IDictionary<TKey, TValue>[] targets) 
            => Merge(source, MergeMethod.KeepLast, null, targets);

        /// <inheritdoc cref="Merge{TKey, TValue}(IDictionary{TKey, TValue}, MergeMethod, IEqualityComparer{TKey}?, IDictionary{TKey, TValue}[])"/>
        [ExcludeFromCodeCoverage]
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, MergeMethod mergeMethod, params IDictionary<TKey, TValue>[] targets)
            => Merge(source, mergeMethod, null, targets);
        
        /// <summary>
        /// Merges instances of <see cref="IDictionary{TKey, TValue}"/> into a unifed instance with the option of choosing how they are merged.
        /// </summary>
        /// <typeparam name="TKey">The key type</typeparam>
        /// <typeparam name="TValue">The value type to store</typeparam>
        /// <param name="source">The base value to start merging with.</param>
        /// <param name="equalityComparer">How keys will be compaired between two dictionaries</param>
        /// <param name="targets">The target instances to merge into the dictionary</param>
        /// <returns>The result of the merging of objects</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> was null</exception>
        /// <exception cref="DuplicateKeyException">Two dictionaries contained the same key and the <paramref name="mergeMethod"/> was set to <see cref="MergeMethod.Throw"/></exception>
        [ExcludeFromCodeCoverage]
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IEqualityComparer<TKey>? equalityComparer, params IDictionary<TKey, TValue>[] targets)
           => Merge(source, MergeMethod.KeepLast, equalityComparer, targets);

        /// <summary>
        /// Merges instances of <see cref="IDictionary{TKey, TValue}"/> into a unifed instance with the option of choosing how they are merged.
        /// </summary>
        /// <typeparam name="TKey">The key type</typeparam>
        /// <typeparam name="TValue">The value type to store</typeparam>
        /// <param name="source">The base value to start merging with.</param>
        /// <param name="mergeMethod">The mergeMethod which is used to merge duplicate keys</param>
        /// <param name="equalityComparer">How keys will be compaired between two dictionaries</param>
        /// <param name="targets">The target instances to merge into the dictionary</param>
        /// <returns>The result of the merging of objects</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> was null</exception>
        /// <exception cref="DuplicateKeyException">Two dictionaries contained the same key and the <paramref name="mergeMethod"/> was set to <see cref="MergeMethod.Throw"/></exception>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, MergeMethod mergeMethod, IEqualityComparer<TKey>? equalityComparer, params IDictionary<TKey, TValue>[] targets)  
        {
            if(source == null) throw new ArgumentNullException(nameof(source));

            equalityComparer ??= EqualityComparer<TKey>.Default;

            IDictionary<TKey, TValue> merged = new Dictionary<TKey, TValue>(source, equalityComparer);

            foreach (IDictionary<TKey, TValue> other in targets)
            {
                foreach(KeyValuePair<TKey, TValue> pair in other)
                {
                    if (merged.ContainsKey(pair.Key))
                    {
                        switch(mergeMethod)
                        {
                            case MergeMethod.KeepFirst:
                                continue;
                            case MergeMethod.KeepLast:
                                merged[pair.Key] = pair.Value;
                                break;
                            case MergeMethod.Throw:
                                throw new DuplicateKeyException(pair.Key);
                        }
                    }
                    else
                    {
                        merged.Add(pair.Key, pair.Value);
                    }
                }
            }
            return merged;
        }
    }
}
