using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStoreHub.Data.Models
{
	public class OrderGame
	{
        public OrderGame()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; } = null!;

        [Required]
        public Guid GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = null!;

        [Required]
        public decimal PriceAtPurchase { get; set; }

        public bool IsActive {get; set; }
    }
}
