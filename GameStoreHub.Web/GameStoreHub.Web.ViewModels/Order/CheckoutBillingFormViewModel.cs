using System.ComponentModel.DataAnnotations;
using static GameStoreHub.Common.EntityValidationConstants.Order;
namespace GameStoreHub.Web.ViewModels.Order
{
	public class CheckoutBillingFormViewModel
	{
        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
		public string Address { get; set; } = null!;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
		public string City { get; set; } = null!;

        [Required]
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength)]
        public string Country { get; set; } = null!;

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(ZipCodeMaxLength, MinimumLength = ZipCodeMinLength)]
        public string ZipCode { get; set; } = null!;

        [StringLength(OrderNotesMaxLength)]
        public string? OrderNotes { get; set; }
    }
}
