using System.ComponentModel.DataAnnotations;
using static GameStoreHub.Common.EntityValidationConstants.Review;

namespace GameStoreHub.Web.ViewModels.Review
{
	public class ReviewFormModel
	{
		[Range(RatingMinValue, RatingMaxValue)]
        public int Rating { get; set; }

		[MaxLength(CommentMaxLength)]
        public string? Comment { get; set; }
    }
}
