using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalDevicesSearcher.Models
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetDevices();
        Task<Device> GetDeviceById(int deviceId);
        Task AddDevice(Device device);
        Task UpdateDevice(Device device);
        Task DeleteDevice(int deviceId);
    }
}