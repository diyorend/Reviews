using Reviews.Data.Enum;

namespace Reviews.ViewModels
{
    public class CreateReviewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Likes { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Group Group { get; set; }
        public string AppUserId { get; set; }
    }
}
