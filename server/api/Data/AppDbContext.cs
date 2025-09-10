using api.Models;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Frontend", Category = "Web Dev" },
                new Role { Id = 2, Name = "Backend", Category = "Web Dev" },
                new Role { Id = 3, Name = "QA", Category = "Web Dev" },
                new Role { Id = 4, Name = "PM", Category = "Web Dev" }
            );
        }
    }
}
