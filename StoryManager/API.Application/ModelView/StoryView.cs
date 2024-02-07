﻿using API.Services.DTOs;

namespace API.Application.ViewModel
{
    public class StoryView
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }

        public List<VoteView> Votes { get; set; }
    }
}
