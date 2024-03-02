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

        public Task<IPagedResult<MovieOutputDto>> GetPagedListMovies(MovieInputDto movieInput)
        {
            var query = _dataContext.Movies
                .Where(m => m.IsDeleted == false && m.IsApproved == true)
                .AsQueryable();

            return null;
        }

        public async Task<Movie> GetListMoviesForEdit(MovieParams movieParams)
        {
            return null;
        }

        public async Task<MovieOutputDto> GetMovieById(int movieId)
        {
           return await _dataContext.Movies
                .Where(m => m.Id == movieId && m.IsDeleted == false
                    && m.IsApproved == true)
                .ProjectTo<MovieOutputDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Movie> GetMovieByIdForEdit(int movieId)
        {
            return await _dataContext.Movies
                .Include(c => c.Certification)
                .Include(r => r.Ratings)
                .Include(mg => mg.MovieGenres)
                    .ThenInclude(g => g.Genre)
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
                .Where(m => m.IsDeleted == false && m.IsApproved == true)
                .AsQueryable();

            // loc theo keyword
            if (!string.IsNullOrWhiteSpace(movieParams.Keyword))
            {
                query = query.Where(m => m.Title.Contains(movieParams.Keyword));
            }

            // loc theo certifications
            if (movieParams.CertificationId != null && movieParams.CertificationId.Any())
            {
                query = query.Where(m => movieParams.CertificationId.Any(id => id.Equals(m.CertificationId)));
            }

            //  loc theo genres
            if (movieParams.GenreId != null && movieParams.GenreId.Any())
            {
                query = query.Where(m => m.MovieGenres.Any(g => movieParams.GenreId.Contains(g.GenreId)));
            }

            // loc theo status
            if (!string.IsNullOrWhiteSpace(movieParams.Status))
            {
                query = query.Where(m => m.Status.Equals(movieParams.Status));
            }

            query = movieParams.OrderBy switch
            {
                "status" => query.OrderBy(m => m.Status),
                "release-date" => query.OrderBy(m => m.ReleaseDate),
                _ => query.OrderBy(m => m.Title),
            };

            var sortOrder = movieParams.SortOrder?.ToLowerInvariant();
            if (sortOrder == "desc")
            {
                query = movieParams.OrderBy switch
                {
                    "status" => query.OrderByDescending(m => m.Status),
                    "release-date" => query.OrderByDescending(m => m.ReleaseDate),
                    _ => query.OrderByDescending(m => m.Title),
                };
            }

            // so luong phim lay ra

            if (movieParams.PageSize.HasValue && movieParams.PageSize > 0)
            {
                query = query.Take(movieParams.PageSize.Value);
            }

            var movies = await query.Select(m => new ListMoviesOutputDto
            {
                Id = m.Id,
                Title = m.Title,
                AverageRating = _dataContext.Ratings
                    .Where(r => r.MovieId == m.Id)
                    .Select(r => (double?)r.Score) // Chuyển đổi sang kiểu double nullable
                    .Average() ?? 0, // Nếu không có giá trị, sẽ trả về 0
                TotalRatings = _dataContext.Ratings.Count(r => r.MovieId == m.Id),
                IsInWatchList = userId != -1 ? _dataContext.Watchlists
                    .Any(w => w.MovieId == m.Id && w.AppUserId == userId) : false,
                BannerOutput = _mapper.Map<BannerDto>(m.Banner),
            })
            .ToListAsync();
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