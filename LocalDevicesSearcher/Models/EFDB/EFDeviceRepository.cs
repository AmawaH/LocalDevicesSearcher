using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace LocalDevicesSearcher.Models.EFDB
{
    public class EFDeviceRepository : IDeviceRepository
    {
        private readonly DeviceDbContext _context;
        public EFDeviceRepository(DeviceDbContext context)
        {
            _context = context;
        }
        public async Task<List<Device>> GetDevices()
        {
            return await _context.Devices.Include(d => d.OpenedPorts).ToListAsync();
        }
        public async Task<Device> GetDeviceById(int deviceId)
        {
            return await _context.Devices.Include(d => d.OpenedPorts).FirstOrDefaultAsync(d => d.Id == deviceId);
        }
        public async Task AddDevice(Device device)
        {
            _context.Devices.Add(device);
            _context.SaveChanges();
            foreach (var openedPort in device.OpenedPorts)
            {
                if (_context.Entry(openedPort).State == EntityState.Detached)
                {
                    openedPort.DeviceId = device.Id;
                    _context.OpenedPorts.Add(openedPort);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDevice(Device device)
        {
            _context.Entry(device).State = EntityState.Modified;
            foreach (var openedPort in device.OpenedPorts)
            {
                _context.Entry(openedPort).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteDevice(int deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);
            if (device != null)
            {
                _context.Devices.Remove(device);
                _context.OpenedPorts.RemoveRange(device.OpenedPorts);
                await _context.SaveChangesAsync();
            }
        }
    }
}