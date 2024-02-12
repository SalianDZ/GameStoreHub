using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GameStoreHub.Common.EntityValidationConstants.Game;

namespace GameStoreHub.Data.Models
{
	public class Game
	{
        public Game()
        {
			OrderGames = new HashSet<OrderGame>(); 
			Id = Guid.NewGuid();
        }

        [Key]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(TitleMaxLength)]
		public string Title { get; set; } = null!;

		[Required]
		[MaxLength(DescriptionMaxLength)]
		public string Description { get; set; } = null!;

		[Required]
        public decimal Price { get; set; }

		[Required]
		[MaxLength(DeveloperMaxLength)]
		public string Developer { get; set; } = null!;

		[Required]
        public DateTime ReleaseDate { get; set; }

		[Required]
        public int CategoryId { get; set; }

		[ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<OrderGame> OrderGames { get; set; }
    }
}
