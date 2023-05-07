using Devi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Devi.UnitTests.ControllerTests;

public class HomeControllerTests
{
    private readonly HomeController _controller;
    
    public HomeControllerTests()
    {
        var nullLogger = (ILogger<HomeController>)null!;
        _controller = new HomeController(nullLogger);
    }
    
    [Fact]
    public void Index_should_return_index_view()
    {
        // Arrange

        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var hasCorrectView = (result.ViewName == null || result.ViewName == "Index");
        Assert.True(hasCorrectView);
    }
    
    [Fact]
    public void Privacy_should_return_privacy_view()
    {
        // Arrange

        // Act
        var result = _controller.Privacy() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var hasCorrectView = (result.ViewName == null || result.ViewName == "Privacy");
        Assert.True(hasCorrectView);
    }
}