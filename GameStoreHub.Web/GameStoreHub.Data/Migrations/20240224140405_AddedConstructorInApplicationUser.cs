using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStoreHub.Data.Migrations
{
    public partial class AddedConstructorInApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("0bfb67be-9b89-4cf5-8169-c29c0e6c2e3a"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("53a9f1f7-6d3e-4b3e-a0eb-c03a4c64039e"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("79e214de-259a-47af-ae83-b610ede5264f"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("b9effa65-8913-48ef-b9f8-b7c54baa81d8"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("c925c778-7741-4028-95e0-a4ea4d97df5c"));

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Description", "Developer", "ImagePath", "IsActive", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("425eb6df-5b4b-4346-948b-650ea4d0dc6f"), 4, "Solve puzzles and battle mythical creatures to uncover ancient treasures.", "Labyrinth Games", "images/games/CrypticDungeons.jpeg", true, 30.00m, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cryptic Dungeons" },
                    { new Guid("8d73faea-cf42-4830-b138-6ae24ae5a3b1"), 5, "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.", "Adrenaline Motorsports", "images/games/SpeedDemons.jpeg", true, 50.00m, new DateTime(2022, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Speed Demons" },
                    { new Guid("a3526d1f-66da-4d00-873c-2058834651c4"), 2, "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.", "Celestial Studios", "images/games/astralVoyagers.jpeg", true, 60.00m, new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Astral Voyagers" },
                    { new Guid("bae88a45-8777-4fc8-80c1-5437000e28c7"), 3, "Command your armies in epic battles to conquer new lands and expand your empire.", "Strategem Interactive", "images/games/WarlordsofTerra.jpeg", true, 45.00m, new DateTime(2021, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warlords of Terra" },
                    { new Guid("d1cbc7e1-edc4-4569-b42c-71683bccee5d"), 1, "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.", "Combat Core Studios", "images/games/ShadowFighters.jpeg", true, 20.00m, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shadow Fighters" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("425eb6df-5b4b-4346-948b-650ea4d0dc6f"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("8d73faea-cf42-4830-b138-6ae24ae5a3b1"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("a3526d1f-66da-4d00-873c-2058834651c4"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("bae88a45-8777-4fc8-80c1-5437000e28c7"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("d1cbc7e1-edc4-4569-b42c-71683bccee5d"));

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Description", "Developer", "ImagePath", "IsActive", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("0bfb67be-9b89-4cf5-8169-c29c0e6c2e3a"), 3, "Command your armies in epic battles to conquer new lands and expand your empire.", "Strategem Interactive", "images/games/WarlordsofTerra.jpeg", true, 45.00m, new DateTime(2021, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warlords of Terra" },
                    { new Guid("53a9f1f7-6d3e-4b3e-a0eb-c03a4c64039e"), 5, "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.", "Adrenaline Motorsports", "images/games/SpeedDemons.jpeg", true, 50.00m, new DateTime(2022, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Speed Demons" },
                    { new Guid("79e214de-259a-47af-ae83-b610ede5264f"), 2, "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.", "Celestial Studios", "images/games/astralVoyagers.jpeg", true, 60.00m, new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Astral Voyagers" },
                    { new Guid("b9effa65-8913-48ef-b9f8-b7c54baa81d8"), 1, "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.", "Combat Core Studios", "images/games/ShadowFighters.jpeg", true, 20.00m, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shadow Fighters" },
                    { new Guid("c925c778-7741-4028-95e0-a4ea4d97df5c"), 4, "Solve puzzles and battle mythical creatures to uncover ancient treasures.", "Labyrinth Games", "images/games/CrypticDungeons.jpeg", true, 30.00m, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cryptic Dungeons" }
                });
        }
    }
}
