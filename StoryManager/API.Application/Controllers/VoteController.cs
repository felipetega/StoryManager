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

        [HttpGet("votes")]
        [ProducesResponseType(typeof(List<VoteView>), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<List<VoteView>>> GetAll()
        {
            IEnumerable<VoteDTO> voteDTOs = await _voteService.GetAll();

            if (voteDTOs == null || !voteDTOs.Any())
            {
                return NoContent();
            }

            List<VoteView> voteViews = voteDTOs.Select(voteDTO => new VoteView
            {
                VoteValue = voteDTO.VoteValue,
            }).ToList();

            return Ok(voteViews);
        }

        [HttpPost("votes")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Create([FromBody] CreateVoteView createVoteView)
        {
            if (createVoteView == null || createVoteView.UserId <= 0 || createVoteView.StoryId <= 0 || createVoteView.UserId is string)
            {
                return BadRequest();
            }

            VoteDTO voteDTO = new VoteDTO
            {
                StoryId = createVoteView.StoryId,
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


        [HttpPut("votes/{id}")]
        [ProducesResponseType(typeof(VoteView), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Update([FromBody] CreateVoteView voteView, int id)
        {
            if (id<0 || voteView.UserId<0 || voteView.StoryId<0 || !(voteView.VoteValue is bool))
            {
                return BadRequest();
            }

            VoteDTO voteDTO = new VoteDTO
            {
                Id = id,
                UserId = voteView.UserId,
                StoryId = voteView.StoryId,
                VoteValue = voteView.VoteValue
            };

            var updatedVoteDTO = await _voteService.Update(voteDTO, id);

            if (updatedVoteDTO == false)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("votes/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            bool deleted = await _voteService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
