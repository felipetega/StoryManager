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
        //[Fact]
        //public async Task GetAll_ReturnsListOfVoteViews_WhenVotesExist()
        //{
        //    // Arrange
        //    var mockVoteService = new Mock<IVoteService>();
        //    var voteController = new VoteController(mockVoteService.Object);
        //    int storyId = 1;

        //    var voteDTOs = new List<VoteDTO>
        //    {
        //        new VoteDTO { VoteValue = true },
        //        new VoteDTO { VoteValue = false },
        //    };

        //    mockVoteService.Setup(x => x.GetByStoryId(storyId))
        //                   .ReturnsAsync(voteDTOs);

        //    // Act
        //    var result = await voteController.GetAll(storyId);

        //    // Assert
        //    var actionResult = Assert.IsType<ActionResult<List<VoteView>>>(result);
        //    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        //    var voteViews = Assert.IsType<List<VoteView>>(okResult.Value);
        //    Assert.NotEmpty(voteViews);

        //}

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenVotesDoNotExist()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);
            int id = 1;

            mockVoteService.Setup(x => x.GetByStoryId(id))
                           .ReturnsAsync(new List<VoteDTO>());

            // Act
            var result = await voteController.GetAll(id);

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
            var result = await voteController.Create(storyId, createVoteView);
            Assert.NotNull(result);

            // Assert
            var createdResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }



        [Theory]
        [InlineData(-1, 1, true)]
        //[InlineData(1, -1, true)]
        [InlineData(-1, -1, true)]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput(int userId, int storyId, bool voteValue)
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            var createVoteView = new CreateVoteView
            {
                UserId = userId,
                VoteValue = voteValue
            };

            // Act
            var result = await voteController.Create(storyId, createVoteView);

            // Assert
            Assert.IsType<BadRequestResult>(result);

            mockVoteService.Verify(x => x.Create(It.IsAny<VoteDTO>()), Times.Never);
        }
    }
}
