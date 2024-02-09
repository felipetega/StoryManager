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
    // Define the fixture class
    public class StoryControllerFixture : IDisposable
    {
        public Mock<IStoryService> MockStoryService { get; private set; }
        public StoryController StoryController { get; private set; }

        public StoryControllerFixture()
        {
            // Initialize common objects here
            MockStoryService = new Mock<IStoryService>();
            StoryController = new StoryController(MockStoryService.Object);
        }

        public void Dispose()
        {
            // Clean up resources if needed
        }
    }

    // Use IClassFixture to share the fixture among test classes
    public class StoryControllerTests : IClassFixture<StoryControllerFixture>
    {
        private readonly StoryControllerFixture _fixture;

        public StoryControllerTests(StoryControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoStories()
        {
            // Arrange
            _fixture.MockStoryService.Setup(x => x.GetAll()).ReturnsAsync(new List<StoryDTO>());

            // Act
            var result = await _fixture.StoryController.GetAll();

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

            _fixture.MockStoryService.Setup(x => x.GetAll()).ReturnsAsync(mockStoryDTOs);

            // Act
            var result = await _fixture.StoryController.GetAll();

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
        public async Task Update_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            int id = 1; // Use a valid story id
            string invalidTitle = null;
            string invalidDescription = null;
            string invalidDepartment = null;

            // Act
            var result = await _fixture.StoryController.Update(id, invalidTitle, invalidDescription, invalidDepartment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenStoryNotExists()
        {
            // Arrange
            int nonExistingId = 999; // Use an id that does not exist
            string validTitle = "Updated Title";
            string validDescription = "Updated Description";
            string validDepartment = "Updated Department";

            _fixture.MockStoryService.Setup(x => x.Update(It.IsAny<StoryDTO>(), nonExistingId))
                .ReturnsAsync((StoryDTO)null); // Simulate the service returning null for a non-existing story

            // Act
            var result = await _fixture.StoryController.Update(nonExistingId, validTitle, validDescription, validDepartment);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task Update_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            int existingId = 1; // Use a valid story id
            string validTitle = "Updated Title";
            string validDescription = "Updated Description";
            string validDepartment = "Updated Department";

            var updatedStoryDTO = new StoryDTO
            {
                Id = existingId,
                Title = validTitle,
                Description = validDescription,
                Department = validDepartment
            };

            _fixture.MockStoryService.Setup(x => x.Update(It.IsAny<StoryDTO>(), existingId))
                .ReturnsAsync(updatedStoryDTO); // Simulate the service returning an updated story

            // Act
            var result = await _fixture.StoryController.Update(existingId, validTitle, validDescription, validDepartment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<StoryView>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var updatedStoryView = Assert.IsType<StoryView>(okObjectResult.Value);

            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(validTitle, updatedStoryView.Title);
            Assert.Equal(validDescription, updatedStoryView.Description);
            Assert.Equal(validDepartment, updatedStoryView.Department);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            string invalidTitle = null;
            string validDescription = "Sample Description";
            string validDepartment = "Sample Department";

            // Act
            var result = await _fixture.StoryController.Create(invalidTitle, validDescription, validDepartment);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
