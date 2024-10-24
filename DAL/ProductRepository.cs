using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ProductApi.Models;

namespace ProductApi.DAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfigurationProvider configurationProvider)
        {
            _connectionString = configurationProvider.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string cannot be null.");
        }

        private async Task<DataSet> GetProductsDataSetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Products", connection);
                var dataAdapter = new SqlDataAdapter(command);
                var dataSet = new DataSet();
                await Task.Run(() => dataAdapter.Fill(dataSet));
                return dataSet;
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var dataSet = await GetProductsDataSetAsync();
            var productsTable = dataSet.Tables[0];

            var product = (from DataRow row in productsTable.Rows
                           where (int)row["ProductID"] == productId
                           select new Product
                           {
                               ProductID = (int)row["ProductID"],
                               ProductName = (string)row["ProductName"],
                               CategoryID = (int)row["CategoryID"],
                               SupplierID = (int)row["SupplierID"],
                               UnitPrice = (decimal)row["UnitPrice"],
                               UnitsInStock = (int)row["UnitsInStock"],
                               Discontinued = (bool)row["Discontinued"]
                           }).FirstOrDefault();

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() // This method signature is correct
        {
            var dataSet = await GetProductsDataSetAsync();
            var productsTable = dataSet.Tables[0];

            var tasks = productsTable.AsEnumerable().Select(row => Task.Run(() =>
            {
                return new Product
                {
                    ProductID = (int)row["ProductID"],
                    ProductName = (string)row["ProductName"],
                    CategoryID = (int)row["CategoryID"],
                    SupplierID = (int)row["SupplierID"],
                    UnitPrice = (decimal)row["UnitPrice"],
                    UnitsInStock = (int)row["UnitsInStock"],
                    Discontinued = (bool)row["Discontinued"]
                };
            }));

            return await Task.WhenAll(tasks);
        }

        public async Task AddProductAsync(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("INSERT INTO Products (ProductName, CategoryID, SupplierID, UnitPrice, UnitsInStock, Discontinued) VALUES (@ProductName, @CategoryID, @SupplierID, @UnitPrice, @UnitsInStock, @Discontinued)", connection);

                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                command.Parameters.AddWithValue("@SupplierID", product.SupplierID);
                command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                command.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock);
                command.Parameters.AddWithValue("@Discontinued", product.Discontinued);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
