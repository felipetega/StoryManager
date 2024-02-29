using API.Services.DTOs;
using MediatR;
using System.Collections.Generic;

namespace API.Services.Requests
{
    public class GetStoryRequest : IRequest<List<StoryDTO>>
    {
        //
    }
}
