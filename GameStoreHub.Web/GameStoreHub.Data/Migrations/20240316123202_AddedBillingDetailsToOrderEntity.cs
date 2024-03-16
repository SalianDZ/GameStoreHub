using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStoreHub.Data.Migrations
{
    public partial class AddedBillingDetailsToOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("80ff9c4d-f15f-44a2-b7ad-46b2e594eda6"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("a63624ce-1c15-400a-a754-2f546d6326d7"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("b97cd115-488f-4e9b-9d26-6ff352a09e59"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("d8ff8163-66a2-4914-a2d4-da8668fbbbe2"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("d964bfc7-e8ce-42fc-97b4-d2ab823dc4e5"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderNotes",
                table: "Orders",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Description", "Developer", "ImagePath", "IsActive", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("3580d600-c40e-4e29-80e1-6af097b5dfac"), 1, "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.", "Combat Core Studios", "images/game/ShadowFighters.jpeg", true, 20.00m, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shadow Fighters" },
                    { new Guid("61cb249e-7ba1-42e1-83bd-3e5ca32a60f3"), 2, "Command your armies in epic battles to conquer new lands and expand your empire.", "Strategem Interactive", "images/game/WarlordsofTerra.jpeg", true, 45.00m, new DateTime(2021, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warlords of Terra" },
                    { new Guid("834ba258-3651-488c-b07b-6c4031cbe0bd"), 3, "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.", "Celestial Studios", "images/game/astralVoyagers.jpeg", true, 60.00m, new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Astral Voyagers" },
                    { new Guid("b291b4d4-1c5b-47ab-8a3f-d2b3107582b6"), 4, "Solve puzzles and battle mythical creatures to uncover ancient treasures.", "Labyrinth Games", "images/game/CrypticDungeons.jpeg", true, 30.00m, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cryptic Dungeons" },
                    { new Guid("e99a322e-9306-434a-bce3-b4ce3d277b82"), 5, "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.", "Adrenaline Motorsports", "images/game/SpeedDemons.jpeg", true, 50.00m, new DateTime(2022, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Speed Demons" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("3580d600-c40e-4e29-80e1-6af097b5dfac"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("61cb249e-7ba1-42e1-83bd-3e5ca32a60f3"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("834ba258-3651-488c-b07b-6c4031cbe0bd"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("b291b4d4-1c5b-47ab-8a3f-d2b3107582b6"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("e99a322e-9306-434a-bce3-b4ce3d277b82"));

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNotes",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Description", "Developer", "ImagePath", "IsActive", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("80ff9c4d-f15f-44a2-b7ad-46b2e594eda6"), 4, "Solve puzzles and battle mythical creatures to uncover ancient treasures.", "Labyrinth Games", "images/game/CrypticDungeons.jpeg", true, 30.00m, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cryptic Dungeons" },
                    { new Guid("a63624ce-1c15-400a-a754-2f546d6326d7"), 1, "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.", "Combat Core Studios", "images/game/ShadowFighters.jpeg", true, 20.00m, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shadow Fighters" },
                    { new Guid("b97cd115-488f-4e9b-9d26-6ff352a09e59"), 3, "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.", "Celestial Studios", "images/game/astralVoyagers.jpeg", true, 60.00m, new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Astral Voyagers" },
                    { new Guid("d8ff8163-66a2-4914-a2d4-da8668fbbbe2"), 2, "Command your armies in epic battles to conquer new lands and expand your empire.", "Strategem Interactive", "images/game/WarlordsofTerra.jpeg", true, 45.00m, new DateTime(2021, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warlords of Terra" },
                    { new Guid("d964bfc7-e8ce-42fc-97b4-d2ab823dc4e5"), 5, "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.", "Adrenaline Motorsports", "images/game/SpeedDemons.jpeg", true, 50.00m, new DateTime(2022, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Speed Demons" }
                });
        }
    }
}
