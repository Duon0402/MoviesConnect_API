using API.DTOs.Users;
using API.Entities.Users;
using API.Extentions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
    }
}
