using API.DTOs.Accounts;
using API.Entities.Users;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AccountOutputDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

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
                Roles = roles,
                Token = await _tokenService.CreateToken(user),
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

            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());


            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid password");
            var roles = await _userManager.GetRolesAsync(user);

            return new AccountOutputDto
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = roles,
                Token = await _tokenService.CreateToken(user),
            };
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(string username,
            string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound();

            var checkPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!checkPassword) return BadRequest("Invalid old password");

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded) return BadRequest("Password changed failed");
            return Ok("Password changed successfully");
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}