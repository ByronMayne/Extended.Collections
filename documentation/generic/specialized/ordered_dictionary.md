# Ordered Dictionary 

Combines the features of a dictionary and a list, allowing key-value pairs to be stored in a specific order and accessed efficiently by their keys. 

Pros:
 * Maintains the order of key-value pairs, which can be useful for scenarios where the order of insertion or access is important.
 * Provides the functionality of a dictionary, allowing for fast key-based lookups.
 * Supports generic TKey and TValue types, making it flexible for various data types.

Cons:
 * Slightly higher memory usage compared to a regular dictionary due to the need to store the order of items.
 * Some operations may be slower compared to a regular dictionary due to maintaining the order.

### Hypothetical Use Case:
Suppose you are building a task management application, and you need to maintain a list of tasks for a project while preserving the order in which they were added. An OrderedDictionary can be useful in this scenario to store and manage tasks associated with a project.

```csharp file=../../../src/Extended.Collections.Playground/Generic/Specialized/OrderedDictionarySandbox.cs#L2-
// Imported
```