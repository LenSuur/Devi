using Devi.Models;

namespace Devi.Data.Repositories
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllDevices();
        Task<U> Get<U>(int id);
        Task<Device> Get(int id);
        Task AddDevice(Device device);
        Task UpdateDevice(Device device);
        Task DeleteDevice(int id);
    }
}
