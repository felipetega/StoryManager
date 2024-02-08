using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Models
{
    public class Story
    {
        //public Story(string title, string description, string department)
        //{
        //    Title = title;
        //    Description = description;
        //    Department = department;
        //}

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }

        public IEnumerable<Vote> Votes { get; set; }
    }
}