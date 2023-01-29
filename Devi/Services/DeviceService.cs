using Devi.Data.Repositories;
using Devi.Data;
using Devi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devi.Services
    {
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;        

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;            
        }

        public async Task<List<Device>> GetAllDevices()
        {
            return await _deviceRepository.GetAllDevices();
        }
        public async Task<Device> GetDeviceById(int id)
        {
            return await _deviceRepository.GetDeviceById(id);
        }

        public async Task AddDevice(Device device)
        {
            await _deviceRepository.AddDevice(device);
        }

        public async Task UpdateDevice(Device device)
        {
            await _deviceRepository.UpdateDevice(device);
        }

        public async Task DeleteDevice(int id)
        {
            await _deviceRepository.DeleteDevice(id);
        }
    }
}