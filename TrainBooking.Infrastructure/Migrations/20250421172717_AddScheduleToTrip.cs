using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Schedule",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Ticket");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Trip",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trip_ScheduleId",
                table: "Trip",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Schedule__",
                table: "Trip",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Schedule__",
                table: "Trip");

            migrationBuilder.DropIndex(
                name: "IX_Trip_ScheduleId",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Trip");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Schedule",
                table: "Ticket",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "ScheduleId");
        }
    }
}
