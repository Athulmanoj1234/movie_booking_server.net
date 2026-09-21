using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class addedmanytomanyrelationshipsbetweenseatsandshowlists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "TheatreSeats",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "TheatreSeats",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShowsListTheatreSeat",
                columns: table => new
                {
                    SeatsId = table.Column<int>(type: "int", nullable: false),
                    ShowListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowsListTheatreSeat", x => new { x.SeatsId, x.ShowListsId });
                    table.ForeignKey(
                        name: "FK_ShowsListTheatreSeat_ShowsLists_ShowListsId",
                        column: x => x.ShowListsId,
                        principalTable: "ShowsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShowsListTheatreSeat_TheatreSeats_SeatsId",
                        column: x => x.SeatsId,
                        principalTable: "TheatreSeats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowsListTheatreSeat_ShowListsId",
                table: "ShowsListTheatreSeat",
                column: "ShowListsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowsListTheatreSeat");

            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "TheatreSeats");
        }
    }
}
