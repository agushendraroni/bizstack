using Microsoft.EntityFrameworkCore;
using OrganizationService.Models;
using SharedLibrary.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrganizationService.Data
{
    public class OrganizationDbContext : DbContext
    {
        private readonly string _currentUser;

        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options, string currentUser = "system")
            : base(options)
        {
            _currentUser = currentUser;
        }

        // DbSets
        public DbSet<Company> Companies => Set<Company>();

        // Add more DbSets as needed, e.g.:
        // public DbSet<LegalDocument> LegalDocuments => Set<LegalDocument>();
        // public DbSet<JobTitle> JobTitles => Set<JobTitle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: If you use composite keys or custom relationships, define them here
            // modelBuilder.Entity<JobTitleDepartment>()
            //     .HasKey(jt => new { jt.JobTitleId, jt.DepartmentId });

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
