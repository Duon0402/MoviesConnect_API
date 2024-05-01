using API.Data;
using API.DTOs.Movies.Actor;
using API.DTOs.Movies.Movie;
using API.Entities.Movies;
using API.Entities.Movies.Persons;
using API.Interfaces.Movies.Persons;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Movies.Persons
{
    public class ActorReponsitory : IActorReponsitory
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ActorReponsitory(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<bool> ActorExits(string actorName)
        {
            return await _dataContext.Actors
                .AnyAsync(a => a.Name == actorName && a.IsDeleted == false);
        }

        public void CreateActor(Actor actor)
        {
            _dataContext.Actors.Add(actor);
        }

        public void DeleteActor(Actor actor)
        {
            _dataContext.Entry(actor).State = EntityState.Modified;
        }

        public async Task<ActorOutputDto> GetActor(int actorId)
        {
            return await _dataContext.Actors
                .Where(a => a.Id == actorId & a.IsDeleted == false)
                .ProjectTo<ActorOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Actor> GetActorForEdit(int actorId)
        {
            return await _dataContext.Actors
                .Include(a => a.ActorImage)
                .SingleOrDefaultAsync(a => a.Id == actorId);
        }

        public async Task<IEnumerable<ActorOutputDto>> GetListActors()
        {
            return await _dataContext.Actors
                .Where(a => a.IsDeleted == false)
                .ProjectTo<ActorOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActorOutputDto>> GetListActorsByMovieId(int movieId)
        {
            return await _dataContext.MovieActors
                .Where(ma => ma.MovieId == movieId)
                .Select(a => a.Actor)
                .Where(a => a.IsDeleted == false)
                .ProjectTo<ActorOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ListMoviesOutputDto>> GetListMoviesByActorId(int actorId)
        {
            return await _dataContext.MovieActors
                .Where(ma => ma.ActorId == actorId)
                .Select(m => m.Movie)
                .ProjectTo<ListMoviesOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateActor(Actor actor)
        {
            _dataContext.Entry(actor).State = EntityState.Modified;
        }
    }
}
