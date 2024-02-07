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
        Task<List<StoryDTO>> GetAllStories();
        Task<StoryDTO> Create(StoryDTO storyDTO);
        Task<StoryDTO> Update(StoryDTO storyDTO, int id);
        Task<StoryDTO> Delete(int id);
    }
}
