using uppgift1.Entities;
using Microsoft.EntityFrameworkCore;

namespace uppgift1.Data;

public class DataContext : DbContext
{
  public DbSet<Product> Products { get; set; }
  public DbSet<Supplier> Suppliers { get; set; }
  public DbSet<Supplierproduct> Supplierproducts { get; set; }
  
  public DataContext(DbContextOptions options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Supplierproduct>().HasKey(o => new { o.ProductId, o.SupplierId });
  }
}
