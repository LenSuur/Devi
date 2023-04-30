using AutoMapper;
using Devi.Data.Repositories;
using Devi.Data;
using Devi.Models;

namespace Devi.Services
    {
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;        
        private readonly IMapper _objectMapper;

        public DeviceService(IDeviceRepository deviceRepository, IMapper objectMapper)
        {
            _deviceRepository = deviceRepository;       
            _objectMapper = objectMapper;
        }

        public async Task<List<DeviceModel>> GetAllDevices()
        {
            List<Device> deviceList = await _deviceRepository.GetAllDevices();
            List<DeviceModel> deviceModelList = new List<DeviceModel>();
            foreach (var device in deviceList)
            {
                var deviceModel = _objectMapper.Map<Device, DeviceModel>(device);
                deviceModelList.Add(deviceModel);
            }
            
            return deviceModelList;
        }
        public async Task<U> Get<U>(int id)
        {
            return await _deviceRepository.Get<U>(id);
        }

        public async Task AddDevice(DeviceModel model)
        {
            var device = new Device();
            if (model.Id != 0)
            {
                device = await _deviceRepository.Get<Device>(model.Id);
            }

            _objectMapper.Map(model, device);
            
            await _deviceRepository.AddDevice(device);
        }

        public async Task UpdateDevice(DeviceModel model)
        {
            var device = await _deviceRepository.Get<Device>(model.Id);
            _objectMapper.Map(model, device);
            device.Id = model.Id;
            await _deviceRepository.UpdateDevice(device);
        }

        public async Task DeleteDevice(int id)
        {
            await _deviceRepository.DeleteDevice(id);
        }
    }
}