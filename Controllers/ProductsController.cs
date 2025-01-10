using uppgift1.Data;
using uppgift1.Entities;
using uppgift1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace uppgift1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(DataContext context) : ControllerBase
{
  private readonly DataContext _context = context;

  [HttpGet("{id}")]
  public async Task<ActionResult> FindSpecificProduct(int id)
  {
    var product = await _context.Products
      .Where(c => c.ProductId == id)
      .Include(p => p.Supplierproducts)
      .ThenInclude(sp => sp.Supplier)
      .Select(product => new
      {
        product.ProductId,
        product.ItemNumber,
        product.ProductName,
        product.Description,
        Supplierproducts = product.Supplierproducts.Select(sp => new
        {
          sp.Supplier.SupplierName,
          sp.Price
        })
      }
      )
      .ToListAsync();
    return Ok(new { success = true, product });
  }

  [HttpPost("{id}")]
  public async Task<ActionResult> CreateProduct(int id, ProductViewModel product)
  {
    var supplier = await _context.Suppliers
        .Where(s => s.SupplierId == id)
        .SingleOrDefaultAsync();

    if (supplier == null)
    {
      return BadRequest(new { success = false, StatusCode = 400, message = "Supplier was not found!" });
    }
    var existingProduct = await _context.Products
        .Where(p => p.ItemNumber == product.ItemNumber)
        .SingleOrDefaultAsync();
    if (existingProduct != null)
    {
      return BadRequest(new { success = false, StatusCode = 400, message = "This Product already exists!" });
    }

    var newProduct = new Product
    {
      ItemNumber = product.ItemNumber,
      ProductName = product.ProductName,
      Description = product.Description
    };

    _context.Products.Add(newProduct);

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
      return BadRequest(new { success = false, StatusCode = 400, message = "Something went really wrong!?!?" });
    }

    var newSupplierproduct = new Supplierproduct
    {
      SupplierId = product.SupplierId,
      ProductId = newProduct.ProductId,
      Price = product.Price
    };

    _context.Supplierproducts.Add(newSupplierproduct);

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
      return BadRequest(new { success = false, StatusCode = 400, message = "Something went really wrong!?!?" });
    }
    return CreatedAtAction(nameof(FindSpecificProduct), new { id = newProduct.ProductId }, new { success = true, StatusCode = 201, data = newProduct });
  }

  [HttpPatch("{supplierid}/{productid}")]
  public async Task<ActionResult> UpdatePrice(int supplierid, int productid, ProductPriceViewModel product)
  {
    var priceToUpdate = await _context.Supplierproducts
        .Where(x => x.SupplierId == supplierid && x.ProductId == productid)
        .SingleOrDefaultAsync();

    if (priceToUpdate == null)
    {
      return BadRequest(new { success = false, StatusCode = 400, message = "Product not found!" });
    }
    priceToUpdate.Price = product.Price;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
      return BadRequest(new { success = false, StatusCode = 400, message = "Something went really wrong!" });
    }

    return Ok(new {success = true, message = "Price updated!"});
  }

}