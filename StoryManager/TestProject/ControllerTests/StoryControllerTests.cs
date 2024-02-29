using API.Application.Controllers;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Handler;
using API.Services.Requests;
using API.Services.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject.ControllerTests
{

    public class StoryControllerTests
    {

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoStories()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            mockMediator.Setup(x => x.Send(It.IsAny<GetStoryRequest>(), default))
                        .ReturnsAsync(new List<StoryDTO>());

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

            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            mockMediator.Setup(x => x.Send(It.IsAny<GetStoryRequest>(), default))
                        .ReturnsAsync(mockStoryDTOs);

            // Act
            var result = await storyController.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<IdStoryView>>>(result);
            var okObjectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            var returnedStories = Assert.IsType<List<IdStoryView>>(okObjectResult.Value);

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
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<CreateStoryRequest>(), default)).ReturnsAsync(true);

            var storyController = new StoryController(mockMediator.Object);

            var title = "Sample title";
            var description = "Sample description";
            var department = "IT";

            var storyView = new StoryView
            {
                Title = title,
                Description = description,
                Department = department
            };

            // Act
            var result = await storyController.Create(storyView);

            // Assert
            var createdResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }






        [Theory]
        [InlineData("", "", "")]
        [InlineData("a", "", "")]
        [InlineData("", "a", "")]
        [InlineData("", "", "a")]
        [InlineData("a", "a", "")]
        [InlineData("", "a", "a")]
        [InlineData("a", "", "a")]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput(string title, string description, string department)
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            var storyView = new StoryView
            {
                Title = title,
                Description = description,
                Department = department
            };

            // Act
            var result = await storyController.Create(storyView);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var validationErrors = Assert.IsAssignableFrom<IEnumerable<string>>(badRequestResult.Value);
            Assert.NotEmpty(validationErrors);

            mockMediator.Verify(x => x.Send(It.IsAny<CreateStoryRequest>(), default), Times.Never);
        }



        [Fact]
        public async Task Update_ReturnsOk_WhenValidInput()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            var id = 1;
            var title = "Updated Title";
            var description = "Updated Description";
            var department = "Updated Department";

            var storyView = new StoryView
            {
                Title = title,
                Description = description,
                Department = department
            };

            mockMediator.Setup(x => x.Send(It.IsAny<UpdateStoryRequest>(), default))
                        .ReturnsAsync(true);

            // Act
            var result = await storyController.Update(id, storyView);

            // Assert
            var okObjectResult = Assert.IsType<OkResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }


        [Fact]
        public async Task Update_ReturnsNotFound_WhenIdNotFound()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            var id = 1;
            var title = "Updated Title";
            var description = "Updated Description";
            var department = "Updated Department";

            var storyView = new StoryView
            {
                Title = title,
                Description = description,
                Department = department
            };

            mockMediator.Setup(x => x.Send(It.IsAny<UpdateStoryRequest>(), default))
                        .ReturnsAsync(false);

            // Act
            var result = await storyController.Update(id, storyView);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }





        [Theory]
        [InlineData("", "", "")]
        [InlineData("a", "", "")]
        [InlineData("", "a", "")]
        [InlineData("", "", "a")]
        [InlineData("a", "a", "")]
        [InlineData("", "a", "a")]
        [InlineData("a", "", "a")]
        public async Task Update_ReturnsBadRequest_WhenInvalidInput(string title, string description, string department)
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            var id = 1;

            var storyView = new StoryView
            {
                Title = title,
                Description = description,
                Department = department
            };

            // Act
            var result = await storyController.Update(id, storyView);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            mockMediator.Verify(x => x.Send(It.IsAny<UpdateStoryRequest>(), default), Times.Never);
        }


        [Fact]
        public async Task Delete_ReturnsOk_WhenIdFound()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            var id = 1;

            mockMediator.Setup(x => x.Send(It.IsAny<DeleteStoryRequest>(), default)).ReturnsAsync(true);

            // Act
            var result = await storyController.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }


        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdNotFound()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var storyController = new StoryController(mockMediator.Object);

            var id = 1;

            mockMediator.Setup(x => x.Send(It.IsAny<DeleteStoryRequest>(), default)).ReturnsAsync(false);

            // Act
            var result = await storyController.Delete(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


    }
}

