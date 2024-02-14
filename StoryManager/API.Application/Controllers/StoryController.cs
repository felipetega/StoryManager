using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers
{
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet("stories")]
        [ProducesResponseType(typeof(List<StoryView>), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<List<StoryView>>> GetAll()
        {
            IEnumerable<StoryDTO> storyDTOs = await _storyService.GetAll();

            List<StoryView> storyViewModel = storyDTOs.Select(x => new StoryView()
            {
                Id = x.Id,
                Department = x.Department,
                Description = x.Description,
                Title = x.Title,
                Votes = x.Votes.Select(y => new VoteView()
                {
                    VoteValue = y.VoteValue,
                    User = new UserView()
                    {
                        Name = y.User.Name,
                    }
                })
            }).ToList();
            if (storyViewModel.Count == 0)
            {
                return NoContent();
            }

            return StatusCode(200, storyViewModel);
        }





        [HttpPost("stories")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<StoryView>> Create(string title, string description, string department)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(department))
            {
                return BadRequest();
            }

            StoryDTO storyDTO = new StoryDTO
            {
                Title = title,
                Description = description,
                Department = department
            };

            await _storyService.Create(storyDTO);


            return StatusCode(201, "Created");
        }

        [HttpPut("stories/{id}")]
        [ProducesResponseType(typeof(StoryView), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<StoryView>> Update(int id, string title, string description, string department)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(department))
            {
                return BadRequest("Invalid input. Title, Description, and Department are required.");
            }

            StoryDTO storyDTO = new StoryDTO
            {
                Id = id,
                Title = title,
                Description = description,
                Department = department
            };

            StoryDTO updatedStoryDTO = await _storyService.Update(storyDTO, id);

            if (updatedStoryDTO == null)
            {
                return NotFound();
            }

            StoryView updatedStoryView = new StoryView
            {
                Title = title,
                Description = description,
                Department = department
            };

            return Ok(updatedStoryView);
        }


        [HttpDelete("stories/{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoryView>> Delete(int id)
        {
            bool deleted = await _storyService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok(deleted);
        }
    }
}
