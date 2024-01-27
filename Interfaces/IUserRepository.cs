using API.DTOs.Users;
using API.DTOs.Users.Member;
using API.Entities.Users;
using API.Helpers.Pagination;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        // users
        void UpdateUser(AppUser user);
        Task<bool> SaveAllAsync();
        Task<AppUser> GetUserById(int userId);
        Task<AppUser> GetUserByUsername(string username);
        Task<IEnumerable<AppUser>> GetListUsers();

        // members
        Task<MemberDto> GetMemberById(int userId);
        Task<MemberDto> GetMemberByUsername(string username);
        Task<IEnumerable<MemberDto>> GetListMembers();
    }
}
