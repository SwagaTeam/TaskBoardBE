using Contributors.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Contributors.Initilizers;

public class DbContextInitializer
{
    public static void Initialize(IServiceCollection services, string conn)
    {
        services.AddDbContext<ContributorsDbContext>(options =>
            options.UseNpgsql(conn));
    }

    public static async Task Migrate(ContributorsDbContext context)
    {
        await context.Database.MigrateAsync();
        await context.SaveChangesAsync();
    }
}