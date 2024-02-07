using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Models
{
    public class Vote
    {
        public Vote(bool voteValue)
        {
            VoteValue = voteValue;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoryId { get; set; }
        public bool VoteValue { get; set; }


        public User User { get; set; }
        public Story Story { get; set; }
    }
}
