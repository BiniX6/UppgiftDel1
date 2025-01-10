using System.Text.Json.Serialization;
namespace uppgift1.Entities;

public class Supplier
{
  public int SupplierId { get; set; }
  public string SupplierName { get; set; }
  public string SupplierAdress { get; set; }
  public string SupplierCity { get; set; }
  public string SupplierPostalcode { get; set; }
  public string Contactperson { get; set; }
  public string Phone { get; set; }
  public string Epost { get; set; }

  [JsonIgnore]
  public IList<Supplierproduct> Supplierproducts { get; set; }
}
