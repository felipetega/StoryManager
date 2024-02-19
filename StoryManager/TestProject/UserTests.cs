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

            var name = "Felipe";

            UserView userView = new UserView
            {
                Name = name
            };

            // Act
            var result = await userController.Create(userView);

            // Assert
            var createdResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, createdResult.StatusCode);

        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            var name = "";

            UserView userview = new UserView
            {
                Name = name
            };

            // Act
            var result = await userController.Create(userview);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            mockUserService.Verify(x => x.Create(It.IsAny<UserDTO>()), Times.Never);
        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedUserView_WhenValidInput()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            int id = 1;
            var name = "Felipe";

            UserView userView = new UserView
            {
                Name = name
            };

            mockUserService.Setup(x => x.Update(It.IsAny<UserDTO>(), It.IsAny<int>()))
                           .ReturnsAsync(true);

            // Act
            var result = await userController.Update(userView, id);

            // Assert
            var okObjectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }


        [Fact]
        public async Task Update_ReturnsNotFound_WhenUserServiceReturnsNull()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            int id = 1;
            var name = "Felipe";

            UserView userView = new UserView
            {
                Name = name
            };

            mockUserService.Setup(x => x.Update(It.IsAny<UserDTO>(), It.IsAny<int>()))
                           .ReturnsAsync(false);

            // Act
            var result = await userController.Update(userView, id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userController = new UserController(mockUserService.Object);

            int id = 1;
            var name = "";

            UserView userView = new UserView
            {
                Name = name
            };

            // Act
            var result = await userController.Update(userView, id);

            // Assert
            Assert.IsType<BadRequestResult>(result);
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
