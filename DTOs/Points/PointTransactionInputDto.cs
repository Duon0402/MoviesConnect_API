namespace API.DTOs.Points
{
    public class PointTransactionInputDto
    {
        public int UserId { get; set; }
        public int PointsChange { get; set; }
        public string Description { get; set; }
    }
}
