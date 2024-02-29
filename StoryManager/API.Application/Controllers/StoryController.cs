using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers
{
    public class StoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("stories")]
        [ProducesResponseType(typeof(List<IdStoryView>), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<List<IdStoryView>>> GetAll()
        {
            var getStoryRequest = new GetStoryRequest();

            List<StoryDTO> storyDTOs = await _mediator.Send(getStoryRequest);

            List<IdStoryView> storyViewModel = storyDTOs.Select(x => new IdStoryView()
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
        public async Task<ActionResult> Create([FromBody] StoryView storyView)
        {
            if (storyView == null || string.IsNullOrWhiteSpace(storyView.Title) ||
                string.IsNullOrWhiteSpace(storyView.Description) || string.IsNullOrWhiteSpace(storyView.Department))
            {
                return BadRequest();
            }

            var createStoryRequest = new CreateStoryRequest
            {
                Title = storyView.Title,
                Description = storyView.Description,
                Department = storyView.Department
            };

            bool success = await _mediator.Send(createStoryRequest);

            if (!success)
            {
                return BadRequest("Failed to create the story.");
            }

            return StatusCode(201);
        }

        [HttpPut("stories/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(int id, [FromBody] StoryView storyView)
        {
            if (storyView == null || id <= 0 || string.IsNullOrWhiteSpace(storyView.Title) ||
                string.IsNullOrWhiteSpace(storyView.Description) || string.IsNullOrWhiteSpace(storyView.Department))
            {
                return BadRequest();
            }

            var updateStoryRequest = new UpdateStoryRequest
            {
                Id = id,
                Title = storyView.Title,
                Description = storyView.Description,
                Department = storyView.Department
            };

            bool success = await _mediator.Send(updateStoryRequest);

            if (!success)
            {
                return NotFound("Story not found or failed to update.");
            }

            return Ok();
        }


        [HttpDelete("stories/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var deleteStoryRequest = new DeleteStoryRequest
            {
                Id = id
            };

            bool success = await _mediator.Send(deleteStoryRequest);

            if (!success)
            {
                return NotFound("Story not found or failed to delete.");
            }

            return Ok();
        }

    }
}
