using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.UserService;
using UserService.DataLayer;

namespace UserService.Initializers
{
    public static class DbContextInitializer
    {
        public static void Initialize(IServiceCollection services, string conn)
        {
            services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(conn));
        }

        public static async Task Migrate(UserDbContext context)
        {
            await context.Database.MigrateAsync();

            await context.SaveChangesAsync();
        }
    }
}
