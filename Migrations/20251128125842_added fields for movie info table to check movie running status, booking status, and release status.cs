using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class addedfieldsformovieinfotabletocheckmovierunningstatusbookingstatusandreleasestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBookingStarted",
                table: "MovieInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMovieCommingSoon",
                table: "MovieInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMovieCurrentlyRunning",
                table: "MovieInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBookingStarted",
                table: "MovieInfos");

            migrationBuilder.DropColumn(
                name: "IsMovieCommingSoon",
                table: "MovieInfos");

            migrationBuilder.DropColumn(
                name: "IsMovieCurrentlyRunning",
                table: "MovieInfos");
        }
    }
}
