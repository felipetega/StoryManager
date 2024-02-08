using API.Services.DTOs;

namespace API.Application.ViewModel
{
    public class VoteView
    {

        public int UserId { get; set; }
        public int StoryId { get; set; }
        public bool VoteValue { get; set; }


        public UserView User { get; set; }
        public StoryView Story { get; set; }
    }
}
