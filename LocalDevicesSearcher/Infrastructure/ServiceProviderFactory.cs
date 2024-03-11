using LocalDevicesSearcher.Models;
using LocalDevicesSearcher.Models.EFDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LocalDevicesSearcher.Infrastructure
{
    public interface IServiceProviderFactory
    {
        public IServiceProvider ServiceProvider { get; }
    }
    public class ServiceProviderFactory : IServiceProviderFactory
    {
        public IServiceProvider ServiceProvider { get; }
        public ServiceProviderFactory(IConfig config)
        {
            IServiceCollection services = new ServiceCollection();
            string connectionString = config.ConnectionString;
            services.AddDbContext<DeviceDbContext>(options => options.UseSqlServer(connectionString))
                        .AddTransient<IDeviceRepository, EFDeviceRepository>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}