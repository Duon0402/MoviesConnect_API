using API.DTOs.Admin;
using API.DTOs.Movies.Ratings;
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
        private readonly IRatingRepository _ratingRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        public AdminController(UserManager<AppUser> userManager, IMovieRepository movieRepository,
            IReportRepository reportRepository, IRatingRepository ratingRepository, 
            IStatisticsRepository statisticsRepository)
        {
            _userManager = userManager;
            _movieRepository = movieRepository;
            _reportRepository = reportRepository;
            _ratingRepository = ratingRepository;
            _statisticsRepository = statisticsRepository;
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
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpDelete("DeleteRating")]
        public async Task<ActionResult> DeleteRating(int userId, int movieId)
        {
            var rating = await _ratingRepository.GetRatingForEdit(movieId, userId);
            if (rating == null) return BadRequest("Rating is deleted before");
            if (rating.IsDeleted == true) return BadRequest("Rating is deleted before");
            rating.IsDeleted = true;
            rating.DeletedAt = DateTime.Now;
            rating.DeletedId = User.GetUserId();

            _ratingRepository.RemoveRating(rating);
            if (await _ratingRepository.Save()) return Ok();

            return BadRequest("Failed to delete rating");
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPut("UpdateRatingStatus")]
        public async Task<ActionResult> UpdateRatingStatus([FromQuery] int userId, int movieId)
        {
            var rating = await _ratingRepository.GetRatingForEdit(movieId, userId);
            if (rating == null) return BadRequest();
            if (rating.RatingViolation == true) return Ok();
            else rating.RatingViolation = true;

            _ratingRepository.EditRating(rating);
            if (await _ratingRepository.Save()) return Ok();

            return BadRequest("Failed to update status rating");
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("GetRatingForHandle")]
        public async Task<ActionResult<RatingOutputDto>> GetRatingForHandle([FromQuery] int movieId, int userId)
        {
            var rating = await _ratingRepository.GetRating(movieId, userId);
            if (rating == null) return BadRequest("Rating is deleted");
            return Ok(rating);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("GetStatistics")]
        public async Task<ActionResult<StatisticsDto>> GetStatistics()
        {
            return await _statisticsRepository.GetStatistics();
        }
    }
}
