using API.DTOs.Admin;
using API.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Admins
{
    [Authorize]
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // Users
        #region Get Users With Roles
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult<IEnumerable<UsersWithRolesDto>>> GetUsersWithRoles([FromQuery] string? username)
        {
            IQueryable<AppUser> query = _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName);

            if (!string.IsNullOrWhiteSpace(username))
            {
                query = query.Where(u => u.UserName.Contains(username));
            }

            var users = await query.Select(u => new UsersWithRolesDto
            {
                Id =  u.Id,
                Username = u.UserName,
                Roles = u.UserRoles
                    .Select(r => r.Role.Name)
                    .ToList()
            }).ToListAsync();

            return Ok(users);
        }
        #endregion

        #region Edit Roles 
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }
        #endregion
    }
}
