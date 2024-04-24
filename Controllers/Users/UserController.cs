using API.DTOs.Photos;
using API.DTOs.Users;
using API.DTOs.Users.Member;
using API.Entities.Users;
using API.Extentions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, IMapper mapper,
            IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet("GetUserByUsername/{username}", Name = "GetUserByUsername")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            return await _userRepository.GetMemberByUsername(username);
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<MemberDto>> GetUserById(int userId)
        {
            return await _userRepository.GetMemberById(userId);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetListUsers")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetListUsers()
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

        #region SetAvatar
        [HttpPost("SetAvatar")]
        public async Task<ActionResult<AvatarDto>> SetAvatar(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsername(User.GetUsername());

            if(user.Avatar.PublicId != null || user.Avatar.PublicId != "default_avatar")
            {
                await _photoService.DeletePhoto(user.Avatar.PublicId);
            }

            var result = await _photoService.AddPhoto(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var avatar = new Avatar
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                AppUserId = user.Id,
            };

            user.Avatar = avatar;

            if (await _userRepository.Save())
            {
                return CreatedAtRoute("GetUserByUsername", new { username = user.UserName }, _mapper.Map<AvatarDto>(avatar));
            }

            return BadRequest("Problem adding photo");
        }
        #endregion
    }
}