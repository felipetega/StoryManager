using API.Application.Controllers;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class StoryControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsNoContentResultWhenNoStories()
    {
        // Arrange
        var mockStoryService = new Mock<IStoryService>();
        var controller = new StoryController(mockStoryService.Object);

        // Mock empty data for your service
        mockStoryService.Setup(x => x.GetAll()).ReturnsAsync(new List<StoryDTO>());

        // Act
        var result = await controller.GetAll();

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result.Result);
        Assert.Equal(204, noContentResult.StatusCode);
    }
}
