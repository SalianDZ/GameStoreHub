namespace GameStoreHub.Common
{
	public static class EntityValidationConstants
	{
		public static class Game
		{
			public const int TitleMinLength = 3;
			public const int TitleMaxLength = 50;

			public const int DescriptionMinLength = 15;
			public const int DescriptionMaxLength = 500;

			public const string PriceMinRange = "0";
			public const string PriceMaxRange = "10000.00";

			public const int DeveloperMinLength = 3;
			public const int DeveloperMaxLength = 100;
		}

		public static class Category
		{
			public const int NameMinLength = 3;
			public const int NameMaxLength = 50;
		}

		public static class Order
		{
			public const int DefaultTotalPriceValue = 0;
        }

		public static class Review
		{
			public const string RatingMinValue = "1";
			public const string RatingMaxValue = "5";
			public const int CommentMaxLength = 500;
		}
    }
}
