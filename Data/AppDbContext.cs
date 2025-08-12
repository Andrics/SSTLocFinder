using Microsoft.EntityFrameworkCore;
using PasswordLookupApp.Models;

namespace PasswordLookupApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
