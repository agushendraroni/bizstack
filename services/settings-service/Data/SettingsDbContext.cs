using Microsoft.EntityFrameworkCore;
using SettingsService.Models;

namespace SettingsService.Data;

public class SettingsDbContext : DbContext
{
    public SettingsDbContext(DbContextOptions<SettingsDbContext> options) : base(options) { }

    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<Theme> Themes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // MenuItem configuration
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.MenuContext).HasMaxLength(50);
            entity.HasIndex(e => new { e.MenuContext, e.SortOrder });
            entity.HasIndex(e => e.ParentId);
            
            // Self-referencing relationship
            entity.HasOne(e => e.Parent)
                  .WithMany(e => e.Children)
                  .HasForeignKey(e => e.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // SystemSetting configuration
        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.DataType).HasMaxLength(50);
            entity.HasIndex(e => e.Key).IsUnique();
            entity.HasIndex(e => new { e.Category, e.SortOrder });
        });

        // Configuration configuration
        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ConfigType).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Environment).HasMaxLength(50);
            entity.HasIndex(e => new { e.Name, e.Environment }).IsUnique();
        });

        // Theme configuration
        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PrimaryColor).HasMaxLength(20);
            entity.Property(e => e.SecondaryColor).HasMaxLength(20);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Seed default data
        SeedDefaultData(modelBuilder);
    }

    private void SeedDefaultData(ModelBuilder modelBuilder)
    {
        // Default system settings
        var defaultSettings = new[]
        {
            new SystemSetting
            {
                Id = Guid.NewGuid(),
                Key = "app.name",
                Value = "BizStack",
                DefaultValue = "BizStack",
                Category = "general",
                Description = "Application name",
                CreatedAt = DateTime.UtcNow
            },
            new SystemSetting
            {
                Id = Guid.NewGuid(),
                Key = "app.version",
                Value = "2.0.0",
                DefaultValue = "2.0.0",
                Category = "general",
                Description = "Application version",
                IsEditable = false,
                CreatedAt = DateTime.UtcNow
            },
            new SystemSetting
            {
                Id = Guid.NewGuid(),
                Key = "company.name",
                Value = "Your Company",
                DefaultValue = "Your Company",
                Category = "company",
                Description = "Company name",
                CreatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<SystemSetting>().HasData(defaultSettings);

        // Default theme
        var defaultTheme = new Theme
        {
            Id = Guid.NewGuid(),
            Name = "default",
            DisplayName = "Default Theme",
            Description = "Default BizStack theme",
            IsDefault = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        modelBuilder.Entity<Theme>().HasData(defaultTheme);
    }
}