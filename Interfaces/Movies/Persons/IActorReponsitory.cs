using API.DTOs.Movies.Actor;
using API.DTOs.Movies.Movie;
using API.Entities.Movies.Persons;

namespace API.Interfaces.Movies.Persons
{
    public interface IActorReponsitory
    {
        void CreateActor(Actor actor);
        void UpdateActor(Actor actor);
        void DeleteActor(Actor actor);
        Task<bool> ActorExits(string actorName);
        Task<bool> Save();
        Task<ActorOutputDto> GetActor(int actorId);
        Task<Actor> GetActorForEdit(int actorId);
        Task<IEnumerable<ActorOutputDto>> GetListActors();
        Task<IEnumerable<ActorOutputDto>> GetListActorsByMovieId(int movieId);
        Task<IEnumerable<ListMoviesOutputDto>> GetListMoviesByActorId(int actorId);
    }
}
