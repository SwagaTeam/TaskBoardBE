using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.ProjectService;

namespace Contributors.DataLayer.Repositories;

public class ContributorsDbContext(DbContextOptions<ContributorsDbContext> options) : DbContext(options)
{
    public DbSet<UserProjectEntity> UserProjects { get; set; }
}