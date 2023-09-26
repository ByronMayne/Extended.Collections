using Extended.Collections.Exceptions;

namespace Extended.Collections
{
    /// <summary>
    /// Describes the different methods used for merging key value pairs
    /// </summary>
    public enum MergeMethod
    {
        /// <summary>
        /// If two values share the same key throw a <see cref="DuplicateKeyException"/> will be thrown
        /// </summary>
        Throw,
        /// <summary>
        /// Keep the first value and discard any future changes
        /// </summary>
        KeepFirst,
        /// <summary>
        /// Discard the current value and use the new one.
        /// </summary>
        KeepLast,
    }
}
