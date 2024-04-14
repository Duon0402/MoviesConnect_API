namespace API.DTOs.Reports
{
    public class ReportCreateDto
    {
        public string Content { get; set; }
        public string ObjectType { get; set; }
        public int ObjectId { get; set; }
        public int? ObjectId2 { get; set; }
    }
}
