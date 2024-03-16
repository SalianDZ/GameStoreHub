using GameStoreHub.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GameStoreHub.Common.EntityValidationConstants.Order;

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

        //Billing information

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(CityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(CountryMaxLength)]
        public string Country { get; set; } = null!;

        [Required]
        [MaxLength(ZipCodeMaxLength)]
        public string ZipCode { get; set; } = null!;

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(OrderNotesMaxLength)]
        public string? OrderNotes { get; set; }

        public virtual ICollection<OrderGame> OrderGames { get; set; }
    }
}
