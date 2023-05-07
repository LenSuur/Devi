using Devi.Controllers;
using Devi.Data;
using Devi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DeviceModel = Devi.Models.DeviceModel;

namespace Devi.UnitTests.ControllerTests;

public class DeviceControllerTests
{
    private readonly Mock<IDeviceService> _mockDeviceService;
    private readonly Mock<IFileClient> _mockFileClient;
    private readonly DeviceController _controller;

    public DeviceControllerTests()
    {
        _mockDeviceService = new Mock<IDeviceService>();
        _mockFileClient = new Mock<IFileClient>();

        _controller = new DeviceController(_mockDeviceService.Object, _mockFileClient.Object);
    }
    
    private IFormFile CreateMockFormFile()
    {
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.FileName).Returns("test.txt");
        formFileMock.Setup(f => f.Length).Returns(10);
        formFileMock.Setup(f => f.OpenReadStream()).Returns(Stream.Null);
        var file = formFileMock.Object;
        return file;
    }

    [Fact]
    public async Task Index_Returns_ViewResult_With_DeviceList()
    {
        // Arrange
        var devices = new List<DeviceModel>
        {
            new DeviceModel
            {
                Id = 1,
                DeviceName = "Device 1",
                SerialNumber = "ABC123",
                Price = (decimal)9.99,
                Receipt = "receipt1.jpg",
                BoughtOn = new DateTime(2022, 1, 1),
                WarrantyTill = new DateTime(2023, 1, 1)
            },
            new DeviceModel
            {
                Id = 2,
                DeviceName = "Device 2",
                SerialNumber = "DEF456",
                Price = (decimal)19.9,
                Receipt = "receipt2.jpg",
                BoughtOn = new DateTime(2022, 2, 1),
                WarrantyTill = new DateTime(2023, 2, 1)
            },
            // Add more devices as needed
        };
        _mockDeviceService.Setup(d => d.GetAllDevices()).ReturnsAsync(devices);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(devices, viewResult.Model);
    }
    
    [Fact]
    public void Create_Returns_ViewResult()
    {
        // Arrange

        // Act
        var result = _controller.Create();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public async Task Details_should_return_notfound_when_id_is_missing()
    {
        // Arrange
        int? id = null;

        // Act
        var result = await _controller.Details(id) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Details_should_return_notfound_when_task_was_not_found()
    {
        // Arrange
        var id = 1;
        _mockDeviceService.Setup(service => service.Get<DeviceModel>(id)).ReturnsAsync((DeviceModel)null!);

        // Act
        var result = await _controller.Details(id) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Details_should_return_view_when_task_was_found()
    {
        // Arrange
        var id = 1;
        var device = new DeviceModel { Id = id };
        _mockDeviceService.Setup(service => service.Get<DeviceModel>(id))
            .ReturnsAsync(device);

        // Act
        var result = await _controller.Details(id) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var hasCorrectView = (result.ViewName == null || result.ViewName == "Details");
        Assert.True(hasCorrectView);
        Assert.Equal(device, result.Model);
    }

    [Fact]
    public async void DeleteConfirmed_should_redirect()
    {
        // Arrange
        var id = 1;
        string receipt = "receipt";
        _mockDeviceService.Setup(s => s.DeleteDevice(id))
            .Verifiable();

        // Act
        var result = await _controller.DeleteConfirmed(id, receipt) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        _mockDeviceService.VerifyAll();
    }
    
    [Fact]
    public async Task Create_Returns_Content_When_File_Is_Null()
    {
        // Arrange
        var model = new DeviceModel();
        IFormFile? file = null;

        // Act
        var result = await _controller.Create(model, file) as ContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file not selected", result.Content);
    }
    
    [Fact]
    public async Task Create_Calls_Save_Method_When_File_Is_Not_Null()
    {
        // Arrange
        DeviceModel model = new DeviceModel();
        var file = CreateMockFormFile();

        // Act
        var result = await _controller.Create(model, file) as RedirectToActionResult;

        // Assert
        _mockFileClient.Verify(f => f.Save(FileContainerNames.Receipts, It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
    
    [Fact]
    public async Task Create_Calls_AddDevice_Method()
    {
        // Arrange
        var model = new DeviceModel();
        var file = CreateMockFormFile();
        
        // Act
        await _controller.Create(model, file);

        // Assert
        _mockDeviceService.Verify(d => d.AddDevice(model), Times.Once);
    }
    
    [Fact]
    public async Task Edit_Returns_ViewResult_With_DeviceModel()
    {
        // Arrange
        int deviceId = 1; // Specify the desired device ID for the test
        var deviceModel = new DeviceModel
        {
            Id = 1,
            DeviceName = "Device 1",
            SerialNumber = "ABC123",
            Price = (decimal)9.99,
            Receipt = "receipt1.jpg",
            BoughtOn = new DateTime(2022, 1, 1),
            WarrantyTill = new DateTime(2023, 1, 1)
        };
        _mockDeviceService.Setup(d => d.Get<DeviceModel>(deviceId)).ReturnsAsync(deviceModel);

        // Act
        var result = await _controller.Edit(deviceId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(deviceModel, viewResult.Model);
    }
    
    [Fact]
    public async Task Edit_Redirects_To_Index_After_Updating_Device()
    {
        // Arrange
        var model = new DeviceModel { Id = 1,
            DeviceName = "Device 1",
            SerialNumber = "ABC123",
            Price = (decimal)9.99,
            Receipt = "receipt1.jpg",
            BoughtOn = new DateTime(2022, 1, 1),
            WarrantyTill = new DateTime(2023, 1, 1) };
        IFormFile file = null;

        var existingDevice = new Device { Id = 1, Receipt = "existing_receipt.jpg" };
        _mockDeviceService.Setup(d => d.Get<Device>(model.Id)).ReturnsAsync(existingDevice);
        _mockDeviceService.Setup(d => d.UpdateDevice(model)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Edit(model, file) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        _mockFileClient.Verify(f => f.Delete(FileContainerNames.Receipts, existingDevice.Receipt), Times.Never);
        _mockFileClient.Verify(f => f.Save(FileContainerNames.Receipts, It.IsAny<string>(), It.IsAny<Stream>()), Times.Never);
    }
    
    [Fact]
    public async Task Edit_Updates_Device_Without_File()
    {
        // Arrange
        var model = new DeviceModel { Id = 1,
            DeviceName = "Device 1",
            SerialNumber = "ABC123",
            Price = (decimal)9.99,
            Receipt = "receipt1.jpg",
            BoughtOn = new DateTime(2022, 1, 1),
            WarrantyTill = new DateTime(2023, 1, 1)  };
        IFormFile file = null;

        var existingDevice = new Device { Id = 1, Receipt = "existing_receipt.jpg" };
        _mockDeviceService.Setup(d => d.Get<Device>(model.Id)).ReturnsAsync(existingDevice);
        _mockDeviceService.Setup(d => d.UpdateDevice(model)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Edit(model, file);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToActionResult>(result);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        _mockFileClient.Verify(f => f.Delete(FileContainerNames.Receipts, existingDevice.Receipt), Times.Never);
        _mockFileClient.Verify(f => f.Save(FileContainerNames.Receipts, It.IsAny<string>(), It.IsAny<Stream>()), Times.Never);
    }
    
    [Fact]
    public async Task Edit_Updates_Device_With_File()
    {
        // Arrange
        var model = new DeviceModel { Id = 1,
            DeviceName = "Device 1",
            SerialNumber = "ABC123",
            Price = (decimal)9.99,
            Receipt = "receipt1.jpg",
            BoughtOn = new DateTime(2022, 1, 1),
            WarrantyTill = new DateTime(2023, 1, 1)  };
        var fileMock = new Mock<IFormFile>();
        var streamMock = new Mock<Stream>();
        fileMock.Setup(f => f.OpenReadStream()).Returns(streamMock.Object);
        fileMock.Setup(f => f.FileName).Returns("new_receipt.jpg");

        var existingDevice = new Device { Id = 1, Receipt = "existing_receipt.jpg" };
        _mockDeviceService.Setup(d => d.Get<Device>(model.Id)).ReturnsAsync(existingDevice);
        _mockDeviceService.Setup(d => d.UpdateDevice(model)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Edit(model, fileMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToActionResult>(result);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        _mockFileClient.Verify(f => f.Delete(FileContainerNames.Receipts, existingDevice.Receipt), Times.Once);
        _mockFileClient.Verify(f => f.Save(FileContainerNames.Receipts, "new_receipt.jpg", streamMock.Object), Times.Once);
    }
    
    [Fact]
    public async Task Delete_Returns_ViewResult_With_DeviceModel()
    {
        // Arrange
        int deviceId = 1;
        var deviceModel = new DeviceModel { Id = 1,
            DeviceName = "Device 1",
            SerialNumber = "ABC123",
            Price = (decimal)9.99,
            Receipt = "receipt1.jpg",
            BoughtOn = new DateTime(2022, 1, 1),
            WarrantyTill = new DateTime(2023, 1, 1)  };
        _mockDeviceService.Setup(d => d.Get<DeviceModel>(deviceId)).ReturnsAsync(deviceModel);

        // Act
        var result = await _controller.Delete(deviceId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(deviceModel, viewResult.Model);
    }
}