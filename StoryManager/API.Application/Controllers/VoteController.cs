using API.Application.ModelView;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Handler;
using API.Services.Services;
using API.Services.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers
{
    public class VoteController : Controller
    {
        private readonly IVoteService _voteService;
        private readonly IMediator _mediator;

        public VoteController(IVoteService voteService, IMediator mediator)
        {
            _voteService = voteService;
            _mediator = mediator;
        }

        [HttpGet("stories/{storyId}/votes")]
        [ProducesResponseType(typeof(List<VoteView>), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<List<VoteView>>> GetAll(int storyId)
        {
            IEnumerable<VoteDTO> voteDTOs = await _voteService.GetByStoryId(storyId);

            if (voteDTOs == null || !voteDTOs.Any())
            {
                return NoContent();
            }

            List<VoteView> voteViews = voteDTOs.Select(voteDTO => new VoteView
            {
                Name = voteDTO.User.Name,
                VoteValue = voteDTO.VoteValue,
            }).ToList();

            return Ok(voteViews);
        }


        [HttpPost("stories/{storyId}/votes")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Create(int storyId, [FromBody] CreateVoteView createVoteView)
        {
            var createVoteRequest = new CreateVoteRequest
            {
                StoryId = storyId,
                UserId = createVoteView.UserId,
                VoteValue = createVoteView.VoteValue,
            };

            bool success = await _mediator.Send(createVoteRequest);

            if (!success)
            {
                return BadRequest();
            }

            return StatusCode(201);
        }




    }
}
