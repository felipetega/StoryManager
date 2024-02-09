using API.Application.Controllers;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class UserTests
    {
        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoUsers()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            mockUserService.Setup(x => x.GetAll()).ReturnsAsync(new List<UserDTO>());

            // Act
            var result = await userController.GetAll();

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenUsersExist()
        {
            // Arrange
            var mockUserDTOs = new List<UserDTO>
            {
                new UserDTO
                {
                    Name = "John Doe"
                }
            };

            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            mockUserService.Setup(x => x.GetAll()).ReturnsAsync(mockUserDTOs);

            // Act
            var result = await userController.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<UserView>>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedUsers = Assert.IsType<List<UserView>>(okObjectResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.NotEmpty(returnedUsers);
            Assert.Equal(mockUserDTOs[0].Name, returnedUsers[0].Name);
        }
        [Fact]
        public async Task Create_ReturnsOk_WhenValidInput()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            var name = "John Doe";

            // Act
            var result = await userController.Create(name);

            // Assert
            var okResult = Assert.IsType<ActionResult<UserView>>(result);
            Assert.IsType<OkResult>(okResult.Result); // Ensure the inner result is OkObjectResult

            var actualStatusCode = (okResult.Result as OkResult)?.StatusCode;

            Assert.Equal(StatusCodes.Status200OK, actualStatusCode);

            // Ensure that the Create method of the service is called with the correct parameters
            mockUserService.Verify(x => x.Create(It.Is<UserDTO>(u => u.Name == name)), Times.Once);

        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            // Act
            var result = await userController.Create(null);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<UserView>>(result);
            Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);

            var actualStatusCode = (badRequestResult.Result as BadRequestObjectResult)?.StatusCode;
            var actualValue = (badRequestResult.Result as BadRequestObjectResult)?.Value;

            Assert.Equal(StatusCodes.Status400BadRequest, actualStatusCode);
            Assert.Equal("Invalid input. Name is required.", actualValue);

            // Ensure that the Create method of the service is not called when input is invalid
            mockUserService.Verify(x => x.Create(It.IsAny<UserDTO>()), Times.Never);

        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedUserView_WhenValidInput()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            var id = 1;
            var name = "John Doe";

            mockUserService.Setup(x => x.Update(It.IsAny<UserDTO>(), It.IsAny<int>()))
                           .ReturnsAsync(new UserDTO { Id = id, Name = name });

            // Act
            var result = await userController.Update(id, name);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserView>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var updatedUserView = Assert.IsType<UserView>(okResult.Value);
            Assert.Equal(name, updatedUserView.Name);

            mockUserService.Verify(x => x.Update(It.Is<UserDTO>(u => u.Id == id && u.Name == name), id), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenUserServiceReturnsNull()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            var id = 1;
            var name = "John Doe";

            mockUserService.Setup(x => x.Update(It.IsAny<UserDTO>(), It.IsAny<int>()))
                           .ReturnsAsync((UserDTO)null);

            // Act
            var result = await userController.Update(id, name);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserView>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);

            mockUserService.Verify(x => x.Update(It.Is<UserDTO>(u => u.Id == id && u.Name == name), id), Times.Once);

        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            // Act
            var result = await userController.Update(1, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserView>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid input. Name is required.", badRequestResult.Value);

            // Ensure that the Update method of the service is not called when input is invalid
            mockUserService.Verify(x => x.Update(It.IsAny<UserDTO>(), It.IsAny<int>()), Times.Never);

        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenUserDeletedSuccessfully()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            var id = 1;

            mockUserService.Setup(x => x.Delete(id))
                           .ReturnsAsync(true);

            // Act
            var result = await userController.Delete(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<bool>>(result);
            Assert.IsType<OkResult>(actionResult.Result);

            mockUserService.Verify(x => x.Delete(id), Times.Once);
        }


        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            var id = 1;

            mockUserService.Setup(x => x.Delete(id))
                           .ReturnsAsync(false);

            // Act
            var result = await userController.Delete(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<bool>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);

            mockUserService.Verify(x => x.Delete(id), Times.Once);
        }

    }
}
