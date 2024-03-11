using Microsoft.Extensions.Configuration;
using System.IO;
namespace LocalDevicesSearcher.Infrastructure
{
    public class Config : IConfig
    {
        private IConfiguration configuration;
        public string ConnectionString { get; }
        public bool Range { get; }
        public int MinSubnetRange { get; }
        public int MaxSubnetRange { get; }
        public int[] SubnetCollection { get; }
        public Config()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            Range = configuration.GetValue<bool>("Constants:Range");
            MinSubnetRange = configuration.GetValue<int>("Constants:MinSubnetRange");
            MinSubnetRange = configuration.GetValue<int>("Constants:MaxSubnetRange");
            SubnetCollection = configuration.GetSection("Constants:SubnetCollection").Get<int[]>();
        }
    }
}