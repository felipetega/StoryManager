using MediatR;

namespace API.Services.Requests
{
    public class DeleteStoryRequest : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
