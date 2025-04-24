using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.ProjectService;

namespace Contributors.DataLayer.Repositories;

public class ContributorsDbContext(DbContextOptions<ContributorsDbContext> options) : DbContext(options)
{
    public DbSet<UserProjectEntity> UserProjects { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}