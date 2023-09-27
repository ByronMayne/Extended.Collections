using System;

namespace Extended.Collections.Exceptions
{
    public class DuplicateKeyException : Exception
    {
        public object? Key { get; }

        public DuplicateKeyException(object? key) : base($"The key {key} already exists in the collection")
        {
            Key = key;
        }
    }
}
