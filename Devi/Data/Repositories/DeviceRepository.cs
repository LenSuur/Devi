using AutoMapper;
using AutoMapper.QueryableExtensions;
using Devi.Models;
using Microsoft.EntityFrameworkCore;

namespace Devi.Data.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DeviContext _dataContext;
        private readonly IMapper _objectMapper;

        public DeviceRepository(DeviContext dataContext, IMapper objectMapper)
        {
            _dataContext = dataContext;
            _objectMapper = objectMapper;
        }        

        public async Task<List<Device>> GetAllDevices()
        {
            return await _dataContext.Device.ToListAsync();
        }

        public async Task<U> Get<U>(int id)
        {
            return await _dataContext.Device
                .Where(device => device.Id == id)
                .ProjectTo<U>(_objectMapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Device> Get(int id)
        {
            return await _dataContext.Device
                .Where(device => device.Id == id)
                .FirstOrDefaultAsync();
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
