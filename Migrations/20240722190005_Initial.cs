using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_API_Simple_Digital_Wallet.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Address = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<double>(type: "float", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Address);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAddress = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserAddress1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_RAddress",
                        column: x => x.RAddress,
                        principalTable: "Users",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_SAddress",
                        column: x => x.SAddress,
                        principalTable: "Users",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserAddress",
                        column: x => x.UserAddress,
                        principalTable: "Users",
                        principalColumn: "Address");
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserAddress1",
                        column: x => x.UserAddress1,
                        principalTable: "Users",
                        principalColumn: "Address");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RAddress",
                table: "Transactions",
                column: "RAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SAddress",
                table: "Transactions",
                column: "SAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserAddress",
                table: "Transactions",
                column: "UserAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserAddress1",
                table: "Transactions",
                column: "UserAddress1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
