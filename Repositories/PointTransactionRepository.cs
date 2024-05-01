using API.Data;
using API.DTOs.Points;
using API.Entities.Users;
using API.Interfaces;

namespace API.Repositories
{
    public class PointTransactionRepository : IPointTransactionRepository
    {
        private readonly DataContext _dataContext;

        public PointTransactionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddPointTransaction(PointTransactionInputDto pointTransactionInput)
        {
            var pointTran = new PointTransaction
            {
                UserId = pointTransactionInput.UserId,
                PointsChange = pointTransactionInput.PointsChange,
                Description = pointTransactionInput.Description,
                TransactionDate = DateTime.Now,
            };

            await _dataContext.PointTransaction.AddAsync(pointTran);
        }
    }
}
