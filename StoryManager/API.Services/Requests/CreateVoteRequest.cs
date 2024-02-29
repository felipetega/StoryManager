using MediatR;

namespace API.Services.Handler
{
    public class CreateVoteRequest : IRequest<bool>
    {
        public int StoryId { get; set; }
        public int UserId { get; set; }
        public bool VoteValue { get; set; }
    }
}
