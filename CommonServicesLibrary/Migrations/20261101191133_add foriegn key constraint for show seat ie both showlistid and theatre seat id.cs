using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonServicesLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addforiegnkeyconstraintforshowseatiebothshowlistidandtheatreseatid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowTheatreSeatId",
                table: "ShowSeat");

            migrationBuilder.DropIndex(
                name: "IX_ShowSeat_ShowTheatreSeatId",
                table: "ShowSeat");

            migrationBuilder.DropColumn(
                name: "ShowTheatreSeatId",
                table: "ShowSeat");

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeat_ShowSeatid",
                table: "ShowSeat",
                column: "ShowSeatid");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowSeatid",
                table: "ShowSeat",
                column: "ShowSeatid",
                principalTable: "TheatreSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowSeatid",
                table: "ShowSeat");

            migrationBuilder.DropIndex(
                name: "IX_ShowSeat_ShowSeatid",
                table: "ShowSeat");

            migrationBuilder.AddColumn<int>(
                name: "ShowTheatreSeatId",
                table: "ShowSeat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeat_ShowTheatreSeatId",
                table: "ShowSeat",
                column: "ShowTheatreSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_TheatreSeats_ShowTheatreSeatId",
                table: "ShowSeat",
                column: "ShowTheatreSeatId",
                principalTable: "TheatreSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
