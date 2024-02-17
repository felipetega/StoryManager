using API.Services.DTOs;

namespace API.Application.ModelView
{
    public class CreateVoteView
    {
        //public int Id { get; set; }
        public int UserId { get; set; }
        public int StoryId { get; set; }
        public bool VoteValue { get; set; }


        //public UserDTO User { get; set; }
        //public StoryDTO Story { get; set; }
    }
}
