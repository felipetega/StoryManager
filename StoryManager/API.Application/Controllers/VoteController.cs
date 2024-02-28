using API.Application.ModelView;
using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers
{
    public class VoteController : Controller
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
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
            if (createVoteView == null || createVoteView.UserId <= 0 || createVoteView.UserId is string)
            {
                return BadRequest();
            }

            VoteDTO voteDTO = new VoteDTO
            {
                StoryId = storyId,
                UserId = createVoteView.UserId,
                VoteValue = createVoteView.VoteValue,
            };

            var createdVote = await _voteService.Create(voteDTO);

            if (createdVote == false)
            {
                return NotFound();
            }

            return StatusCode(201);
        }



    }
}
