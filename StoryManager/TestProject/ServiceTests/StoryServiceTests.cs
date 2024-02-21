using API.Infrastructure.Context;
using API.Infrastructure.Models;
using API.Services.DTOs;
using API.Services.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject.ServiceTests
{
    public class StoryServiceTests
    {
        private readonly ApiContext _context;

        public StoryServiceTests()
        {
            DbContextOptions<ApiContext> options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApiContext(options);
        }

        [Fact]
        public async Task GetAll_ReturnsAllStories()
        {
            // Arrange
            var service = new StoryService(_context);

            _context.Stories.AddRange(new[]
            {
        new Story { Id = 1, Title = "Story 1", Department = "Dept1", Description = "Desc1", Votes = new[] { new Vote { Id = 1, VoteValue = true } } },
        new Story { Id = 2, Title = "Story 2", Department = "Dept2", Description = "Desc2", Votes = new[] { new Vote { Id = 2, VoteValue = true } } },
        new Story { Id = 3, Title = "Story 3", Department = "Dept3", Description = "Desc3", Votes = new[] { new Vote { Id = 3, VoteValue = true } } },
    });
            await _context.SaveChangesAsync();

            // Act
            var result = await service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal("Story 1", result[0].Title);
            Assert.Equal("Story 2", result[1].Title);
            Assert.Equal("Story 3", result[2].Title);
        }

        [Fact]
        public async Task Create_AddsStoryToDatabase()
        {
            // Arrange
            var service = new StoryService(_context);

            // Act
            var created = await service.Create(new StoryDTO
            {
                Title = "New Story",
                Description = "New Description",
                Department = "New Department",
            });

            // Assert
            Assert.True(created);

            var createdStory = await _context.Stories.FirstOrDefaultAsync(s => s.Title == "New Story");

            Assert.NotNull(createdStory);
            Assert.Equal("New Story", createdStory.Title);
            Assert.Equal("New Description", createdStory.Description);
            Assert.Equal("New Department", createdStory.Department);
        }

        [Fact]
        public async Task Update_UpdatesStoryInDatabase()
        {
            // Arrange
            var service = new StoryService(_context);

            // Add a story to the database
            _context.Stories.Add(new Story
            {
                Id = 1,
                Title = "Existing Story",
                Department = "Existing Department",
                Description = "Existing Description",
            });
            await _context.SaveChangesAsync();

            // Act
            var updated = await service.Update(new StoryDTO
            {
                Title = "Updated Story",
                Description = "Updated Description",
                Department = "Updated Department",
            }, 1);

            // Assert
            Assert.True(updated);

            var updatedStory = await _context.Stories.FirstOrDefaultAsync(s => s.Id == 1);

            Assert.NotNull(updatedStory);
            Assert.Equal("Updated Story", updatedStory.Title);
            Assert.Equal("Updated Description", updatedStory.Description);
            Assert.Equal("Updated Department", updatedStory.Department);
        }

        [Fact]
        public async Task Delete_RemovesStoryFromDatabase()
        {
            // Arrange
            var service = new StoryService(_context);

            // Add a story to the database
            _context.Stories.Add(new Story
            {
                Id = 1,
                Title = "Story to be deleted",
                Department = "Department",
                Description = "Description",
            });
            await _context.SaveChangesAsync();

            // Act
            var deleted = await service.Delete(1);

            // Assert
            Assert.True(deleted);

            // Check if the story is removed from the database
            var deletedStory = await _context.Stories.FirstOrDefaultAsync(s => s.Id == 1);

            Assert.Null(deletedStory);
        }



    }
}