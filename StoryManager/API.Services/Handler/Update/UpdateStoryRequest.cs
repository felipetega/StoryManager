using MediatR;

namespace API.Services.Handler
{
    public class UpdateStoryRequest : IRequest<bool>
    {
        public int Id { get; set; }  // Assuming you have an Id property for identifying the story
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
    }
}
