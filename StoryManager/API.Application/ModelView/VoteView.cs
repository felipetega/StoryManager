using API.Services.DTOs;

namespace API.Application.ViewModel
{
    public class VoteView
    {

        public bool VoteValue { get; set; }
        public UserView User { get; set; }
        public StoryView Story { get; set; }
    }
}
