using GameStoreHub.Data;
using Microsoft.EntityFrameworkCore;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Services.Data;
using GameStoreHub.Data.Models;

namespace GameStoreHub.Services.Tests
{
	[TestFixture]
	public class CategoryServiceTests
	{
		private GameStoreDbContext dbContext;
		private DbContextOptions<GameStoreDbContext> dbOptions;
		private ICategoryService categoryService;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			dbOptions = new DbContextOptionsBuilder<GameStoreDbContext>()
				.UseInMemoryDatabase("GameStoreInMemory" + Guid.NewGuid())
				.Options;

			dbContext = new GameStoreDbContext(dbOptions);
			dbContext.Categories.AddRange(
				new Category { Id = 1, Name = "Electronics", ImagePath = "test/path", IsActive = true },
				new Category { Id = 2, Name = "Books", ImagePath = "test/path", IsActive = false },
				new Category { Id = 3, Name = "Home Appliances", ImagePath = "test/path", IsActive = true }
			);
			dbContext.SaveChanges();
			this.categoryService = new CategoryService(dbContext);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			dbContext.Dispose();
		}


		[Test]
		public async Task GetAllCategoriesAsync_ReturnsActiveCategories()
		{
			var categories = await categoryService.GetAllCategoriesAsync();
			Assert.IsNotNull(categories);
			Assert.AreEqual(2, categories.Count());
		}
	}
}