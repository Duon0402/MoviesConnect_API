using API.Entities.Users;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<ActionResult<AppUser>> GetUserByUsername(string username)
        {

            var user = await _userRepository.GetUserByUsername(username);
            return Ok(user);
        }
        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<AppUser>> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return Ok(user);
        }
    }
}
