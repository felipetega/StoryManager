using API.Services.DTOs;

namespace API.Application.ModelView
{
    public class CreateVoteView
    {
        public int UserId { get; set; }
        public bool VoteValue { get; set; }
    }
}
