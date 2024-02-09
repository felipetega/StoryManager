﻿using API.Application.Controllers;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class VoteTests
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
                // Add more VoteDTOs as needed
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

            // Additional assertions if needed
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

            // Act
            var result = await voteController.Create(userId, storyId, voteValue);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VoteView>>(result);
            Assert.IsType<OkResult>(actionResult.Result);

            mockVoteService.Verify(x => x.Create(It.Is<VoteDTO>(v => v.UserId == userId && v.StoryId == storyId && v.VoteValue == voteValue)), Times.Once);
        }

        [Theory]
        [InlineData(0, 1, true)]
        [InlineData(1, 0, true)]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput(int userId, int storyId, bool voteValue)
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            // Act
            var result = await voteController.Create(userId, storyId, voteValue);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VoteView>>(result);
            var objectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

            Assert.Equal(400, objectResult.StatusCode);

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

            // Set up the mock to return a non-null value for the specified input
            mockVoteService.Setup(x => x.Update(It.Is<VoteDTO>(v => v.Id == id && v.UserId == userId && v.StoryId == storyId && v.VoteValue == voteValue), id))
                           .ReturnsAsync(new VoteDTO { /* Provide necessary properties */ });

            // Act
            var result = await voteController.Update(id, userId, storyId, voteValue);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VoteView>>(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }


        [Theory]
        [InlineData(-1, 1, 1, true)]
        public async Task Update_ReturnsBadRequest_WhenInvalidInput(int id, int userId, int storyId, bool voteValue)
        {
            // Arrange
            var mockVoteService = new Mock<IVoteService>();
            var voteController = new VoteController(mockVoteService.Object);

            // Act
            var result = await voteController.Update(id, userId, storyId, voteValue);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VoteView>>(result);
            var objectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

            Assert.Equal(400, objectResult.StatusCode);

            mockVoteService.Verify(x => x.Update(It.IsAny<VoteDTO>(), id), Times.Never);
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

            mockVoteService.Setup(x => x.Update(It.IsAny<VoteDTO>(), id))
                           .ReturnsAsync((VoteDTO)null);

            // Act
            var result = await voteController.Update(id, userId, storyId, voteValue);

            // Assert
            Assert.IsType<ActionResult<VoteView>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
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

            // Check the specific status code for Ok
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

            // Check the specific status code for NotFound
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            mockVoteService.Verify(x => x.Delete(id), Times.Once);
        }

    }
}
