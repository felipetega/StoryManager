using API.Services.DTOs;
using API.Services.Requests;
using API.Services.Services.Interfaces;
using MediatR;

namespace API.Services.Handlers
{
    public class UpdateStoryHandler : IRequestHandler<UpdateStoryRequest, bool>
    {
        private readonly IStoryService _service;

        public UpdateStoryHandler(IStoryService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(UpdateStoryRequest storyRequest, CancellationToken cancellationToken)
        {
            if (storyRequest.Id <= 0 || 
                string.IsNullOrWhiteSpace(storyRequest.Title) ||
                string.IsNullOrWhiteSpace(storyRequest.Description) ||
                string.IsNullOrWhiteSpace(storyRequest.Department))
            {
                return false;
            }

            var storyDTO = new StoryDTO
            {
                Title = storyRequest.Title,
                Description = storyRequest.Description,
                Department = storyRequest.Department
            };

            await _service.Update(storyDTO, storyRequest.Id);

            return true;
        }
    }
}
