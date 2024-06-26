﻿using API.DTOs.Movies.Genres;
using API.DTOs.Movies.Movie;
using API.DTOs.Photos;
using API.Entities.Movies;
using API.Entities.Movies.Persons;
using API.Entities.Users;
using API.Extentions;
using API.Helpers.Params;
using API.Helpers.Params.Movies;
using API.Interfaces;
using API.Interfaces.Movies;
using API.Interfaces.Movies.Persons;
using API.Repositories.Movies.Persons;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies
{
    public class MovieController : BaseApiController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IRatingRepository _ratingRepository;
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IRecommendMovieService _recommendMovieService;
        private readonly IActorReponsitory _actorReponsitory;

        public MovieController(IMovieRepository movieRepository, IMapper mapper,
            IPhotoService photoService, IRatingRepository ratingRepository,
            IWatchlistRepository watchlistRepository, IRecommendMovieService recommendMovieService,
            IActorReponsitory actorReponsitory)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _photoService = photoService;
            _ratingRepository = ratingRepository;
            _watchlistRepository = watchlistRepository;
            _recommendMovieService = recommendMovieService;
            _actorReponsitory = actorReponsitory;
        }
        #region CreateMovie
        [Authorize]
        [HttpPost("CreateMovie")]
        public async Task<ActionResult<int>> CreateMovie([FromBody] MovieCreateDto movieCreate)
        {
            var userRoles = User.GetRoles();

            if (movieCreate == null) return BadRequest("Invalid data");
            if (await _movieRepository.MovieExits(movieCreate.Title)) return BadRequest("Movie already exists");

            var banner = new Banner
            {
                PublicId = "default_banner",
                Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741602/moviebanner_djgd3a.jpg"
            };

            var newMovie = new Movie()
            {
                CreatedId = User.GetUserId(),
                CreatedAt = DateTime.Now,
                CertificationId = movieCreate.CertificationId,
                DirectorId = movieCreate.DirectorId,
                Banner = banner,
                MovieGenres = movieCreate.GenreIds
                    .Select(genreId => new MovieGenre { GenreId = genreId }).ToList(),
                MovieActors = movieCreate.ActorIds
                    .Select(actorId => new MovieActor { ActorId = actorId}).ToList(),
            };

            newMovie.Banner.MovieId = newMovie.Id;

            _mapper.Map(movieCreate, newMovie);
            _movieRepository.CreateMovie(newMovie);

            if(await _movieRepository.Save()) return Ok(newMovie.Id);
            return BadRequest("Failed to create new movie");
        }
        #endregion

        #region UpdateMovie
        [Authorize]
        [HttpPut("UpdateMovie/{movieId}")]
        public async Task<ActionResult<int>> UpdateMovie(int movieId,[FromBody]MovieUpdateDto movieUpdate)
        {
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);
            if(movie == null || movie.IsDeleted == true) return NotFound();
            if(movieUpdate == null) return BadRequest("Invalid data");

            movie.UpdatedAt = DateTime.Now;
            movie.UpdatedId = User.GetUserId();
            movie.MovieGenres.Clear();
            movie.MovieActors.Clear();

            foreach (var actorId in movieUpdate.ActorIds)
            {
                movie.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actorId });
            }

            foreach (var genreId in movieUpdate.GenreIds)
            {
                movie.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
            }

            _mapper.Map(movieUpdate, movie);
            _movieRepository.UpdateMovie(movie);
            if (await _movieRepository.Save()) return Ok(movie.Id);

            return BadRequest("Failed to update movie");
        }
        #endregion

        #region DeleteMovie
        [Authorize]
        [HttpDelete("DeleteMovie/{movieId}")]
        public async Task<ActionResult> DeleteMovie(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);
            if (movie == null) return NotFound();

            movie.IsDeleted = true;
            movie.DeletedId = User.GetUserId();
            movie.DeletedAt = DateTime.Now;

            _movieRepository.DeleteMovie(movie);
            if(await _movieRepository.Save()) return NoContent();

            return BadRequest("Failed to delete movie");
        }
        #endregion

        #region GetMovieById
        [HttpGet("GetMovieById/{movieId}")]
        public async Task<ActionResult<MovieOutputDto>> GetMovieById(int movieId, [FromQuery] RatingParams ratingParams)
        {
            var movie = await _movieRepository.GetMovieById(movieId);
            if (movie == null) return NotFound();
            ratingParams.RatingViolation = true;
            var rating = await _ratingRepository.GetListRatings(movieId, ratingParams);
            movie.Genres = await _movieRepository.GetListGenresByMovieId(movieId);
            movie.Actors = await _actorReponsitory.GetListActorsByMovieId(movieId);
            movie.TotalRatings = rating.Count();
            movie.AverageRating = rating.CalculateRatingScore();
            movie.IsInWatchlist = User.Identity.IsAuthenticated ? await _watchlistRepository.ExistWatchlistItem(User.GetUserId(), movieId) : false;
            return Ok(movie);
        }
        #endregion

        #region GetListGenresByMovieId
        [HttpGet("GetListGenresByMovieId/{movieId}")]
        public async Task<ActionResult<GenreOutputDto>> GetListGenresByMovieId(int movieId)
        {
            var genres = await _movieRepository.GetListGenresByMovieId(movieId);
            return Ok(genres);
        }
        #endregion

        #region GetListMovies
        [HttpGet("GetListMovies")]
        public async Task<ActionResult<IEnumerable<ListMoviesOutputDto>>> GetListMovies([FromQuery] MovieParams movieParams)
        {
            int userId = -1;

            if (User.Identity.IsAuthenticated)
            {
                userId = User.GetUserId();
            }

            var movies = await _movieRepository.GetListMovies(movieParams, userId);
            return Ok(movies);
        }
        #endregion

        #region GetListRecommendMovies
        [Authorize]
        [HttpGet("GetListRecommendMovies")]
        public async Task<IEnumerable<ListMoviesOutputDto>> GetListRecommendMovies()
        {
            return await _recommendMovieService.GetListRecommendMovies(User.GetUserId());
        }
        #endregion

        #region SetBanner
        [HttpPost("SetBanner/{movieId}")]
        public async Task<ActionResult<AvatarDto>> SetBanner(int movieId, IFormFile file)
        {
            if (file == null) return BadRequest("File image invalid");
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);
            if (movie == null) return NotFound();

            //if (movie.Banner.PublicId != null || movie.Banner.PublicId != "default_banner")
            //{
            //    await _photoService.DeletePhoto(movie.Banner.PublicId);
            //}

            var result = await _photoService.AddPhoto(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var banner = new Banner
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MovieId = movieId
            };

            movie.Banner = banner;

            if (await _movieRepository.Save())
            {
                return Ok();
            }

            return BadRequest("Problem adding photo");
        }
        #endregion
    }
}
