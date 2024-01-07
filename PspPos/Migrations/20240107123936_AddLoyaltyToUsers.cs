using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PspPos.Migrations
{
    /// <inheritdoc />
    public partial class AddLoyaltyToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoyaltyPoints",
                table: "Users",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoyaltyPoints",
                table: "Users");
        }
    }
}
