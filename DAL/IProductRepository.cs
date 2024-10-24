using ProductApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApi.DAL
{
    public interface IProductRepository
    {
        // Fetch a product by its ID from the data source
        Task<Product> GetProductByIdAsync(int productId);

        // Fetch all products from the data source
        Task<IEnumerable<Product>> GetAllProductsAsync(); // Corrected this line

        // Add a new product to the data source
        Task AddProductAsync(Product product);
    }
}
