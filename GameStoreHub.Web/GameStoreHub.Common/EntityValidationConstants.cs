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

			//Billing information constants

			public const int AddressMinLength = 20;
			public const int AddressMaxLength = 255;

			public const int CityMinLength = 3;
			public const int CityMaxLength = 100;

			public const int CountryMinLength = 3;
			public const int CountryMaxLength = 50;

			public const int ZipCodeMinLength = 2;
			public const int ZipCodeMaxLength = 20;

			public const int PhoneNumberMinLength = 10;
			public const int PhoneNumberMaxLength = 15;

			public const int OrderNotesMaxLength = 300;
		}

		public static class Review
		{
			public const string RatingMinValue = "1";
			public const string RatingMaxValue = "5";
			public const int CommentMaxLength = 500;
		}

		public static class User
		{
			public const int FirstNameMinLength = 2;
			public const int FirstNameMaxLength = 50;

			public const int LastNameMinLength = 2;
			public const int LastNameMaxLength = 50;

			public const int PasswordMinLength = 6;
			public const int PasswordMaxLength = 100;
		}
    }
}
