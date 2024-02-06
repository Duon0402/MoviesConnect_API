using API.Data;
using API.DTOs.Movies.Genres;
using API.Entities.Movies;
using API.Helpers.Pagination;
using API.Interfaces.Movies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace API.Repositories.Movies
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GenreRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public void CreateGenre(Genre genre)
        {
            _dataContext.Add(genre);
        }
        
        public void DeleteGenre( Genre genre)
        {
            _dataContext.Entry(genre).State = EntityState.Modified;
        }

        public async Task<bool> GenreExits(string genreName)
        {
            return await _dataContext.Genres
                .AnyAsync(g => g.Name == genreName && g.IsDeleted == false);
        }

        public async Task<GenreOutputDto> GetGenreById(int genreId)
        {
            return await _dataContext.Genres
                .Where(g => g.Id == genreId && g.IsDeleted == false)
                .ProjectTo<GenreOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Genre> GetGenreByIdForEdit(int genreId)
        {
            return await _dataContext.Genres
                .SingleOrDefaultAsync(g => g.Id == genreId && g.IsDeleted == false);
        }

        //public async Task<IEnumerable<GenreOutputDto>> GetListGenres(string keyword)
        //{
        //    var genres = _dataContext.Genres
        //            .OrderBy(g => g.Name)
        //            .Where(g => g.IsDeleted == false)
        //            .AsQueryable();

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        genres = genres.Where(g => g.Name.Contains(keyword));
        //    }
        //    return await genres
        //        .ProjectTo<GenreOutputDto>(_mapper.ConfigurationProvider)
        //        .ToListAsync();
        //}

        public async Task<PagedResults<GenreOutputDto>> GetListGenres(GenreInputDto genreInput)
        {
            var query = _dataContext.Genres
                .OrderBy(g => g.Name)
                .Where(g => g.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(genreInput.Keyword))
            {
                query = query.Where(g => g.Name.Contains(genreInput.Keyword));
            }

            var genres = query
                .ProjectTo<GenreOutputDto>(_mapper.ConfigurationProvider)
                .ToPagedList(genreInput.PageNumber, genreInput.PageSize);

            return new PagedResults<GenreOutputDto>
            {
                TotalItems = genres.Count(),
                PagedItems = genres
            };
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateGenre(Genre genre)
        {
            _dataContext.Entry(genre).State = EntityState.Modified;
        }
    }
}
