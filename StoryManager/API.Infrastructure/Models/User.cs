using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Models
{
    public class User
    {
        //public User(string name)
        //{
        //    Name = name;
        //}

        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Vote> Votes { get; set; }
    }
}