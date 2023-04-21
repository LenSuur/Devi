using Devi.Data;
using Devi.Models;

public interface IDeviceService
{
    Task<List<DeviceModel>> GetAllDevices();
    Task<U> Get<U>(int id);
    Task AddDevice(DeviceModel device);
    Task UpdateDevice(DeviceModel device);
    Task DeleteDevice(int id);
}


