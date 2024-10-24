// DAL/IConfigurationProvider.cs
namespace ProductApi.DAL
{
    // Interface for configuration provider
    public interface IConfigurationProvider
    {
        // Method to get connection string by name
        string GetConnectionString(string name);
    }
}
