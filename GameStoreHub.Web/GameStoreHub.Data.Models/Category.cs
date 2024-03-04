using System.ComponentModel.DataAnnotations;
using static GameStoreHub.Common.EntityValidationConstants.Category;

namespace GameStoreHub.Data.Models
{
	public class Category
	{
        public Category()
        {
            Games = new HashSet<Game>();
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

        [Required]
		public string ImagePath { get; set; }

		public virtual ICollection<Game> Games { get; set; }

    }
}
