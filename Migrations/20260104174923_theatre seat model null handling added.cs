using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class theatreseatmodelnullhandlingadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "SeatAvailabilityStatus",
                table: "TheatreSeats",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SeatAvailabilityStatus",
                table: "TheatreSeats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
