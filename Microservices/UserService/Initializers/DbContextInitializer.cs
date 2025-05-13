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

            if (!context.Users.Any())
                context.Users.Add(new UserEntity() 
                { 
                    Username = "Sample User",
                    Password = "liu0/z+pmykycxiF/6r8UjK3vZKpRg8wx0ZrAS60Vb9SspUJA92A0BS+pE1dpMEz2k4nPj8WANrY19jtOBlLmA==",
                    Salt = "ba3666ea-dd16-4fb1-a5c6-c53ad6a49966",
                    Email = "TestMessagesService@yandex.ru"
                });

            await context.SaveChangesAsync();
        }
    }
}
