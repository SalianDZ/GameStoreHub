using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static GameStoreHub.Common.EntityValidationConstants.User;

namespace GameStoreHub.Data.Models
{
	public class ApplicationUser : IdentityUser<Guid>
	{
        public ApplicationUser()
        {
                Id = Guid.NewGuid();
        }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]  
        public decimal WalletBalance { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
