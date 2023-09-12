using Extended.Collections.Generic.Specialized;
using System.Collections;

namespace Extended.Collections.Playground.Generic.Specialized;

public class OrderedDictionarySandbox : Sandbox
{
    private OrderedDictionary<string, string> m_tasks = new ();

    protected override void Run()
    {
        // Add tasks to the project in a specific order.
        m_tasks.Add("Task1", "Complete research");
        m_tasks.Add("Task2", "Write documentation");
        m_tasks.Add("Task3", "Test functionality");

        // Access tasks by their keys while maintaining their order.
        Console.WriteLine("Project Tasks:");
        foreach (KeyValuePair<string, string> entry in m_tasks)
        {
            Console.WriteLine($"TaskId: {entry.Key}");
            Console.WriteLine($"Name:{entry.Value}");
        }

        // Remove 
        m_tasks.RemoveAt(1); // Remove by index
        m_tasks.Remove("Task1"); // Remove by key 

        Logger.Information("Value: {Value}", m_tasks);
        // 1. [ 'Task2' ] = "Write documentation"
        // 2. [ 'Task3' ] = "Test functionality"
    }
}