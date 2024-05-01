using API.DTOs.Vouchers;
using API.Entities;

namespace API.Interfaces
{
    public interface IVoucherRepository
    {
        Task CreateVoucher(Voucher voucher);
        Task<bool> Save();
        Task<IEnumerable<VoucherOutputDto>> GetListVoucersByUserId(int userId);
        Task<VoucherOutputDto> GetVoucher(int voucherId);
    }
}
