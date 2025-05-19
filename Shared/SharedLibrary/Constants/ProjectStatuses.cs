using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Constants
{
    public static class ProjectStatuses
    {
        public const int NOT_ACTIVE = 0;
        public const int IN_WORK = 1;
        public const int COMPLETED = 2;

        public static Dictionary<int, string> Names = new()
        {
            { NOT_ACTIVE, "Не активный" },
            { IN_WORK, "В работе" },
            { COMPLETED, "Завершён" }
        };
    }
}
