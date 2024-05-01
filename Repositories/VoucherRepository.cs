using API.Data;
using API.DTOs.Vouchers;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public VoucherRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task CreateVoucher(Voucher voucher)
        {
            await _dataContext.Vouchers.AddAsync(voucher);
        }

        public async Task<IEnumerable<VoucherOutputDto>> GetListVoucersByUserId(int userId)
        {
            return await _dataContext.Vouchers
                .Where(v => v.UserId  == userId)
                .ProjectTo<VoucherOutputDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(v => v.Id)
                .ToListAsync();
        }

        public async Task<VoucherOutputDto> GetVoucher(int voucherId)
        {
            return await _dataContext.Vouchers.Where(v => v.Id == voucherId)
                .ProjectTo<VoucherOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
