using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static ReadOnlyDictionary<int, string> Names { get; } =
            new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
            {
                { NOT_ACTIVE, "Не активный" },
                { IN_WORK, "В работе" },
                { COMPLETED, "Завершён" }
            });

    }
}
