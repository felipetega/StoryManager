using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Controllers;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestProject
{

    // Use IClassFixture to share the fixture among test classes
    public class StoryTests
    {

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoStories()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            mockStoryService.Setup(x => x.GetAll()).ReturnsAsync(new List<StoryDTO>());

            // Act
            var result = await storyController.GetAll();

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenStoriesExist()
        {
            // Arrange
            var mockStoryDTOs = new List<StoryDTO>
            {
                new StoryDTO
                {
                    Department = "IT",
                    Description = "Sample description",
                    Title = "Sample title",
                    Votes = new List<VoteDTO>
                    {
                        new VoteDTO
                        {
                            VoteValue = true,
                            User = new UserDTO
                            {
                                Name = "John Doe"
                            }
                        }
                    }
                }
            };

            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            mockStoryService.Setup(x => x.GetAll()).ReturnsAsync(mockStoryDTOs);

            // Act
            var result = await storyController.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<StoryView>>>(result);
            var okObjectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            var returnedStories = Assert.IsType<List<StoryView>>(okObjectResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.NotEmpty(returnedStories);
            Assert.Equal(mockStoryDTOs[0].Department, returnedStories[0].Department);
            Assert.Equal(mockStoryDTOs[0].Description, returnedStories[0].Description);
            Assert.Equal(mockStoryDTOs[0].Title, returnedStories[0].Title);
        }
        [Fact]
        public async Task Create_ReturnsCreated_WhenValidInput()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            var title = "Sample title";
            var description = "Sample description";
            var department = "IT";

            var storyView = new CreateStoryView
            {
                Title = title,
                Description = description,
                Department = department
            };

            // Act
            var result = await storyController.Create(storyView);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal("Created", createdResult.Value);

            // You can also assert other properties of the ObjectResult if needed.
        }


        [Fact]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            // Act
            var result = await storyController.Create(null, "Sample description", "IT");

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);

            // Ensure that the Create method of the service is not called when input is invalid
            mockStoryService.Verify(x => x.Create(It.IsAny<StoryDTO>()), Times.Never);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenValidInput()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            var id = 1;
            var title = "Updated Title";
            var description = "Updated Description";
            var department = "Updated Department";

            var updatedStoryDTO = new StoryDTO
            {
                Id = id,
                Title = title,
                Description = description,
                Department = department
            };

            mockStoryService.Setup(x => x.Update(It.IsAny<StoryDTO>(), id))
                            .ReturnsAsync(updatedStoryDTO);

            // Act
            var result = await storyController.Update(id, title, description, department);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedStoryView = Assert.IsType<StoryView>(okObjectResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.Equal(title, updatedStoryView.Title);
            Assert.Equal(description, updatedStoryView.Description);
            Assert.Equal(department, updatedStoryView.Department);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenIdNotFound()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            var id = 1;

            mockStoryService.Setup(x => x.Update(It.IsAny<StoryDTO>(), id))
                            .ReturnsAsync((StoryDTO)null);

            // Act
            var result = await storyController.Update(id, "Updated Title", "Updated Description", "Updated Department");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            // Act
            var result = await storyController.Update(1, null, "Updated Description", "Updated Department");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);

            // Ensure that the Update method of the service is not called when input is invalid
            mockStoryService.Verify(x => x.Update(It.IsAny<StoryDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenIdFound()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            var id = 1;

            mockStoryService.Setup(x => x.Delete(id)).ReturnsAsync(true);

            // Act
            var result = await storyController.Delete(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var deleted = Assert.IsType<bool>(okObjectResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(deleted);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdNotFound()
        {
            // Arrange
            var mockStoryService = new Mock<IStoryService>();
            var storyController = new StoryController(mockStoryService.Object);

            var id = 1;

            mockStoryService.Setup(x => x.Delete(id)).ReturnsAsync(false);

            // Act
            var result = await storyController.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

    }
}

