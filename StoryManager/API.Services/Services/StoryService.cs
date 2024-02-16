using API.Infrastructure.Context;
using API.Infrastructure.Models;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace API.Services.Services
{
    public class StoryService : IStoryService
    {
        private readonly ApiContext _context;

        public StoryService(ApiContext apiContext)
        {
            _context = apiContext;
        }

        public async Task<List<StoryDTO>> GetAll()
        {
            var stories = await _context.Stories
                .Select(s => new StoryDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Department = s.Department,
                    Votes = s.Votes.Select(v => new VoteDTO
                    {
                        Id = v.Id,
                        UserId = v.UserId,
                        StoryId = v.StoryId,
                        VoteValue = v.VoteValue,
                        User = new UserDTO()
                        {
                            Name = v.User.Name
                        }
                    })
                })
                .ToListAsync();

            var sortedStories = stories.OrderByDescending(s => s.Votes.Sum(v => v.VoteValue ? 1 : -1)).ToList();

            return sortedStories;
        }




        public async Task<StoryDTO> Create(StoryDTO storyDTO)
        {
            var story = new Story
            {
                Title = storyDTO.Title,
                Description = storyDTO.Description,
                Department = storyDTO.Department,
            };

            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            return storyDTO;
        }

        public async Task<bool> Update(StoryDTO storyDTO, int id)
        {
            var storyId = _context.Stories.FirstOrDefault(x => x.Id == id);
            if (storyId == null)
            {
                return false;
            }

            storyId.Title = storyDTO.Title;
            storyId.Description = storyDTO.Description;
            storyId.Department = storyDTO.Department;

            _context.Stories.Update(storyId);
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> Delete(int id)
        //{
        //    var storyId = _context.Stories.FirstOrDefault(x => x.Id == id);

        //    if (storyById == null)
        //    {
        //        throw new Exception("Story not found");
        //    }

        //    _context.Stories.Remove(storyById);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<bool> FindId(int id)
        {
            var findId = await _context.Stories.FirstOrDefaultAsync(x => x.Id == id);
            if(findId == null)
            {
                return false;
            }
            return true;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}