using Extended.Collections.Generic;
namespace Extended.Collections.Playground.Generic;

internal class DequeSandbox : Sandbox
{
    public Deque<string> m_deque = new Deque<string>();

    protected override void Run()
    {
        m_deque.PushFirst("First");
        m_deque.PushLast("Last");

        Logger.Information("Count: {Count}", m_deque.Count); // 2

        Logger.Information("Value: {Value}", m_deque.PopFirst()); // First 
        Logger.Information("Vlaue: {Value}", m_deque.PopFirst()); // Last 
        Logger.Information("Count: {Count}", m_deque.Count); // 0 
    }
}
