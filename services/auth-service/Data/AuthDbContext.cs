using Microsoft.EntityFrameworkCore;
using AuthService.Models;
using SharedLibrary.Entities;

namespace AuthService.Data
{
    public class AuthDbContext : DbContext
    {

        // DbSets
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserPasswordHistory> UserPasswordHistories { get; set; }



        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ======= Composite Keys =======
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<MenuPermission>()
                .HasKey(mp => new { mp.MenuId, mp.PermissionId });

            // ======= Relationships =======

            // Menu Parent-Child (Self Reference)
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Company ↔ Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);

            // Company ↔ Roles
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Company)
                .WithMany(c => c.Roles)
                .HasForeignKey(r => r.CompanyId);


            // Company ↔ Permissions
            modelBuilder.Entity<Permission>()
                .HasOne(p => p.Company)
                .WithMany(c => c.Permissions)
                .HasForeignKey(p => p.CompanyId);

            // Company ↔ Menus
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Company)
                .WithMany(c => c.Menus)
                .HasForeignKey(m => m.CompanyId);

    

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);


            modelBuilder.Entity<UserPasswordHistory>()
                .HasOne(p => p.User)
                .WithMany(u => u.PasswordHistories)
                .HasForeignKey(p => p.UserId);


        }

        private readonly string _currentUser;

        // Jika kamu punya mekanisme mendapatkan user yang sedang login (misal lewat IHttpContextAccessor),
        // bisa inject dan simpan ke _currentUser.
        // Untuk demo, bisa di-set manual lewat constructor.

        public AuthDbContext(DbContextOptions<AuthDbContext> options, string currentUser = "system") : base(options)
        {
            _currentUser = currentUser;
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
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is not BaseEntity entity) continue;

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
