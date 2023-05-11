using Microsoft.AspNetCore.Identity;

namespace Reviews.Models
{
    public class AppUser: IdentityUser
    {
        public ICollection<Review>? Reviews { get; set; }
    }
}
