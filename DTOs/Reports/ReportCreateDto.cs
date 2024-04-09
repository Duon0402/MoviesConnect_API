namespace API.DTOs.Reports
{
    public class ReportCreateDto
    {
        public string Content { get; set; }
        public string ObjectType { get; set; } // Loại đối tượng (rating, movie, hoặc một loại khác)
        public int ObjectId { get; set; }
        public int ReporterId { get; set; }
    }
}
