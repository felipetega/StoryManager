using System.Threading;
using System.Threading.Tasks;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using MediatR;

namespace API.Services.Handler
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
            // Fail Fast Validations
            if (storyRequest.Id <= 0 ||  // Assuming you have an Id property for identifying the story
                string.IsNullOrWhiteSpace(storyRequest.Title) ||
                string.IsNullOrWhiteSpace(storyRequest.Description) ||
                string.IsNullOrWhiteSpace(storyRequest.Department))
            {
                return false;
            }

            // Create StoryDTO for update
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
