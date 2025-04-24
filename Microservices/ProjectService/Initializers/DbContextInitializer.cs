using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer;

namespace ProjectService.Initializers
{
    public class DbContextInitializer
    {
        public static void Initialize(IServiceCollection services, string conn)
        {
            services.AddDbContext<ProjectDbContext>(options =>
            options.UseNpgsql(conn));
        }

        public static async Task Migrate(ProjectDbContext context)
        {
            await context.Database.MigrateAsync();
            await context.SaveChangesAsync();
        }
    }
}
