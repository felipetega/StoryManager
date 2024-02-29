using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Handler;
using API.Services.Requests;
using API.Services.Validators;
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
            var createStoryRequest = new CreateStoryRequest
            {
                Title = storyView.Title,
                Description = storyView.Description,
                Department = storyView.Department
            };

            var validator = new CreateStoryRequestValidator();
            var validationResult = await validator.ValidateAsync(createStoryRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
            }

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
            var updateStoryRequest = new UpdateStoryRequest
            {
                Id = id,
                Title = storyView.Title,
                Description = storyView.Description,
                Department = storyView.Department
            };

            var validator = new UpdateStoryRequestValidator();
            var validationResult = await validator.ValidateAsync(updateStoryRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            bool success = await _mediator.Send(updateStoryRequest);

            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }



        [HttpDelete("stories/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var deleteStoryRequest = new DeleteStoryRequest
            {
                Id = id
            };

            var validator = new DeleteStoryRequestValidator();
            var validationResult = await validator.ValidateAsync(deleteStoryRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            bool success = await _mediator.Send(deleteStoryRequest);

            if (!success)
            {
                return NotFound("Story not found or failed to delete.");
            }

            return Ok();
        }


    }
}
