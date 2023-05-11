using Microsoft.AspNetCore.Identity;

namespace Reviews.Models
{
    public class AppUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImagesUrl { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
