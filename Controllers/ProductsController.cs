using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    // Set the base route for the controller as "api/products"
    [Route("api/[controller]")]
    
    // Mark the class as an API controller which enables automatic model binding and validation
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Private readonly field for IProductService dependency
        private readonly IProductService _productService;

        // Constructor to inject IProductService dependency
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // Endpoint to get a product by its ID (HTTP GET: /api/products/{id})
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            // Call the service to fetch the product by ID asynchronously
            var product = await _productService.GetProductByIdAsync(id);

            // If the product is not found, throw an exception
            if (product == null)
            {
                 return NotFound(new ErrorResponse { Message = $"Product with ID {id} not found." });
            }

            // Return 200 OK with the product if found
            return Ok(product);
        }

        // Endpoint to retrieve all products (HTTP GET: /api/products)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            // Fetch all products asynchronously from the service
            var products = await _productService.GetAllProductsAsync();
            
            // Return 200 OK with the list of products
            return Ok(products);
        }

        // Endpoint to add a new product (HTTP POST: /api/products)
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            // Validate if the product object or its name is missing
            if (product == null || string.IsNullOrWhiteSpace(product.ProductName))
            {
                return BadRequest(new { message = "Product name is required." });
            }

            // Add the new product using the service asynchronously
            await _productService.AddProductAsync(product);

            // Return 201 Created with a link to the newly created product
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product);
        }
    }
}
