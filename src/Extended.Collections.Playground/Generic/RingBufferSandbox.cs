namespace Extended.Collections.Playground.Generic;
using Extended.Collections.Generic;

public class RingBufferSandbox : Sandbox
{
    private readonly RingBuffer<string> m_buffer = new (3);

    protected override void Run()
    {
        m_buffer.Add("A");
        m_buffer.Add("B");
        m_buffer.Add("C");
        Logger.Information("1. {Buffer}", m_buffer); // 1. [ "A", "B", "C" ]

        m_buffer.Add("D");
        Logger.Information("2. {Buffer}", m_buffer); // 2. [ "B", "C", "D" ]

        m_buffer.Remove("C");
        Logger.Information("3. {Buffer}", m_buffer); // 3. [ "B", "D" ]

        m_buffer.Add("E");
        Logger.Information("4. {Buffer}", m_buffer); // 4. [ "B", "D", "E" ]

        m_buffer.Clear();
        Logger.Information("5. {Buffer}", m_buffer); // [ ]
    }
}