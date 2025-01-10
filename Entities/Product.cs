using System.Text.Json.Serialization;

namespace uppgift1.Entities;

public class Product
{
  public int ProductId { get; set; }
  public string ItemNumber { get; set; }
  public string ProductName { get; set; }
  public string Description { get; set; }

  [JsonIgnore]
  public IList<Supplierproduct> Supplierproducts { get; set; }
}
