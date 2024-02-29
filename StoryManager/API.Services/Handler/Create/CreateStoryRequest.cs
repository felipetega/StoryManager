using MediatR;

namespace API.Services.Handler.Create
{
    public class CreateStoryRequest : IRequest<bool>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
    }
}
