using MediFormManager.Application.DTOs.Users;
using MediFormManager.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MediFormManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService) { 
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserRequest request)
        {
            var user = await _userService.CreateAsync(request);            
            return CreatedAtAction(
                nameof(GetById), 
                new { id = user.Id }, 
                user);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserDto>> Update(Guid id, UpdateUserRequest request)
        {
            var user = await _userService.UpdateAsync(id, request);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
