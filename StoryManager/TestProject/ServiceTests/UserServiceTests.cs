using API.Infrastructure.Context;
using API.Services.DTOs;
using API.Services.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ServiceTests
{
    public class UserServiceTests
    {
        private readonly ApiContext _context;

        public UserServiceTests()
        {
            DbContextOptions<ApiContext> options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApiContext(options);
        }

        [Fact]
        public async Task Create_AddsUserToDatabase()
        {
            // Arrange
            var service = new UserService(_context);

            // Act
            var created = await service.Create(new UserDTO
            {
                Name = "New User",
            });

            // Assert
            Assert.True(created);

            // Check if the user is added to the database
            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == "New User");

            Assert.NotNull(createdUser);
            Assert.Equal("New User", createdUser.Name);
        }


    }
}
