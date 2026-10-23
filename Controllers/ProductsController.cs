using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tasl_Net.Data;
using tasl_Net.DTOs;

namespace tasl_Net.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
public async Task<IActionResult> GetProducts(
    int page = 1,
    int pageSize = 10)
{
    if (page < 1)
        page = 1;

    if (pageSize < 1)
        pageSize = 10;

    if (pageSize > 100)
        pageSize = 100;

    var products = await _context.Products
        .AsNoTracking()
        .Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            CategoryName = p.Category.Name,
            RowVersion = p.RowVersion
        })
        .OrderBy(p => p.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(products);
}

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
        int id,
        UpdateProductDto dto)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        _context.Entry(product)
            .Property(p => p.RowVersion)
            .OriginalValue = dto.RowVersion;

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new
            {
                message = "The product was modified by another user."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        product.IsDeleted = true;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}