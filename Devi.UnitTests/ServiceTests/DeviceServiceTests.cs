using AutoMapper;
using Devi.Data;
using Devi.Data.Repositories;
using Devi.MappingProfiles;
using Devi.Models;
using Devi.Services;
using Moq;

namespace Devi.UnitTests.ServiceTests;

public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
    private readonly DeviceService _deviceService;
    
    public DeviceServiceTests()
    {
        _deviceRepositoryMock = new Mock<IDeviceRepository>();
        
        var mapperConfig = new MapperConfiguration(conf =>
        {
            conf.AddProfile(new DeviceProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        
        _deviceService = new DeviceService(_deviceRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task Delete_should_call_repository_DeleteDevice()
    {
        // Arrange
        var deviceId = 1;
        _deviceRepositoryMock.Setup(x => x.DeleteDevice(deviceId)).Verifiable();
        
        // Act
        await _deviceService.DeleteDevice(deviceId);
        
        // Assert
        _deviceRepositoryMock.VerifyAll();
    }

    [Fact]
    public async Task Save_should_add_new_device()
    {
        //Arrange
        var model = new DeviceModel();
        model.Id = 1;
        var device = new Device();
        device.Id = 1;
        
        _deviceRepositoryMock.Setup(x => x.Get<Device>(model.Id))
            .ReturnsAsync(device)
            .Verifiable();
        _deviceRepositoryMock.Setup(x => x.AddDevice(It.IsAny<Device>()))
            .Verifiable();
        
        //Act
        await _deviceService.AddDevice(model);
        
        //Arrange
        _deviceRepositoryMock.VerifyAll();
    }

    [Fact]
    public async Task Get_should_call_repository_get()
    {
        //Arrange
        var deviceId = 1;
        var model = new DeviceModel();
        _deviceRepositoryMock.Setup(x => x.Get<DeviceModel>(deviceId))
            .ReturnsAsync(model)
            .Verifiable();
        
        //Act
        var result = await _deviceService.Get<DeviceModel>(deviceId);
        
        //Assert
        Assert.Equal(model, result);
        _deviceRepositoryMock.VerifyAll();
    }
    
    [Fact]
    public async Task GetAllDevices_should_call_repository_GetAllDevices()
    {
        //Arrange
        var deviceList = new List<Device>();
        var deviceModelList = new List<DeviceModel>();
        _deviceRepositoryMock.Setup(x => x.GetAllDevices())
            .ReturnsAsync(deviceList)
            .Verifiable();
        
        //Act
        var result = await _deviceService.GetAllDevices();
        
        //Assert
        Assert.Equal(deviceModelList, result);
        _deviceRepositoryMock.VerifyAll();
    }
    
    [Fact]
    public async Task Update_should_call_repository_UpdateDevice()
    {
        //Arrange
        var model = new DeviceModel();
        model.Id = 1;
        var device = new Device();
        device.Id = 1;
        
        _deviceRepositoryMock.Setup(x => x.Get<Device>(model.Id))
            .ReturnsAsync(device)
            .Verifiable();
        _deviceRepositoryMock.Setup(x => x.UpdateDevice(It.IsAny<Device>()))
            .Verifiable();
        
        //Act
        await _deviceService.UpdateDevice(model);
        
        //Arrange
        _deviceRepositoryMock.VerifyAll();
    }

}