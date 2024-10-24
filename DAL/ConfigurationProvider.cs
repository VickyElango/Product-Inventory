// DAL/ConfigurationProvider.cs
using Microsoft.Extensions.Configuration;

namespace ProductApi.DAL
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method to get a connection string by name
        public string GetConnectionString(string name)
        {
            
            return _configuration.GetConnectionString(name);
        }
    }
}
