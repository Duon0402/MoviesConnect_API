using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Entities.Movies;

namespace API.Entities
{
    public class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CreatedId { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UpdatedId { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int? DeletedId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
