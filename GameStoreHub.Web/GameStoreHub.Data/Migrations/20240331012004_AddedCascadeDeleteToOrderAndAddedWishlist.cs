using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStoreHub.Data.Migrations
{
    public partial class AddedCascadeDeleteToOrderAndAddedWishlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderGames_Orders_OrderId",
                table: "OrderGames");

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("4e26da3d-15da-4a5a-a140-5532352c0cee"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("5590f045-3319-48e2-954f-844b7d3d60d0"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("9bf3c446-1628-4ee7-960f-c05ab0803c1e"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("a0548a2d-18af-450a-8b57-93b193c123e0"));

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: new Guid("fa222fd2-8a27-4650-9eeb-1bfaf56e3219"));

            migrationBuilder.AddColumn<Guid>(
                name: "WishlistId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => new { x.WishlistId, x.GameId });
                    table.ForeignKey(
                        name: "FK_WishlistItems_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Wishlists_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_GameId",
                table: "WishlistItems",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderGames_Orders_OrderId",
                table: "OrderGames",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderGames_Orders_OrderId",
                table: "OrderGames");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "Wishlists");

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

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "Description", "Developer", "ImagePath", "IsActive", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("4e26da3d-15da-4a5a-a140-5532352c0cee"), 3, "Embark on a cosmic journey across the universe, unraveling the mysteries of the stars.", "Celestial Studios", "images/game/astralVoyagers.jpeg", true, 60.00m, new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Astral Voyagers" },
                    { new Guid("5590f045-3319-48e2-954f-844b7d3d60d0"), 4, "Solve puzzles and battle mythical creatures to uncover ancient treasures.", "Labyrinth Games", "images/game/CrypticDungeons.jpeg", true, 30.00m, new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cryptic Dungeons" },
                    { new Guid("9bf3c446-1628-4ee7-960f-c05ab0803c1e"), 1, "Master martial arts and engage in fierce battles to defeat the shadow syndicate and save the world.", "Combat Core Studios", "images/game/ShadowFighters.jpeg", true, 20.00m, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shadow Fighters" },
                    { new Guid("a0548a2d-18af-450a-8b57-93b193c123e0"), 2, "Command your armies in epic battles to conquer new lands and expand your empire.", "Strategem Interactive", "images/game/WarlordsofTerra.jpeg", true, 45.00m, new DateTime(2021, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warlords of Terra" },
                    { new Guid("fa222fd2-8a27-4650-9eeb-1bfaf56e3219"), 5, "Race at breakneck speeds through city streets and winding country roads to become the ultimate champion.", "Adrenaline Motorsports", "images/game/SpeedDemons.jpeg", true, 50.00m, new DateTime(2022, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Speed Demons" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderGames_Orders_OrderId",
                table: "OrderGames",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
