using System.Threading;
using System.Threading.Tasks;
using API.Services.DTOs;
using API.Services.Requests;
using API.Services.Services.Interfaces;
using MediatR;

namespace API.Services.Handlers
{
    public class CreateStoryHandler : IRequestHandler<CreateStoryRequest, bool>
    {
        private readonly IStoryService _service;

        public CreateStoryHandler(IStoryService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(CreateStoryRequest storyRequest, CancellationToken cancellationToken)
        {
            // Fail Fast Validations
            if (string.IsNullOrWhiteSpace(storyRequest.Title) ||
                string.IsNullOrWhiteSpace(storyRequest.Description) ||
                string.IsNullOrWhiteSpace(storyRequest.Department))
            {
                return false;
            }

            // Create StoryDTO
            var storyDTO = new StoryDTO
            {
                Title = storyRequest.Title,
                Description = storyRequest.Description,
                Department = storyRequest.Department
            };

            await _service.Create(storyDTO);

            return true;
        }
    }
}
