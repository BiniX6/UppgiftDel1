using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uppgift1.Data;

namespace uppgift1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet()]
    public async Task<ActionResult> ListAllSuppliers()
    {
        var suppliers = await _context.Suppliers
          .Include(sp => sp.Supplierproducts)
          .ThenInclude(p => p.Product)
          .Select(supplier => new
          {
              supplier.SupplierId,
              supplier.SupplierName,
              supplier.SupplierAdress,
              supplier.SupplierCity,
              supplier.SupplierPostalcode,
              supplier.Contactperson,
              supplier.Phone,
              supplier.Epost,
              Products = supplier.Supplierproducts.Select(sp => new
              {
                  sp.Product.ProductName,
                  sp.Price
              })
          })
          .ToListAsync();
        return Ok(new { success = true, data = suppliers });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindSpecificSupplier(int id)
    {
        var suppliers = await _context.Suppliers
          .Where(s => s.SupplierId == id)
          .Include(sp => sp.Supplierproducts)
          .ThenInclude(p => p.Product)
          .Select(supplier => new
          {
              supplier.SupplierId,
              supplier.SupplierName,
              supplier.SupplierAdress,
              supplier.SupplierCity,
              supplier.SupplierPostalcode,
              supplier.Contactperson,
              supplier.Phone,
              supplier.Epost,
              Products = supplier.Supplierproducts.Select(sp => new
              {
                  sp.Product.ItemNumber,
                  sp.Product.ProductName,
                  sp.Price,
                  sp.Product.Description
              })
          })
          .ToListAsync();
        return Ok(new { success = true, data = suppliers });
    }
}
