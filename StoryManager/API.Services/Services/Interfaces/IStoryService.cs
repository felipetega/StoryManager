using API.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Services.Interfaces
{
    public interface IStoryService
    {
        Task<List<StoryDTO>> GetAll();
        Task<bool> Create(StoryDTO storyDTO);
        Task<bool> Update(StoryDTO storyDTO, int id);
        Task<bool> Delete(int id);
    }
}
