# Ring Buffer

A ring buffer is a data structure that efficiently manages a fixed-size, cyclically-referenced buffer, allowing for constant-time insertions and removals while overwriting the oldest data when full.


```cs
using Extended.Collections;

public class MyClass()
{
    public MyClass()
    {
        RingBuffer<string> buffer = new RingBuffer<string>(3);
        buffer.Add("A");
        buffer.Add("B");
        buffer.Add("C");
        buffer.Add("D");
        Log(buffer) // 'B', 'C', 'D'

        buffer.Remove("C");
        Log(buffer); // 'B', 'D'

    }
}
```