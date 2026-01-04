using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class theatreseatsstructurechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TheatreSeats_ScreenRows_ScreenRowId",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "RowId",
                table: "TheatreSeats");

            migrationBuilder.AlterColumn<int>(
                name: "ScreenRowId",
                table: "TheatreSeats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TheatreSeats_ScreenRows_ScreenRowId",
                table: "TheatreSeats",
                column: "ScreenRowId",
                principalTable: "ScreenRows",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TheatreSeats_ScreenRows_ScreenRowId",
                table: "TheatreSeats");

            migrationBuilder.AlterColumn<int>(
                name: "ScreenRowId",
                table: "TheatreSeats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowId",
                table: "TheatreSeats",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TheatreSeats_ScreenRows_ScreenRowId",
                table: "TheatreSeats",
                column: "ScreenRowId",
                principalTable: "ScreenRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
