using API.DTOs.Points;

namespace API.Interfaces
{
    public interface IPointTransactionRepository
    {
        Task AddPointTransaction(PointTransactionInputDto pointTransactionInput);
    }
}
