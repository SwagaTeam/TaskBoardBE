﻿using Microsoft.EntityFrameworkCore;
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
            await context.Database.MigrateAsync();
            await InitRole(context);
            await InitItemType(context);
            await context.SaveChangesAsync();
        }

        private static async Task InitRole(ProjectDbContext context)
        {
            var firstRole = await context.Roles.FindAsync(1);
            var secondRole = await context.Roles.FindAsync(2);
            if (firstRole is null)
            {
                context.Roles.Add(
                    new RoleEntity {
                        Id = 1,
                        Role = "Создатель"
                    }   
                );
                
            }
            else if (firstRole.Role != "Создатель")
            {
                firstRole.Role = "Создатель";
            }

            if (secondRole is null)
            {
                context.Roles.Add(
                    new RoleEntity()
                    {
                        Id = 2,
                        Role = "Новый пользователь"
                    });
            }
            else if (secondRole.Role != "Новый пользователь")
            {
                firstRole.Role = "Новый пользователь";
            }
        }

        private static async Task InitItemType(ProjectDbContext context)
        {
            var type = context.ItemTypes;
            if (await type.CountAsync() < 3)
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
