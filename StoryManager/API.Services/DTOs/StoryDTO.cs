using API.Infrastructure.Models;

namespace API.Services.DTOs
{
    public class StoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }

        public IEnumerable<VoteDTO> Votes { get; set; }
    }
}
