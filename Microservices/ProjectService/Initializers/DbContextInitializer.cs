using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Initializers
{
    public static class DbContextInitializer
    {
        public static void Initialize(IServiceCollection services, string conn)
        {
            services.AddDbContext<ProjectDbContext>(options =>
            options.UseNpgsql(conn));
        }

        public static async Task Migrate(ProjectDbContext context)
        {
            var role = await context.Roles.FindAsync(1);
            if (role is null)
            {
                context.Roles.Add(
                    new RoleEntity {
                        Id = 1,
                        Role = "Создатель"
                    }    
                );
            }
            else if (role.Role != "Создатель")
            {
                role.Role = "Создатель";
                await context.SaveChangesAsync();
            }

            await context.Database.MigrateAsync();
            await context.SaveChangesAsync();
        }


    }
}
