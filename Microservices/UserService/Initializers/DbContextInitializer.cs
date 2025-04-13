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

        public static async void Migrate(UserDbContext context)
        {
            await context.Database.MigrateAsync();

            context.Users.Add(new UserEntity()
            {
                Id = 1,
                Email = "user@email.ru",
                Password = "password",
                Username = "username"
            });

            await context.SaveChangesAsync();
        }
    }
}
