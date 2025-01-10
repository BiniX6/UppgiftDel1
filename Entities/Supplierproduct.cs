namespace uppgift1.Entities;

public class Supplierproduct
{
  public int SupplierId { get; set; }
  public int ProductId { get; set; }
  public double Price { get; set; }

  // Navigational properties...
  public Product Product { get; set; }
  public Supplier Supplier { get; set; }
}
