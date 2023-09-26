using Extended.Collections.Generic.Specialized;
namespace Extended.Collections.Playground.Generic.Specialized;

internal class LazyDictionarySandbox : Sandbox
{
    private readonly OrderedDictionary<string, string> m_tasks = new();

    private List<string> Initialize(char key)
    {
        return new List<string>();
    }

    protected override void Run()
    {
        string[] words = new string[]
        {
            "Cat",
            "Carp",
            "Rat",
            "Dog"
        };

        LazyDictionary<char, List<string>> map = new(Initialize);

        foreach (string word in words)
        {
            char firstLetter = word[0];
            map[firstLetter].Add(word);
        }

        Logger.Information("{Count}", words['C'].Length); // 2
        Logger.Information("{Count}", words['R'].Length); // 1
        Logger.Information("{Count}", words['F'].Length); // 0 (F was never added) 
    }
}
