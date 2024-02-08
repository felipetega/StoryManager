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
        [ProducesResponseType(typeof(UserView), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserView>> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Invalid input. Name is required.");
            }

            UserDTO userDTO = new UserDTO
            {
                Name = name
            };

            await _userService.Create(userDTO);

            return Ok(new UserView { Name = name });
        }

        [HttpPut("users/{id}")]
        [ProducesResponseType(typeof(UserView), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserView>> Update(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Invalid input. Name is required.");
            }

            UserDTO userDTO = new UserDTO
            {
                Id = id,
                Name = name
            };

            UserDTO updatedUserDTO = await _userService.Update(userDTO, id);

            if (updatedUserDTO == null)
            {
                return NotFound();
            }

            UserView updatedUserView = new UserView
            {
                Name = name
            };

            return Ok(updatedUserView);
        }

        [HttpDelete("users/{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            bool deleted = await _userService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok(deleted);
        }
    }
}
