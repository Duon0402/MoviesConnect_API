using API.DTOs.Admin;
using API.DTOs.Reports;
using API.Entities;
using API.Entities.Movies;
using API.Entities.Users;
using API.Extentions;
using API.Helpers.Params;
using API.Interfaces;
using API.Interfaces.Movies;
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
        private readonly IMovieRepository _movieRepository;
        private readonly IReportRepository _reportRepository;

        public AdminController(UserManager<AppUser> userManager, IMovieRepository movieRepository, 
            IReportRepository reportRepository)
        {
            _userManager = userManager;
            _movieRepository = movieRepository;
            _reportRepository = reportRepository;
        }

        // Users
        #region Get Users With Roles
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult<IEnumerable<UsersWithRolesDto>>> GetUsersWithRoles()
        {
            IQueryable<AppUser> query = _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.Id);

            var users = await query.Where(u => u.UserRoles.All(ur => ur.Role.Name != "Admin"))
                .Select(u => new UsersWithRolesDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
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

        //Movies
        #region Get Movies
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return Ok(await _movieRepository.GetListMoviesForEdit());
        }

        #endregion
        #region GetMovie
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<Movie>> GetMovie(int movieId)
        {
            if (movieId == 0 || movieId == null) return NotFound();
            return Ok(await _movieRepository.GetMovieByIdForEdit(movieId));
        }
        #endregion

        // Report
        #region CreateReport
        [HttpPost("report")]
        public async Task<ActionResult> CreateReport(ReportCreateDto reportCreateDto)
        {
            var newReport = new Report()
            {
                Content = reportCreateDto.Content,
                ObjectId = reportCreateDto.ObjectId,
                ObjectType = reportCreateDto.ObjectType,
                ReporterId = User.GetUserId(),
                Status = "Pending", // Trạng thái chưa xử lý, đã xử lý là Processed
            };
            _reportRepository.CreateReport(newReport);
            return Ok();
        }
        #endregion

        #region GetReports
        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports([FromQuery] ReportParams reportParams)
        {
            var reports = await _reportRepository.GetListReports(reportParams);
            return Ok(reports);
        }
        #endregion

        #region UpdateStatusReport
        [HttpPut("update-status-report/{reportId}")]
        public async Task<ActionResult> UpdateStatusReport(int reportId,[FromBody] ReportUpdateDto reportUpdateDto)
        {
            var report = await _reportRepository.GetReport(reportId);
            report.Status = reportUpdateDto.Status;
            if (await _reportRepository.Save()) return Ok();
            return BadRequest("Failed to update status report");
        } 
        #endregion
    }
}
