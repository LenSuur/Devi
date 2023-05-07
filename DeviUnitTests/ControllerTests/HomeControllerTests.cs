using Devi.Controllers;
using Devi.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DeviUnitTests.ControllerTests;

public class HomeControllerTests
{
    [Fact]
    public void Index_should_return_index_view()
    {
        // Arrange
        var mockDeviceService = new Mock<IDeviceService>();
        var mockFileClient = new Mock<IFileClient>();
        var controller = new DeviceController(mockDeviceService.Object, mockFileClient.Object);

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}