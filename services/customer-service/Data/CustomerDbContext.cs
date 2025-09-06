using Microsoft.EntityFrameworkCore;
using CustomerService.Models;

namespace CustomerService.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerGroup> CustomerGroups { get; set; }
    public DbSet<CustomerContact> CustomerContacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalSpent).HasPrecision(18, 2);
            
            entity.HasOne(e => e.CustomerGroup)
                  .WithMany(cg => cg.Customers)
                  .HasForeignKey(e => e.CustomerGroupId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // CustomerGroup configuration
        modelBuilder.Entity<CustomerGroup>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DiscountPercentage).HasPrecision(5, 2);
            entity.Property(e => e.MinimumSpent).HasPrecision(18, 2);
        });

        // CustomerContact configuration
        modelBuilder.Entity<CustomerContact>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Contacts)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
