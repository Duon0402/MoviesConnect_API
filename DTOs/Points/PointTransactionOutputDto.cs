namespace API.DTOs.Points
{
    public class PointTransactionOutputDto
    {
        public int PointsChange { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
