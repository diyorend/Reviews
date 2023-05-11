using Reviews.Models;

namespace Reviews.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? ProfileImagesUrl { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
