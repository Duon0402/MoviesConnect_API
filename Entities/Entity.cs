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
        public DateTime CreateAt { get; set; }

        public int? UpdatedId { get; set; }
        public DateTime? UpdateAt { get; set; }

        public int? DeletedId { get; set; }
        public DateTime? DeleteAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<MovieGenre> MovieGenres { get; set; }
        public Certification Certification { get; set; }
    }
}
