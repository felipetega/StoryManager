using API.Services.DTOs;
using MediatR;

namespace API.Services.Requests
{
    public class GetStoryRequest : IRequest<List<StoryDTO>>
    {
        //
    }
}
