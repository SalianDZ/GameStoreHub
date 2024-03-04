using GameStoreHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreHub.Data.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(GenerateCategories());
        }

        private Category[] GenerateCategories()
        {
            ICollection<Category> categories = new HashSet<Category>();

            Category category;

            category = new()
            {
                Id = 1,
                Name = "Action",
                ImagePath = "~/images/Categories/actionGames.jpg"
            };

            categories.Add(category);

            category = new()
            { 
                Id = 2,
                Name = "Horror",
				ImagePath = "~/images/Categories/horrorGames.jpg"
			};

            categories.Add(category);

            category = new()
            {
                Id = 3,
                Name = "Strategy",
				ImagePath = "~/images/Categories/strategyGames.jpg"
			};

            categories.Add(category);

            category = new()
            {
                Id = 4,
                Name = "Indie",
				ImagePath = "~/images/Categories/indieGames.jpg"
			};

            categories.Add(category);

            category = new()
            {
                Id = 5,
                Name = "Sports",
				ImagePath = "~/images/Categories/sportGames.jpg"
			};

            categories.Add(category);

            return categories.ToArray();
        }
    
    }
}