using API.DTOs.Accounts;
using API.Entities.Users;
using API.Extentions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Users
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AccountOutputDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);
            user.Avatar = new Avatar
            {
                AppUserId = user.Id,
                PublicId = "default_avatar",
                Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741814/user_yxfmyc.png"
            };

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            var roles = await _userManager.GetRolesAsync(user);
            return new AccountOutputDto
            {
                Id = user.Id,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Roles = roles
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AccountOutputDto>> Login(LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Username))
            {
                return BadRequest("Please enter a Username");
            }
            if (string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Please enter a Password");
            }

            var user = await _userManager.Users
                .Include(a => a.Avatar)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid password");
            var roles = await _userManager.GetRolesAsync(user);
            return new AccountOutputDto
            {
                Id = user.Id,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                AvatarUrl = user.Avatar?.Url,
                Roles = roles
            };
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePassword)
        {
            var user = await _userRepository.GetUserByUsername(User.GetUsername());

            var checkPassword = await _userManager.CheckPasswordAsync(user,
                changePassword.CurrentPassword);
            if (!checkPassword) return BadRequest("Invalid current password");

            var result = await _userManager.ChangePasswordAsync(user,
                changePassword.CurrentPassword, changePassword.NewPassword);
            if (!result.Succeeded) return BadRequest("Password changed failed");
            return Ok();
        }

        [Authorize]
        [HttpPut("ChangeSettingAccount")]
        public async Task<ActionResult> ChangeSettingAccount([FromBody] Boolean isPrivate)
        {
            var user = await _userRepository.GetUserById(User.GetUserId());

            user.IsPrivate = isPrivate;

            _userRepository.UpdateUser(user);

            if (await _userRepository.Save()) return Ok(user.IsPrivate);

            return BadRequest("Failed to change setting account");
        }
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}