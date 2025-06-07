using System.Collections.ObjectModel;

namespace SharedLibrary.Constants;

public static class ItemType
{
    public const int TASK = 0;
    public const int BUG = 1;
    public const int EPIC = 2;

    public static ReadOnlyDictionary<int, string> Names { get; } =
        new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            { TASK, "Task" },
            { BUG, "Bug" },
            { EPIC, "Epic" }
        });
}