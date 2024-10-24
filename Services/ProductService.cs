// Services/ProductService.cs
using Microsoft.Extensions.Caching.Memory;
using ProductApi.DAL;
using ProductApi.Models;

namespace ProductApi.Services
{
    // Service class to manage business logic related to products, with caching
    public class ProductService : IProductService
    {
        // Dependency on product repository for data access and memory cache for caching
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;

        // Cache keys for storing/retrieving products from cache
        private const string AllProductsCacheKey = "allProducts";
        private const string ProductCacheKeyPrefix = "product_";

        // Constructor to inject the repository and memory cache dependencies
        public ProductService(IProductRepository productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        // Method to get a product by ID, with caching logic
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            // Construct a unique cache key for the specific product
            string cacheKey = $"{ProductCacheKeyPrefix}{productId}";

            // Try to retrieve the product from the cache
            if (!_cache.TryGetValue(cacheKey, out Product product))
            {
                // If not found in cache, retrieve it from the repository
                product = await _productRepository.GetProductByIdAsync(productId);

                // If product is found, cache it with expiration policies
                if (product != null)
                {
                    _cache.Set(cacheKey, product, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),  // Cache expires after 5 minutes
                        SlidingExpiration = TimeSpan.FromMinutes(2)  // Resets expiration if accessed within 2 minutes
                    });
                }
            }

            // Return the product, either from cache or repository
            return product;
        }

        // Method to get all products, with caching logic
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            // Try to retrieve all products from the cache
            if (!_cache.TryGetValue(AllProductsCacheKey, out IEnumerable<Product> products))
            {
                // If not found in cache, retrieve them from the repository
                products = await _productRepository.GetAllProductsAsync();

                // Cache the product list with expiration policies
                _cache.Set(AllProductsCacheKey, products, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),  // Cache expires after 5 minutes
                    SlidingExpiration = TimeSpan.FromMinutes(2)  // Resets expiration if accessed within 2 minutes
                });
            }

            // Return the list of products, either from cache or repository
            return products;
        }

        // Method to add a new product, which also clears the product cache
        public async Task AddProductAsync(Product product)
        {
            // Add the product to the repository (database)
            await _productRepository.AddProductAsync(product);

            // Remove the cached list of products to force refresh on next retrieval
            _cache.Remove(AllProductsCacheKey);
        }
    }
}
