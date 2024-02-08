using API.Infrastructure.Models;

namespace API.Services.DTOs
{
    public class UserDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<VoteDTO> Votes { get; set; }
    }
}
