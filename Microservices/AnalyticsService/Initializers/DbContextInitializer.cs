using Microsoft.EntityFrameworkCore;
using AnalyticsService.DataLayer;

namespace AnalyticsService.Initializers
{
    public static class DbContextInitializer
    {
        public static void Initialize(IServiceCollection services, string conn)
        {
            services.AddDbContext<AnalyticsDbContext>(options =>
            options.UseNpgsql(conn));
        }

        public static async Task Migrate(AnalyticsDbContext context)
        {
            await context.Database.MigrateAsync();

            await context.SaveChangesAsync();
        }
    }
}
