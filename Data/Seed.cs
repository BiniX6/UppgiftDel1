using System.Text.Json;
using uppgift1.Entities;

namespace uppgift1.Data;

public static class Seed
{
  public static async Task LoadProducts(DataContext context)
  {
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    if (context.Products.Any()) return;

    var json = File.ReadAllText("Data/json/products.json");
    var products = JsonSerializer.Deserialize<List<Product>>(json, options);

    if (products is not null && products.Count > 0)
    {
      await context.Products.AddRangeAsync(products);
      await context.SaveChangesAsync();
    }
  }

  public static async Task LoadSuppliers(DataContext context)
  {
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    if (context.Suppliers.Any()) return;

    var json = File.ReadAllText("Data/json/suppliers.json");
    var suppliers = JsonSerializer.Deserialize<List<Supplier>>(json, options);

    if (suppliers is not null && suppliers.Count > 0)
    {
      await context.Suppliers.AddRangeAsync(suppliers);
      await context.SaveChangesAsync();
    }
  }

  public static async Task LoadSupplierproducts(DataContext context)
  {
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    if (context.Supplierproducts.Any()) return;

    var json = File.ReadAllText("Data/json/supplierproducts.json");
    var supplierproducts = JsonSerializer.Deserialize<List<Supplierproduct>>(json, options);

    if (supplierproducts is not null && supplierproducts.Count > 0)
    {
      foreach (var sp in supplierproducts)
      {
        var product = await context.Products.FindAsync(sp.ProductId);
        var supplier = await context.Suppliers.FindAsync(sp.SupplierId);

        if (product != null && supplier != null)
        {
          sp.Product = product;
          sp.Supplier = supplier;
          context.Supplierproducts.Add(sp);
        }
      }
      await context.SaveChangesAsync();
    }
  }
}