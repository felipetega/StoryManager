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

        [HttpGet("api")]
        [ProducesResponseType(typeof(List<StoryView>), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<List<StoryView>>> GetAll()
        {
            IEnumerable<StoryDTO> storyDTOs = await _storyService.GetAll();

            if (storyDTOs == null || !storyDTOs.Any())
            {
                return NoContent();
            }

            List<StoryView> storyViews = storyDTOs.Select(storyDTO => new StoryView
            {
                Title = storyDTO.Title,
                Description = storyDTO.Description,
                Department = storyDTO.Department,
                Votes = storyDTO.Votes.Select(voteDTO => new VoteView
                {
                    VoteValue = voteDTO.VoteValue,
                    User = new UserView
                    {
                        Name = voteDTO.User.Name,
                    },
                    Story = new StoryView
                    {
                        Title = voteDTO.Story.Title,
                        Description = voteDTO.Story.Description,
                        Department = voteDTO.Story.Department,
                    }
                })
            }).ToList();

            return Ok(storyViews);
        }



        [HttpPost("api")]
        [ProducesResponseType(typeof(StoryView), 201)]
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


            return Ok();
        }

        [HttpPut("api/{id}")]
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


        [HttpDelete("api/{id}")]
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
