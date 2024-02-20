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
                Name = "Action"
            };

            categories.Add(category);

            category = new()
            { 
                Id = 2,
                Name = "Horror"
            };

            categories.Add(category);

            category = new()
            {
                Id = 3,
                Name = "Strategy"
            };

            categories.Add(category);

            category = new()
            {
                Id = 4,
                Name = "Indie"
            };

            categories.Add(category);

            category = new()
            {
                Id = 5,
                Name = "Sports"
            };

            categories.Add(category);

            return categories.ToArray();
        }
    
    }
}