using Reviews.Models;

namespace Reviews.ViewModels
{
    public class DashboardViewModel
    {
        public AppUser AppUser { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
