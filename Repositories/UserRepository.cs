using API.Data;
using API.DTOs.Users;
using API.Entities.Users;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDto>> GetListMembers()
        {
            return await _dataContext.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetListUsers()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public Task<MemberDto> GetMemberById(int userId)
        {
            return _dataContext.Users
                .Where(u => u.Id == userId)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public Task<MemberDto> GetMemberByUsername(string username)
        {
            return _dataContext.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            return await _dataContext.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _dataContext.Users
                .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateUser(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}