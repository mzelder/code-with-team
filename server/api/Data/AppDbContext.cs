using api.Models;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        // Konstruktor wymagany przez EF Core
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet dla tabeli Users
        public DbSet<User> Users { get; set; }
    }
}
