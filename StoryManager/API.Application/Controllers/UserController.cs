using API.Application.ViewModel;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(List<UserView>), 200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<List<UserView>>> GetAll()
        {
            IEnumerable<UserDTO> userDTOs = await _userService.GetAll();

            if (userDTOs == null || !userDTOs.Any())
            {
                return NoContent();
            }

            List<UserView> userViews = userDTOs.Select(userDTO => new UserView
            {
                Name = userDTO.Name,
            }).ToList();

            return Ok(userViews);
        }

        [HttpPost("users")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Create([FromBody] UserView userView)
        {
            if (string.IsNullOrWhiteSpace(userView.Name))
            {
                return BadRequest();
            }

            UserDTO userDTO = new UserDTO
            {
                Name = userView.Name
            };

            await _userService.Create(userDTO);

            return StatusCode(201);
        }

        [HttpPut("users/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Update([FromBody] UserView userView, int id)
        {
            if (string.IsNullOrWhiteSpace(userView.Name))
            {
                return BadRequest();
            }

            UserDTO userDTO = new UserDTO
            {
                Id = id,
                Name = userView.Name
            };

            bool updatedUser = await _userService.Update(userDTO, id);

            if (!updatedUser)
            {
                return NotFound();
            }


            return Ok();
        }

        [HttpDelete("users/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            bool deleted = await _userService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
