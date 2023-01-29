using Devi.Models;
using Microsoft.EntityFrameworkCore;

namespace Devi.Data.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DeviContext _dataContext;

        public DeviceRepository(DeviContext dataContext)
        {
            _dataContext = dataContext;
        }        

        public async Task<List<Device>> GetAllDevices()
        {
            return await _dataContext.Device.ToListAsync();
        }
        public async Task<Device> GetDeviceById(int id)
        {
            return await _dataContext.Device.FindAsync(id);
        }

        public async Task AddDevice(Device device)
        {
            _dataContext.Device.Add(device);
            await _dataContext.SaveChangesAsync();
        }
        
        public async Task UpdateDevice(Device device)
        {
            _dataContext.Device.Update(device);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteDevice(int id)
        {
            var device = await _dataContext.Device.FindAsync(id);
            _dataContext.Device.Remove(device);
            await _dataContext.SaveChangesAsync();
        }

    }
}
