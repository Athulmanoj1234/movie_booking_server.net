using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonServicesLibrary.Migrations
{
    /// <inheritdoc />
    public partial class migrationname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "SeatAvailabilityStatus",
                table: "TheatreSeats");

            migrationBuilder.CreateTable(
                name: "ShowSeat",
                columns: table => new
                {
                    ShowListId = table.Column<int>(type: "int", nullable: false),
                    ShowSeatid = table.Column<int>(type: "int", nullable: false),
                    ShowTheatreSeatId = table.Column<int>(type: "int", nullable: false),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LockExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LockToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowSeat", x => new { x.ShowListId, x.ShowSeatid });
                    table.ForeignKey(
                        name: "FK_ShowSeat_ShowsLists_ShowListId",
                        column: x => x.ShowListId,
                        principalTable: "ShowsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShowSeat_TheatreSeats_ShowTheatreSeatId",
                        column: x => x.ShowTheatreSeatId,
                        principalTable: "TheatreSeats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeat_ShowTheatreSeatId",
                table: "ShowSeat",
                column: "ShowTheatreSeatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowSeat");

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

            migrationBuilder.AddColumn<bool>(
                name: "SeatAvailabilityStatus",
                table: "TheatreSeats",
                type: "bit",
                nullable: true);
        }
    }
}
