using API.Data;
using API.DTOs.Movies.Certifications;
using API.DTOs.Movies.Genres;
using API.DTOs.Movies.Movie;
using API.Entities.Movies;
using API.Helpers.Pagination;
using API.Helpers.Params.Movies;
using API.Interfaces.Movies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Extentions;
using API.DTOs.Photos;

namespace API.Repositories.Movies
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public MovieRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public void ApproveMovie(Movie movie)
        {
            _dataContext.Entry(movie).State = EntityState.Modified;
        }

        public void CreateMovie(Movie movie)
        {
            _dataContext.Movies.Add(movie);
        }

        public void DeleteMovie(Movie movie)
        {
            _dataContext.Entry(movie).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Movie>> GetListMoviesForEdit()
        {
            return await _dataContext.Movies.Where(m => m.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<MovieOutputDto> GetMovieById(int movieId)
        {
           return await _dataContext.Movies
                .Where(m => m.Id == movieId && m.IsDeleted == false)
                .ProjectTo<MovieOutputDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Movie> GetMovieByIdForEdit(int movieId)
        {
            return await _dataContext.Movies
                .Include(b => b.Banner)
                .Include(c => c.Certification)
                .Include(r => r.Ratings)
                .Include(mg => mg.MovieGenres)
                    .ThenInclude(g => g.Genre)
                .Include(mc => mc.MovieActors)
                    .ThenInclude(a => a.Actor)
                .Include(d => d.Director)
                .SingleOrDefaultAsync(m => m.Id == movieId);
        }

        public async Task<bool> MovieExits(string movieTitle)
        {
            return await _dataContext.Movies
                .AnyAsync(m => m.Title == movieTitle && m.IsDeleted == false);
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateMovie(Movie movie)
        {
            _dataContext.Entry(movie).State = EntityState.Modified;
        }
        #region GetListMovies
        public async Task<IEnumerable<ListMoviesOutputDto>> GetListMovies(MovieParams movieParams, int userId)
        {
            var query = _dataContext.Movies
                .Where(m => m.IsDeleted == false)
                .AsQueryable();

            // Filter by keyword
            if (!string.IsNullOrWhiteSpace(movieParams.Keyword))
            {
                query = query.Where(m => m.Title.Contains(movieParams.Keyword));
            }

            // Filter by certifications
            if (movieParams.CertificationId != null && movieParams.CertificationId.Any())
            {
                query = query.Where(m => movieParams.CertificationId.Contains(m.CertificationId));
            }

            // Filter by genres
            if (movieParams.GenreId != null && movieParams.GenreId.Any())
            {
                query = query.Where(m => m.MovieGenres.Any(g => movieParams.GenreId.Contains(g.GenreId)));
            }

            // Filter by actors
            if (movieParams.ActorId != null && movieParams.ActorId.Any())
            {
                query = query.Where(m => m.MovieActors.Any(g => movieParams.ActorId.Contains(g.ActorId)));
            }

            // Filter by directors
            if (movieParams.DirectorId != null && movieParams.DirectorId.Any())
            {
                query = query.Where(m => movieParams.DirectorId.Contains(m.DirectorId));
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(movieParams.Status))
            {
                query = query.Where(m => m.Status.Equals(movieParams.Status));
            }

            // Sorting
            query = movieParams.OrderBy switch
            {
                "status" => query.OrderBy(m => m.Status),
                "release-date" => query.OrderBy(m => m.ReleaseDate),
                _ => query.OrderBy(m => m.Title),
            };

            if (!string.IsNullOrWhiteSpace(movieParams.SortOrder) &&
                movieParams.SortOrder == "desc")
            {
                query = movieParams.OrderBy switch
                {
                    "status" => query.OrderByDescending(m => m.Status),
                    "release-date" => query.OrderByDescending(m => m.ReleaseDate),
                    _ => query.OrderByDescending(m => m.Title),
                };
            }

            // Limit number of movies returned
            if (movieParams.PageSize.HasValue && movieParams.PageSize > 0)
            {
                query = query.Take(movieParams.PageSize.Value);
            }

            var query2 = query.Select(m => new ListMoviesOutputDto
            {
                Id = m.Id,
                Title = m.Title,
                AverageRating = _dataContext.Ratings
                    .Where(r => r.MovieId == m.Id)
                    .Select(r => (double?)r.Score)
                    .Average() ?? 0,
                TotalRatings = _dataContext.Ratings.Count(r => r.MovieId == m.Id),
                IsInWatchList = userId != -1 ? _dataContext.Watchlists
                    .Any(w => w.MovieId == m.Id && w.AppUserId == userId) : false,
                Banner = _mapper.Map<BannerDto>(m.Banner),
            });

            query2 = movieParams.OrderBy switch
            {
                "score" => query2.OrderBy(m => m.AverageRating),
                _ => query2.OrderByDescending(m => m.Title),
            };

            if (!string.IsNullOrWhiteSpace(movieParams.SortOrder) &&
                movieParams.SortOrder == "desc")
            {
                query2 = movieParams.OrderBy switch
                {
                    "score" => query2.OrderByDescending(m => m.AverageRating),
                     _ => query2.OrderByDescending(m => m.Title),
                };
            }

            // Filter by rating range
            if (movieParams.MinRating.HasValue && movieParams.MaxRating.HasValue)
            {
                query2 = query2.Where(m => m.AverageRating >= movieParams.MinRating && m.AverageRating <= movieParams.MaxRating);
            }

            var movies = await query2.ToListAsync();


            return movies;
        }

        #endregion
        public async Task<IEnumerable<GenreOutputDto>> GetListGenresByMovieId(int movieId)
        {
            return await _dataContext.MovieGenres
                .Where(mg => mg.MovieId == movieId)
                .Select(mg => mg.Genre)
                .ProjectTo<GenreOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}