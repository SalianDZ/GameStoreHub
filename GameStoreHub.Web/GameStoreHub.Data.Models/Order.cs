using GameStoreHub.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStoreHub.Data.Models
{
	public class Order
	{
        public Order()
        {
            Id = Guid.NewGuid();
            OrderGames = new HashSet<OrderGame>();
            IsActive = true;
            OrderStatus = OrderStatus.InCart;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public virtual ICollection<OrderGame> OrderGames { get; set; }
    }
}
