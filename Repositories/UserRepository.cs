using API.Data;
using API.DTOs.Points;
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
        private readonly IPointTransactionRepository _pointTransactionRepository;
        private readonly IVoucherRepository _voucherRepository;

        public UserRepository(DataContext dataContext, IMapper mapper, IPointTransactionRepository pointTransactionRepository,
            IVoucherRepository voucherRepository)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _pointTransactionRepository = pointTransactionRepository;
            _voucherRepository = voucherRepository;
        }

        public async Task<IEnumerable<MemberDto>> GetListMembers()
        {
            return await _dataContext.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<PointTransactionOutputDto>> GetListPointTransactions(int userId)
        {
            var pointTransactions = await _dataContext.PointTransaction
                .Where(pt => pt.UserId == userId)
                .OrderByDescending(pt => pt.TransactionDate)
                .Select(pt => new PointTransactionOutputDto
                {
                    PointsChange = pt.PointsChange,
                    Description = pt.Description,
                    TransactionDate = pt.TransactionDate
                })
                .ToListAsync();

            return pointTransactions;
        }

        public async Task<IEnumerable<AppUser>> GetListUsers()
        {
            return await _dataContext.Users
                .Include(a => a.Avatar)
                .ToListAsync();
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
                .Include(a => a.Avatar)
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _dataContext.Users
                .Include(a => a.Avatar)
                .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task UpdateContributionPoints(PointTransactionInputDto pointTransactionInput)
        {
            var user = await _dataContext.Users.FindAsync(pointTransactionInput.UserId);
            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            user.ContributionPoints += pointTransactionInput.PointsChange;
            await _dataContext.SaveChangesAsync();

            // Lưu lại lịch sử cộng trừ điểm
            await _pointTransactionRepository.AddPointTransaction(pointTransactionInput);
        }

        public void UpdateUser(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}