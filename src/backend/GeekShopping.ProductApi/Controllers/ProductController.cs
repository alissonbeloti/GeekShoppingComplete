using GeekShopping.ProductApi.Data.ValueObjects;
using GeekShopping.ProductApi.Repository;
using GeekShopping.ProductApi.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Pomelo.EntityFrameworkCore.MySql.Query.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeekShopping.ProductApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductVO>>> Get()
    {
        var products = await _productRepository.FindAll();
        if (products == null) { return NotFound(); }
        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ProductVO>> FindById(long id)
    {
        var product = await _productRepository.FindById(id);
        if (product == null) { return NotFound(); }
        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductVO>> Post([FromBody] ProductVO request)
    {
        if (request == null) { return BadRequest(); }
        var product = await _productRepository.Create(request);
        
        return Created(product.Id.ToString(), product!);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<ProductVO>> Put(int id, [FromBody] ProductVO request)
    {
        if (request == null) { return BadRequest(); }
        var product = await _productRepository.Update(request);
        return Ok(product!);
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Delete(int id)
    {
        var ok = await _productRepository.Delete(id);
        if (ok) return Ok(); else return BadRequest();
    }
}
