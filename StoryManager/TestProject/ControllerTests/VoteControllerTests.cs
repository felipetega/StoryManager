using API.Application.Controllers;
using API.Application.ModelView;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject.ControllerTests
{
    public class VoteControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsListOfVoteViews_WhenVotesExist()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            var voteDTOs = new List<VoteDTO>
            {
                new VoteDTO { VoteValue = true },
                new VoteDTO { VoteValue = false },
            };

            mockVoteService.Setup(x => x.GetAll())
                           .ReturnsAsync(voteDTOs);

            // Act
            var result = await voteController.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<VoteView>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var voteViews = Assert.IsType<List<VoteView>>(okResult.Value);
            Assert.NotEmpty(voteViews);

        }

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenVotesDoNotExist()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            mockVoteService.Setup(x => x.GetAll())
                           .ReturnsAsync(new List<VoteDTO>());

            // Act
            var result = await voteController.GetAll();

            // Assert
            Assert.IsType<ActionResult<List<VoteView>>>(result);
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreated_WhenValidInput()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);


            var userId = 1;
            var storyId = 1;
            var voteValue = true;

            var createVoteView = new CreateVoteView
            {
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };

            var voteDTO = new VoteDTO
            {
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };


            mockVoteService.Setup(x => x.Create(It.IsAny<VoteDTO>()))
                .ReturnsAsync(true);


            // Act
            var result = await voteController.Create(createVoteView);
            Assert.NotNull(result);

            // Assert
            var createdResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }



        [Theory]
        [InlineData(-1, 1, true)]
        [InlineData(1, -1, true)]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput(int userId, int storyId, bool voteValue)
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            var createVoteView = new CreateVoteView
            {
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };

            // Act
            var result = await voteController.Create(createVoteView);

            // Assert
            Assert.IsType<BadRequestResult>(result);

            mockVoteService.Verify(x => x.Create(It.IsAny<VoteDTO>()), Times.Never);
        }


        [Fact]
        public async Task Update_ReturnsOk_WhenValidInput()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            var id = 1;
            var userId = 1;
            var storyId = 1;
            var voteValue = true;

            var createVoteView = new CreateVoteView
            {
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };

            mockVoteService.Setup(x => x.Update(It.Is<VoteDTO>(v => v.Id == id && v.UserId == userId && v.StoryId == storyId && v.VoteValue == voteValue), id))
                           .ReturnsAsync(true);

            // Act
            var result = await voteController.Update(createVoteView, id);

            // Assert
            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }


        [Theory]
        [InlineData(-1, 1, 1, true)]
        [InlineData(1, -1, 1, true)]
        [InlineData(1, 1, -1, true)]
        public async Task Update_ReturnsBadRequest_WhenInvalidInput(int id, int userId, int storyId, bool voteValue)
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);



            var createVoteView = new CreateVoteView
            {
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };

            // Act
            var result = await voteController.Update(createVoteView, id);

            // Assert
            Assert.IsType<BadRequestResult>(result);

            mockVoteService.Verify(x => x.Update(It.IsAny<VoteDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenVoteDoesNotExist()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            var id = 1;
            var userId = 1;
            var storyId = 1;
            var voteValue = true;

            CreateVoteView voteView = new CreateVoteView
            {
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };

            mockVoteService.Setup(x => x.Update(It.IsAny<VoteDTO>(), id))
                           .ReturnsAsync(false);

            // Act
            var result = await voteController.Update(voteView, id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenVoteDeleted()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            mockVoteService.Setup(x => x.Delete(It.IsAny<int>()))
                           .ReturnsAsync(true);

            var voteController = new VoteController(mockVoteService.Object);
            var id = 1;

            // Act
            var result = await voteController.Delete(id);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);

            var okResult = Assert.IsType<OkResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);

            mockVoteService.Verify(x => x.Delete(id), Times.Once);
        }


        [Fact]
        public async Task Delete_ReturnsNotFound_WhenVoteNotDeleted()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            mockVoteService.Setup(x => x.Delete(It.IsAny<int>()))
                           .ReturnsAsync(false);

            var voteController = new VoteController(mockVoteService.Object);
            var id = 1;

            // Act
            var result = await voteController.Delete(id);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);

            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            mockVoteService.Verify(x => x.Delete(id), Times.Once);
        }

    }
}
