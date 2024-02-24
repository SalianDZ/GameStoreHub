using Microsoft.AspNetCore.Identity;

namespace GameStoreHub.Data.Models
{
	public class ApplicationUser : IdentityUser<Guid>
	{
        public ApplicationUser()
        {
                Id = Guid.NewGuid();
        }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
