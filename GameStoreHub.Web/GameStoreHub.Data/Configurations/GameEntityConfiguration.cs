using GameStoreHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreHub.Data.Configurations
{
    public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
            .Property(g => g.Price)
            .HasColumnType("decimal(18,2)");

            builder.HasData(GenerateGames());
        }

        private Game[] GenerateGames()
        {
            ICollection<Game> games = new HashSet<Game>();

            Game game;

            game = new()
            {
                Title = "Astral Voyagers",
                Description = "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.",
                Price = 60.00M,
                Developer = "Celestial Studios",
                ImagePath = "images/games/astralVoyagers.jpeg",
                ReleaseDate = new DateTime(2022, 10, 10),
                CategoryId = 2
            };

            games.Add(game);

            game = new()
            {
                Title = "Cryptic Dungeons",
                Description = "Solve puzzles and battle mythical creatures to uncover ancient treasures.",
                Price = 30.00m,
                Developer = "Labyrinth Games",
                ImagePath = "images/games/CrypticDungeons.jpeg",
                ReleaseDate = new DateTime(2023, 2, 15),
                CategoryId = 4
            };

            games.Add(game);

            game = new()
            {
                Title = "Warlords of Terra",
                Description = "Command your armies in epic battles to conquer new lands and expand your empire.",
                Price = 45.00m,
                Developer = "Strategem Interactive",
                ImagePath = "images/games/WarlordsofTerra.jpeg",
                ReleaseDate = new DateTime(2021, 8, 23),
                CategoryId = 3
            };

            games.Add(game);

            game = new()
            {
                Title = "Speed Demons",
                Description = "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.",
                Price = 50.00m,
                Developer = "Adrenaline Motorsports",
                ImagePath = "images/games/SpeedDemons.jpeg",
                ReleaseDate = new DateTime(2022, 5, 14),
                CategoryId = 5
            };

            games.Add(game);

            game = new()
            {
                Title = "Shadow Fighters",
                Description = "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.",
                Price = 20.00m,
                Developer = "Combat Core Studios",
                ImagePath = "images/games/ShadowFighters.jpeg",
                ReleaseDate = new DateTime(2022, 11, 30),
                CategoryId = 1
            };

            games.Add(game);

            return games.ToArray();
        }
    }
}
