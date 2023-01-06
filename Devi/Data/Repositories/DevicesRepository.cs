using Devi.Models;
using Microsoft.EntityFrameworkCore;

namespace Devi.Data.Repositories
{
    public class DevicesRepository : IDevicesRepository
    {
        private readonly DeviContext _dataContext;

        public DevicesRepository(DeviContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Device> Get(int id)
        {
            return await _dataContext.Device.FindAsync(id);
        }

        public async Task Save(Device device)
        {
            if (device.Id == 0)
            {
                await _dataContext.Device.AddAsync(device);
            }
            else
            {
                _dataContext.Device.Update(device);
            }
        }

        public async Task Delete(Device device)
        {
            _dataContext.Device.Remove(device);
        }

        public async Task<IList<Device>> List()
        {
            return await _dataContext.Device.ToListAsync();
        }

        void IDevicesRepository.Save(Device device)
        {
            throw new NotImplementedException();
        }

        void IDevicesRepository.Delete(Device device)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
