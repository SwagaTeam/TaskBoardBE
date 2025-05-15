namespace SharedLibrary.Constants;

public static class ItemType
{
    public const int TASK = 0;
    public const int BUG = 1;
    public const int EPIC = 2;

    public static Dictionary<int, string> Names = new()
    {
        { TASK, "Task" },
        { BUG, "Bug" },
        { EPIC, "Epic" },
    };
}