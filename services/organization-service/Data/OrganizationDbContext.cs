using Microsoft.EntityFrameworkCore;
using OrganizationService.Models;
using System;

namespace OrganizationService.Data;

public class OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
}
