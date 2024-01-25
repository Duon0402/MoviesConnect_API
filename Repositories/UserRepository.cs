using API.Data;
using API.Entities.Users;
using API.Interfaces;
using AutoMapper;
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
        public async Task<IEnumerable<AppUser>> GetListUsers()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            return await _dataContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _dataContext.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}
