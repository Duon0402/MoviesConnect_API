using API.Entities.Users;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetListUsers();
        Task<AppUser> GetUserById(int userId);
        Task<AppUser> GetUserByUsername(string username);
    }
}
