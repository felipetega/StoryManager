using API.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Services.Interfaces
{
    public interface IVoteService
    {
        Task<List<VoteDTO>> GetByStoryId(int id);
        Task<bool> Create(VoteDTO storyDTO);
        Task<bool> Update(VoteDTO storyDTO, int id);
        Task<bool> Delete(int id);
    }
}
