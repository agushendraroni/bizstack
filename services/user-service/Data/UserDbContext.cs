using Microsoft.EntityFrameworkCore;
using FrameworkX.Services.Users.Models;

namespace FrameworkX.Services.Users.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // Contoh DbSet entity User
    }
}