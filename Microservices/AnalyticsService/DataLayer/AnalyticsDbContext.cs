using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.AnalyticsService;
using SharedLibrary.Entities.ProjectService;
using System.Collections.Generic;

namespace AnalyticsService.DataLayer
{
    public class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }

        public DbSet<TaskHistoryEntity> TaskHistories => Set<TaskHistoryEntity>();
    }
}
