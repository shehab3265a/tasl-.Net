using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace tasl_Net.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(new
        {
            id = 1,
            name = "Laptop",
            price = 25000
        });
    }
}