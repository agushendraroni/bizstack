using Microsoft.EntityFrameworkCore;
using OrganizationService.Models;

namespace OrganizationService.Data;

public class OrganizationDbContext : DbContext
{
    public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<BusinessGroup> BusinessGroups { get; set; }
    public DbSet<CostCenter> CostCenters { get; set; }
    public DbSet<LegalDocument> LegalDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Company configuration
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Branch configuration
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.HasOne<Company>()
                  .WithMany()
                  .HasForeignKey(b => b.CompanyId);
        });

        // Division configuration
        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasOne<Company>()
                  .WithMany()
                  .HasForeignKey(d => d.CompanyId);
        });

        // JobTitle configuration
        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
        });

        // BusinessGroup configuration
        modelBuilder.Entity<BusinessGroup>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        // CostCenter configuration
        modelBuilder.Entity<CostCenter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
        });

        // LegalDocument configuration
        modelBuilder.Entity<LegalDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DocumentNumber).IsRequired().HasMaxLength(100);
            entity.HasOne<Company>()
                  .WithMany()
                  .HasForeignKey(l => l.CompanyId);
        });
    }
}
