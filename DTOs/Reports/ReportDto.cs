using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Reports
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string ObjectType { get; set; }
        public int ObjectId { get; set; }
        public int? ObjectId2 { get; set; }
        public int ReporterId { get; set; }
        public DateTime ReportTime { get; set; }
        public int HandlerId { get; set; }
        public DateTime HandlingTime { get; set; }
    }
}
