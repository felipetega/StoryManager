using API.Services.DTOs;

namespace API.Application.ViewModel
{
    public class UserView
    {

        public string Name { get; set; }

        public IEnumerable<VoteView> Votes { get; set; } = new List<VoteView>();
    }
}
