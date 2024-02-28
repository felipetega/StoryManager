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
                    Name = y.User.Name,
                    VoteValue = y.VoteValue,
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
        public async Task<ActionResult> Create([FromBody] CreateStoryView storyView)
        {
            if (storyView == null || string.IsNullOrWhiteSpace(storyView.Title) ||
                string.IsNullOrWhiteSpace(storyView.Description) || string.IsNullOrWhiteSpace(storyView.Department))
            {
                return BadRequest();
            }

            StoryDTO storyDTO = new StoryDTO
            {
                Title = storyView.Title,
                Description = storyView.Description,
                Department = storyView.Department
            };

            await _storyService.Create(storyDTO);

            return StatusCode(201);
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [HttpPut("stories/{id}")]
        public async Task<IActionResult> Update([FromBody] CreateStoryView storyView, int id)
        {
            if (storyView == null || string.IsNullOrWhiteSpace(storyView.Title) ||
                string.IsNullOrWhiteSpace(storyView.Description) || string.IsNullOrWhiteSpace(storyView.Department))
            {
                return BadRequest();
            }

            StoryDTO storyDTO = new StoryDTO
            {
                Id = id,
                Title = storyView.Title,
                Description = storyView.Description,
                Department = storyView.Department
            };

            var updatedStoryDTO = await _storyService.Update(storyDTO, id);

            if (updatedStoryDTO == false)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpDelete("stories/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoryView>> Delete(int id)
        {
            bool deleted = await _storyService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
