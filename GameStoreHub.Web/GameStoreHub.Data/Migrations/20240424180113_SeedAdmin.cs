using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStoreHub.Data.Migrations
{
    public partial class SeedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("30f0662a-29c5-48df-8b57-9c61c671e0fb"), 0, "99fa0334-8b95-44f7-b463-1171d7e48b46", "admin@gamefinity.com", false, "Admin", "Admin", false, null, "ADMIN@GAMEFINITY.com", "ADMIN@GAMEFINITY.COM", "AQAAAAEAACcQAAAAEP6HBNREH9Mkpk1HC/mZSdZ4K2+7X5A1FgfPtxgeuBkfuSp+GRhfwkc35x+TDUfOcg==", null, false, "A1B2C3D4E5F6G7H8I9J0K1L2M3N4O5P", false, "admin@gamefinity.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("30f0662a-29c5-48df-8b57-9c61c671e0fb"));
        }
    }
}