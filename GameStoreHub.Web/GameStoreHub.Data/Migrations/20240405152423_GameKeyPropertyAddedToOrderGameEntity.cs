using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStoreHub.Data.Migrations
{
    public partial class GameKeyPropertyAddedToOrderGameEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("3542c6ad-9a41-4104-9007-956023b2bc2c"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("45b38bb7-0694-4499-9852-29f4481c9925"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("791e45f5-980b-4521-8ebc-2f6f828e686f"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("a26810d3-1158-4ab8-9f6f-0f870f660ce7"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("ddeb2a06-f35f-46eb-b0c1-d1530f4fe460"));

            migrationBuilder.AddColumn<string>(
                name: "GameKey",
                table: "OrderGames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Description", "Developer", "ImagePath", "IsActive", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("2ad62009-2c62-42a2-b888-5bf0769dd430"), 3, "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.", "Celestial Studios", "images/game/astralVoyagers.jpeg", true, 60.00m, new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Astral Voyagers" },
                    { new Guid("695392f7-1089-4e1a-ba67-17e5d311de65"), 1, "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.", "Combat Core Studios", "images/game/ShadowFighters.jpeg", true, 20.00m, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shadow Fighters" },
                    { new Guid("75d37a31-aebf-43d5-a866-1534b874cb3e"), 4, "Solve puzzles and battle mythical creatures to uncover ancient treasures.", "Labyrinth Games", "images/game/CrypticDungeons.jpeg", true, 30.00m, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cryptic Dungeons" },
                    { new Guid("83dc1745-45e2-406e-9e23-8f786a82c9c2"), 5, "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.", "Adrenaline Motorsports", "images/game/SpeedDemons.jpeg", true, 50.00m, new DateTime(2022, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Speed Demons" },
                    { new Guid("d3f7e57b-d7ce-44a6-b158-6e6907861c4e"), 2, "Command your armies in epic battles to conquer new lands and expand your empire.", "Strategem Interactive", "images/game/WarlordsofTerra.jpeg", true, 45.00m, new DateTime(2021, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warlords of Terra" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("2ad62009-2c62-42a2-b888-5bf0769dd430"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("695392f7-1089-4e1a-ba67-17e5d311de65"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("75d37a31-aebf-43d5-a866-1534b874cb3e"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("83dc1745-45e2-406e-9e23-8f786a82c9c2"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("d3f7e57b-d7ce-44a6-b158-6e6907861c4e"));

            migrationBuilder.DropColumn(
                name: "GameKey",
                table: "OrderGames");

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Description", "Developer", "ImagePath", "IsActive", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("3542c6ad-9a41-4104-9007-956023b2bc2c"), 3, "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.", "Celestial Studios", "images/game/astralVoyagers.jpeg", true, 60.00m, new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Astral Voyagers" },
                    { new Guid("45b38bb7-0694-4499-9852-29f4481c9925"), 1, "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.", "Combat Core Studios", "images/game/ShadowFighters.jpeg", true, 20.00m, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shadow Fighters" },
                    { new Guid("791e45f5-980b-4521-8ebc-2f6f828e686f"), 5, "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.", "Adrenaline Motorsports", "images/game/SpeedDemons.jpeg", true, 50.00m, new DateTime(2022, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Speed Demons" },
                    { new Guid("a26810d3-1158-4ab8-9f6f-0f870f660ce7"), 4, "Solve puzzles and battle mythical creatures to uncover ancient treasures.", "Labyrinth Games", "images/game/CrypticDungeons.jpeg", true, 30.00m, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cryptic Dungeons" },
                    { new Guid("ddeb2a06-f35f-46eb-b0c1-d1530f4fe460"), 2, "Command your armies in epic battles to conquer new lands and expand your empire.", "Strategem Interactive", "images/game/WarlordsofTerra.jpeg", true, 45.00m, new DateTime(2021, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warlords of Terra" }
                });
        }
    }
}
