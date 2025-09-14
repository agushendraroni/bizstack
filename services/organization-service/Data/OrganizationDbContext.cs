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
            entity.HasKey(e => e.TenantId);
            entity.Property(e => e.TenantId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Website).HasMaxLength(200);
            entity.Property(e => e.NPWP).HasMaxLength(20);
            entity.Property(e => e.NIB).HasMaxLength(20);
            entity.Property(e => e.SIUP).HasMaxLength(20);
        });

        // Branch configuration
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.HasOne(b => b.Company)
                  .WithMany(c => c.Branches)
                  .HasForeignKey(b => b.TenantId)
                  .HasPrincipalKey(c => c.TenantId);
        });

        // Division configuration
        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasOne(d => d.Company)
                  .WithMany()
                  .HasForeignKey(d => d.TenantId)
                  .HasPrincipalKey(c => c.TenantId);
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
            entity.HasOne(cc => cc.Division)
                  .WithMany(d => d.CostCenters)
                  .HasForeignKey(cc => cc.DivisionId);
        });

        // LegalDocument configuration
        modelBuilder.Entity<LegalDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DocumentNumber).IsRequired().HasMaxLength(100);
            entity.HasOne(l => l.Company)
                  .WithMany(c => c.LegalDocuments)
                  .HasForeignKey(l => l.TenantId)
                  .HasPrincipalKey(c => c.TenantId);
        });
    }
}
