using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MellonBank.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_AspNetUsers_ApplicationUserId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_ApplicationUserId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BankAccounts");

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AUD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CHF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GBP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    USD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetrievedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "BankAccounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BankAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ApplicationUserId",
                table: "BankAccounts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_AspNetUsers_ApplicationUserId",
                table: "BankAccounts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
