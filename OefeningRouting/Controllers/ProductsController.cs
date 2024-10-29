using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OefeningRouting.Models;
using OefeningRouting.Services;

namespace OefeningRouting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _productsService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}/details")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productsService.GetProductById(id);
            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> SearchProductsByName([FromQuery(Name = "name")] string name)
        {
            var products = await _productsService.SearchProductsByName(name);
            if (products is null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("category/{categoryName:alpha}/price/{minPrice:decimal}-{maxPrice:decimal}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategoryAndPrice([FromRoute] string categoryName, decimal minPrice, decimal maxPrice)
        {
            var products = await _productsService.GetProductsByCategoryAndPrice(categoryName, minPrice, maxPrice);
            if (products is null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _productsService.CreateProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var updatedProduct = await _productsService.UpdateProduct(id, product);
            if (updatedProduct is null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpPut("{id:int}/discount/{percentage:int}")]
        public async Task<ActionResult<Product>> ApplyDiscountToProduct(int id, int percentage)
        {
            var updatedProduct = await _productsService.ApplyDiscountToProduct(id, percentage);
            if (updatedProduct is null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productsService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productsService.DeleteProduct(id);
            return NoContent();
        }

        [HttpDelete("delete/multiple/voorbeeld")]
        public async Task<ActionResult> DeleteProduct([FromQuery] string ids)
        {
            var idList = ids.Split(",").Select(int.Parse).ToList();
            await _productsService.DeleteMultipleProducts(idList);
            return NoContent();
        }

    }
}
