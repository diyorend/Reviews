using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reviews.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? ParentCommentId { get; set; }

        [ForeignKey("Review")]
        public int ReviewId { get; set; }
        public Review Review { get; set; }
        
    }
}
