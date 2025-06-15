using Microsoft.EntityFrameworkCore;
using UserService.Models;
using SharedLibrary.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UserService.Data
{
    public class UserDbContext : DbContext
    {
        private readonly string _currentUser;

        public UserDbContext(DbContextOptions<UserDbContext> options, string currentUser = "system")
            : base(options)
        {
            _currentUser = currentUser;
        }

        // DbSets
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<UserPreference> UserPreferences => Set<UserPreference>();
        public DbSet<UserActivityLog> UserActivityLogs => Set<UserActivityLog>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: If you use composite keys or custom relationships, define them here

            // Optional: Global filters (e.g., soft delete)
            modelBuilder.Entity<Company>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && 
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.CreatedBy ??= _currentUser;
                    entity.IsActive = true;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ChangedAt = now;
                    entity.ChangedBy = _currentUser;
                }
            }
        }
    }
}
