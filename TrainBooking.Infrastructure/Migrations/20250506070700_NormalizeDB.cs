using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ__User__5E55825B1A74FCFF",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Seat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Seat",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__5E55825B1A74FCFF",
                table: "User",
                column: "Login",
                unique: true);
        }
    }
}
