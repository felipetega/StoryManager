using API.Services.DTOs;

namespace API.Application.ViewModel
{
    public class UserView
    {

        public string Name { get; set; }

        public List<VoteView> Votes { get; set; }
    }
}
