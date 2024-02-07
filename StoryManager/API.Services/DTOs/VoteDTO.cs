using API.Infrastructure.Models;

namespace API.Services.DTOs
{
    public class VoteDTO
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoryId { get; set; }
        public bool VoteValue { get; set; }


        public UserDTO User { get; set; }
        public StoryDTO Story { get; set; }
    }
}
