using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Models.EFDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;

namespace LocalDevicesSearcher.Infrastructure
{
    public interface IServiceProviderFactory
    {
    }
    public class ServiceProviderFactory : IServiceProviderFactory
    {
        public IServiceProvider ServiceProvider { get; }
        public ServiceProviderFactory(IConfiguration configuration)
        {
            IServiceCollection services = new ServiceCollection();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DeviceDbContext>(options => options.UseSqlServer(connectionString))
                        .AddTransient<IDeviceRepository, EFDeviceRepository>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
