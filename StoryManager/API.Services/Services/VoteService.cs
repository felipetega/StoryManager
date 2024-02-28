using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Context;
using API.Infrastructure.Models;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Services
{
    public class VoteService : IVoteService
    {
        private readonly ApiContext _context;

        public VoteService(ApiContext apiContext)
        {
            _context = apiContext;
        }

        public async Task<List<VoteDTO>> GetByStoryId(int storyId)
        {
            return await _context.Votes
                .Where(v => v.StoryId == storyId)
                .Select(v => new VoteDTO
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
                .ToListAsync();
        }

        public async Task<bool> Create(VoteDTO voteDTO)
        {
            var storyIdExists = await _context.Stories.AnyAsync(x => x.Id == voteDTO.StoryId);
            var userIdExists = await _context.Users.AnyAsync(x => x.Id == voteDTO.UserId);

            if (!storyIdExists || !userIdExists)
            {
                return false;
            }

            var vote = new Vote
            {
                UserId = voteDTO.UserId,
                StoryId = voteDTO.StoryId,
                VoteValue = voteDTO.VoteValue,
            };

            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> Update(VoteDTO voteDTO, int id)
        {
            var voteById = await _context.Votes.FirstOrDefaultAsync(x => x.Id == id);

            if (voteById == null)
            {
                return false;
            }

            voteById.UserId = voteDTO.UserId;
            voteById.StoryId = voteDTO.StoryId;
            voteById.VoteValue = voteDTO.VoteValue;

            _context.Votes.Update(voteById);
            await _context.SaveChangesAsync();

            var updatedVoteDTO = new VoteDTO
            {
                Id = voteById.Id,
                UserId = voteById.UserId,
                StoryId = voteById.StoryId,
                VoteValue = voteById.VoteValue,
            };

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var voteId = await _context.Votes.FirstOrDefaultAsync(x => x.Id == id);

            if (voteId == null)
            {
                return false;
            }

            _context.Votes.Remove(voteId);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
