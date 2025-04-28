using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Constants;
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
            await InitRole(context);
            await InitItemType(context);
            await context.Database.MigrateAsync();
            await context.SaveChangesAsync();
        }

        private static async Task InitRole(ProjectDbContext context)
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
            }
        }

        private static async Task InitItemType(ProjectDbContext context)
        {
            var type = context.ItemTypes;
            if (type.Count() < 3)
            {
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"ItemTypes\" RESTART IDENTITY CASCADE");
                for (var i = 0; i < 3; i++)
                {
                    var entity = new ItemTypeEntity
                    {
                        Level = ItemType.Names[i]
                    };
                    
                    context.ItemTypes.Add(entity);
                }
            } 
        }
    }
}
