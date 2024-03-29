﻿using API.DTOs.Users;
using API.Entities.Users;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        // users
        void UpdateUser(AppUser user);

        Task<bool> Save();

        Task<AppUser> GetUserById(int userId);

        Task<AppUser> GetUserByUsername(string username);

        Task<IEnumerable<AppUser>> GetListUsers();

        // members
        Task<MemberDto> GetMemberById(int userId);

        Task<MemberDto> GetMemberByUsername(string username);

        Task<IEnumerable<MemberDto>> GetListMembers();
    }
}