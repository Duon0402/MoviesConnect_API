using API.DTOs.Users;
using API.DTOs.Users.Member;
using API.Extentions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("GetUserByUsername")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            return await _userRepository.GetMemberByUsername(username);
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<MemberDto>> GetUserById(int userId)
        {
            return await _userRepository.GetMemberById(userId);
        }

        [HttpGet("GetListUsers")]
        public async Task<ActionResult<MemberDto>> GetListUsers()
        {
            var users = await _userRepository.GetListMembers();
            return Ok(users);
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsername(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.UpdateUser(user);

            if (await _userRepository.Save()) return NoContent();

            return BadRequest("Failed to update user");
        }
    }
}