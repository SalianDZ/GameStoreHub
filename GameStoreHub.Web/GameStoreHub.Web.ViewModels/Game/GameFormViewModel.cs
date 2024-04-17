using GameStoreHub.Web.ViewModels.Category;
using System.ComponentModel.DataAnnotations;
using static GameStoreHub.Common.EntityValidationConstants.Game;
using static GameStoreHub.Common.EntityValidationConstants.Category;

namespace GameStoreHub.Web.ViewModels.Game
{
	public class GameFormViewModel
	{
		[Required]
		[StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
		public string Title { get; set; } = null!;

		[Required]
		[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
		public string Description { get; set; } = null!;

		[Required]
		[Range(typeof(decimal), PriceMinRange, PriceMaxRange)]
		public decimal Price { get; set; }

		[Required]
		[StringLength(DeveloperMaxLength, MinimumLength = DeveloperMinLength)]
		public string Developer { get; set; } = null!;

		[Required]
		[Display(Name = "Image Path")]
		[MinLength(PathImageMinLength)]
		public string ImagePath { get; set; } = null!;

		[Required]
		[Display(Name = "Date Of Release")]
		public DateTime ReleaseDate { get; set; }

		[Display(Name = "Category")]
		[Range(CategoryFormMinRange, CategoryFormMaxRange)]
		public int CategoryId { get; set; }

		public IEnumerable<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();

	}
}
