using API.Application.Controllers;
using API.Application.ModelView;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using MediatR;
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
        public async Task Create_ReturnsCreated_WhenValidInput()
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var mockMediator = new Mock<IMediator>();
            var voteController = new VoteController(mockVoteService.Object, mockMediator.Object);


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
            var mockMediator = new Mock<IMediator>();
            var voteController = new VoteController(mockVoteService.Object, mockMediator.Object);

            var createVoteView = new CreateVoteView
            {
                UserId = userId,
                VoteValue = voteValue
            };

            // Act
            var result = await voteController.Create(storyId, createVoteView);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            mockVoteService.Verify(x => x.Create(It.IsAny<VoteDTO>()), Times.Never);
        }
    }
}
