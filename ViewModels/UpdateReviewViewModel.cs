using Reviews.Data.Enum;
using Reviews.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reviews.ViewModels
{
    public class UpdateReviewViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Group Group { get; set; }
    }
}
