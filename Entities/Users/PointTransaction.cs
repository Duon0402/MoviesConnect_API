using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Entities.Users
{
    public class PointTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int PointsChange { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
