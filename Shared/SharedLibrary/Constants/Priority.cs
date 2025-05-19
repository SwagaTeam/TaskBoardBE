namespace SharedLibrary.Constants
{
    public static class Priority
    {
        public const int CRITICAL = 4;
        public const int HIGH = 3;
        public const int MEDIUM = 2;
        public const int LOW = 1;
        public const int VERY_LOW = 0;

        public static Dictionary<int, string> Names = new()
        {
            { CRITICAL, "Критический" },
            { HIGH, "Высокий" },
            { MEDIUM, "Средний" },
            { LOW, "Низкий" },
            { VERY_LOW, "Очень низкий" }
        };
    }
}