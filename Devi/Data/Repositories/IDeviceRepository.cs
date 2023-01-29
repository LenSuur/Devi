using Devi.Models;

namespace Devi.Data.Repositories
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllDevices();
        Task<Device> GetDeviceById(int id);
        Task AddDevice(Device device);
        Task UpdateDevice(Device device);
        Task DeleteDevice(int id);
    }
}
