using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using MediatR;

namespace API.Services.Handler
{
    public class GetStoryHandler : IRequestHandler<GetStoryRequest, List<StoryDTO>>
    {
        private readonly IStoryService _service;

        public GetStoryHandler(IStoryService service)
        {
            _service = service;
        }

        public async Task<List<StoryDTO>> Handle(GetStoryRequest request, CancellationToken cancellationToken)
        {
            return await _service.GetAll();
        }
    }
}
