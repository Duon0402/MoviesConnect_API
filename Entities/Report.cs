using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Entities.Users;
using API.Entities.Movies;

namespace API.Entities
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string ObjectType { get; set; } // Loại đối tượng (rating, movie, hoặc một loại khác)
        public int ObjectId { get; set; }
        public int? ObjectId2 { get; set; }
        public int ReporterId { get; set; }
        public DateTime ReportTime { get; set; } // Thời gian báo cáo
        public int HandlerId { get; set; } // ID của người xử lý
        public DateTime HandlingTime { get; set; } // Thời gian xử lý
    }
}
