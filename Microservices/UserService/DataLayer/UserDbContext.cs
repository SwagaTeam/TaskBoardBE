using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Entities.UserService;

namespace UserService.DataLayer
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users => Set<UserEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
