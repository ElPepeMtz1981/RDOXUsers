using RODXUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace RODXUsers.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<Models.Users> Users => Set<Users>();

        public DbSet<Rols> Roles => Set<Rols>();

        public DbSet<ViewUserRol> ViewUserRol { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewUserRol>().HasNoKey().ToView("ViewUserRol");
        }

    }
}