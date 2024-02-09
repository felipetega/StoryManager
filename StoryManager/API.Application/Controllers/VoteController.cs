using API.Application.ViewModel;
using API.Services.DTOs;
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
        public async Task<ActionResult<VoteView>> Create(int userId, int storyId, bool voteValue)
        {
            if (userId <= 0 || storyId <= 0)
            {
                return BadRequest("Invalid input. UserId and StoryId are required.");
            }

            VoteDTO voteDTO = new VoteDTO
            {
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };

            await _voteService.Create(voteDTO);

            return Ok();
        }

        [HttpPut("votes/{id}")]
        [ProducesResponseType(typeof(VoteView), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<VoteView>> Update(int id, int userId, int storyId, bool voteValue)
        {
            if (id<0)
            {
                return BadRequest("Invalid input. VoteValue must be greater than zero.");
            }

            VoteDTO voteDTO = new VoteDTO
            {
                Id = id,
                UserId = userId,
                StoryId = storyId,
                VoteValue = voteValue
            };

            VoteDTO updatedVoteDTO = await _voteService.Update(voteDTO, id);

            if (updatedVoteDTO == null)
            {
                return NotFound();
            }

            VoteView updatedVoteView = new VoteView
            {
                VoteValue = voteValue
            };

            return Ok(updatedVoteView);
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
