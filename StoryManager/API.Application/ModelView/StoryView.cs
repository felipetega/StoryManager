using API.Services.DTOs;

namespace API.Application.ViewModel
{
    public class StoryView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }

        public IEnumerable<VoteView> Votes { get; set; } = new List<VoteView>();
    }
}
