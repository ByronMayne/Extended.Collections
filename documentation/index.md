---
sidebar_position: 1
---
# Introduction

Collections Extended a zero dependency _netstandard_ library that contains an array of different generic collections to use in any application. Each collection has it's own use 
case and is fully covered in unit tests giving you confidence they they work. 


Have a collection type that you would want to add? Feel free to make a pull request.


## Collections 

| Name | Type Name | Description                                                                         |
|------|-----------|-------------------------------------------------------------------------------------|
| [Ring Buffer](./generic/ring_buffer.md) | `Collections.Extended.Generic.RingBuffer<T>` | A ring buffer efficiently manages a fixed-size, cyclically-referenced buffer, allowing for constant-time insertions and removals while overwriting the oldest data when full. |
| [Ordered Dictionary](./generic/specialized/ordered_dictionary.md) | 'Extended.Collections.Generic.Specialized.OrderedDictionary<TKey, TValue> | Combines the features of a dictionary and a list, allowing key-value pairs to be stored in a specific order and accessed efficiently by their keys. |
| [Lazy Dictionary](./generic/specialized/ordered_dictionary.md) | 'Extended.Collections.Generic.Specialized.LazyDictionary<TKey, TValue>` | Instead of throwing exceptions when a key is not found, a delegate is invoked which creates a value for that key | 