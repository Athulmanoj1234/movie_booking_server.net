using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class addedrowseatpriceattributeinscreenrowentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RowTicketPrice",
                table: "ScreenRows",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowTicketPrice",
                table: "ScreenRows");
        }
    }
}
