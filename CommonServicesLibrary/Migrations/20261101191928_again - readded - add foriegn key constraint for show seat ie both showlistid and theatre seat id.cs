using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonServicesLibrary.Migrations
{
    /// <inheritdoc />
    public partial class againreaddedaddforiegnkeyconstraintforshowseatiebothshowlistidandtheatreseatid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowSeatid",
                table: "ShowSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowSeatid",
                table: "ShowSeat");

            migrationBuilder.RenameColumn(
                name: "ShowSeatid",
                table: "ShowSeat",
                newName: "ShowTheatreSeatId");

            migrationBuilder.RenameIndex(
                name: "IX_ShowSeat_ShowSeatid",
                table: "ShowSeat",
                newName: "IX_ShowSeat_ShowTheatreSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowListId",
                table: "ShowSeat",
                column: "ShowListId",
                principalTable: "ShowsLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowTheatreSeatId",
                table: "ShowSeat",
                column: "ShowTheatreSeatId",
                principalTable: "TheatreSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowListId",
                table: "ShowSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowTheatreSeatId",
                table: "ShowSeat");

            migrationBuilder.RenameColumn(
                name: "ShowTheatreSeatId",
                table: "ShowSeat",
                newName: "ShowSeatid");

            migrationBuilder.RenameIndex(
                name: "IX_ShowSeat_ShowTheatreSeatId",
                table: "ShowSeat",
                newName: "IX_ShowSeat_ShowSeatid");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowSeatid",
                table: "ShowSeat",
                column: "ShowSeatid",
                principalTable: "ShowsLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowSeatid",
                table: "ShowSeat",
                column: "ShowSeatid",
                principalTable: "TheatreSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
