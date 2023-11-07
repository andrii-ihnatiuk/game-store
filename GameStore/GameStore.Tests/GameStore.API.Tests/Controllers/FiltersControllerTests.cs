using GameStore.API.Controllers;
using GameStore.Shared.Constants.Filter;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class FiltersControllerTests
{
    private readonly FiltersController _controller = new();

    [Fact]
    public void GetPublishedDateOptionsForGame_ReturnsOk()
    {
        // Act
        var result = _controller.GetPublishedDateOptionsForGame();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(PublishDateOption.AllOptions, ((OkObjectResult)result).Value);
    }

    [Fact]
    public void GetSortingOptionsForGame_ReturnsOk()
    {
        // Act
        var result = _controller.GetSortingOptionsForGame();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(SortingOption.AllOptions, ((OkObjectResult)result).Value);
    }

    [Fact]
    public void GetPaginationOptionsForGame_ReturnsOk()
    {
        // Act
        var result = _controller.GetPaginationOptionsForGame();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(PaginationOption.AllOptions, ((OkObjectResult)result).Value);
    }
}