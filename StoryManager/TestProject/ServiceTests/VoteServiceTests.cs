using API.Infrastructure.Context;
using API.Infrastructure.Models;
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
    public class VoteServiceTests
    {
        private readonly ApiContext _context;

        public VoteServiceTests()
        {
            DbContextOptions<ApiContext> options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApiContext(options);
        }

        [Fact]
        public async Task Create_AddsVoteToDatabase()
        {
            // Arrange
            var service = new VoteService(_context);

            _context.Stories.Add(new Story { Id = 1, Title = "Story 1", Department = "Dept1", Description = "Desc1" });
            _context.Users.Add(new User { Id = 1, Name = "User 1" });
            await _context.SaveChangesAsync();

            // Act
            var created = await service.Create(new VoteDTO
            {
                UserId = 1,
                StoryId = 1,
                VoteValue = true,
            });

            // Assert
            Assert.True(created);

            var createdVote = await _context.Votes.FirstOrDefaultAsync(v => v.UserId == 1 && v.StoryId == 1);

            Assert.NotNull(createdVote);
            Assert.Equal(1, createdVote.UserId);
            Assert.Equal(1, createdVote.StoryId);
            Assert.True(createdVote.VoteValue);
        }

        [Fact]
        public async Task Create_ReturnsFalseWhenStoryOrUserNotFound()
        {
            // Arrange
            var service = new VoteService(_context);


            // Act
            var created = await service.Create(new VoteDTO
            {
                UserId = 1,
                StoryId = 1,
                VoteValue = true,
            });

            // Assert
            Assert.False(created);

            var createdVote = await _context.Votes.FirstOrDefaultAsync();
            Assert.Null(createdVote);
        }


    }
}
