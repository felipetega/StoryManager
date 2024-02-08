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

        public async Task<List<VoteDTO>> GetAll()
        {
            return await _context.Votes
                .Select(v => new VoteDTO
                {
                    Id = v.Id,
                    UserId = v.UserId,
                    StoryId = v.StoryId,
                    VoteValue = v.VoteValue,
                })
                .ToListAsync();
        }

        public async Task<VoteDTO> Create(VoteDTO voteDTO)
        {
            var vote = new Vote
            {
                UserId = voteDTO.UserId,
                StoryId = voteDTO.StoryId,
                VoteValue = voteDTO.VoteValue,
            };

            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();

            return voteDTO;
        }

        public async Task<VoteDTO> Update(VoteDTO voteDTO, int id)
        {
            Vote voteById = await FindId(id);

            if (voteById == null)
            {
                throw new Exception("Vote not found");
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

            return updatedVoteDTO;
        }

        public async Task<bool> Delete(int id)
        {
            Vote voteById = await FindId(id);

            if (voteById == null)
            {
                throw new Exception("Vote not found");
            }

            _context.Votes.Remove(voteById);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Vote> FindId(int id)
        {
            return await _context.Votes.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
