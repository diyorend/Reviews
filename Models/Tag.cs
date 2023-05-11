using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reviews.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }


        [ForeignKey("Review")]
        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
