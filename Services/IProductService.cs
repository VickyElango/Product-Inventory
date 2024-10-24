using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IProductService
    {
        // Fetch a product by its ID
        Task<Product> GetProductByIdAsync(int productId);

        // Fetch all products
        Task<IEnumerable<Product>> GetAllProductsAsync();

        // Add a new product
        Task AddProductAsync(Product product);
    }
}
