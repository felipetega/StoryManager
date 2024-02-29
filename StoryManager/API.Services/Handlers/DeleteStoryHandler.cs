using API.Services.Requests;
using API.Services.Services.Interfaces;
using MediatR;

namespace API.Services.Handlers
{
    public class DeleteStoryHandler : IRequestHandler<DeleteStoryRequest, bool>
    {
        private readonly IStoryService _service;

        public DeleteStoryHandler(IStoryService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(DeleteStoryRequest storyRequest, CancellationToken cancellationToken)
        {
            // Fail Fast Validations
            if (storyRequest.Id <= 0)
            {
                return false;
            }

            return await _service.Delete(storyRequest.Id);
        }
    }
}
