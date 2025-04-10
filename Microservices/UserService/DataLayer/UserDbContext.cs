using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities.UserService;

namespace UserService.DataLayer
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users => Set<UserEntity>();
    }
}
