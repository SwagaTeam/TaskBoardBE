using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.ProjectService;
using System.Collections.Generic;

namespace ProjectService.DataLayer
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
        public DbSet<UserProjectEntity> UserProjects => Set<UserProjectEntity>();
        public DbSet<DocumentEntity> Documents => Set<DocumentEntity>();
        public DbSet<VisibilityLinkEntity> VisibilityLinks => Set<VisibilityLinkEntity>();
    }
}
