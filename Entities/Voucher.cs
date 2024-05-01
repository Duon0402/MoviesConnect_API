using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities
{
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
