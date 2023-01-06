using Devi.Models;

namespace Devi.Data.Repositories
{
    public interface IDevicesRepository
    {
        Task<Device> Get(int id);
        void Save(Device device);
        void Delete(Device device);
        void Delete(int id);
        Task<IList<Device>> List();
    }
}
