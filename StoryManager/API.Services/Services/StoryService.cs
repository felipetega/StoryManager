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
            return await _context.Stories
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
                        VoteValue = v.VoteValue
                    })
                })
                .ToListAsync();
        }

        //public async Task<StoryDTO> GetById(int id)
        //{
        //    return await _context.Stories
        //        .Select(s => new StoryDTO()
        //        {
        //            Id = s.Id,
        //            Title = s.Title,
        //            Description = s.Description,
        //            Department = s.Department,
        //            Votes = s.Votes.Select(v => new VoteDTO
        //            {
        //                Id = v.Id,
        //                UserId = v.UserId,
        //                StoryId = v.StoryId,
        //                VoteValue = v.VoteValue
        //            })
        //        })
        //        .FirstOrDefaultAsync(x => x.Id == id);
        //}

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

        public async Task<StoryDTO> Update(StoryDTO storyDTO, int id)
        {
            Story storyById = await FindId(id);

            if (storyById == null)
            {
                throw new Exception("Story not found");
            }

            storyById.Title = storyDTO.Title;
            storyById.Description = storyDTO.Description;
            storyById.Department = storyDTO.Department;

            _context.Stories.Update(storyById);
            await _context.SaveChangesAsync();

            var updatedStoryDTO = new StoryDTO
            {
                Id = storyById.Id,
                Title = storyById.Title,
                Description = storyById.Description,
            };

            return storyDTO;
        }

        public async Task<bool> Delete(int id)
        {
            Story storyById = await FindId(id);

            if (storyById == null)
            {
                throw new Exception("Story not found");
            }

            _context.Stories.Remove(storyById);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Story> FindId(int id)
        {
            return await _context.Stories.FirstOrDefaultAsync(x => x.Id == id);
        }


    }
}