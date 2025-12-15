using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class migrationofrowentityadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_TheatreInfos_TheatreInfoId1",
                table: "Screens");

            migrationBuilder.DropIndex(
                name: "IX_Screens_TheatreInfoId1",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "RowValue",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "SeatType",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "TheatreInfoId1",
                table: "Screens");

            migrationBuilder.AddColumn<int>(
                name: "RowId",
                table: "TheatreSeats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScreenRowId",
                table: "TheatreSeats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SeatTicketPrice",
                table: "TheatreSeats",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "TheatreInfoId",
                table: "Screens",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ScreenRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowSeatsCount = table.Column<int>(type: "int", nullable: false),
                    ScreenId = table.Column<int>(type: "int", nullable: true),
                    TheatreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenRows_Screens_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenRows_TheatreInfos_TheatreId",
                        column: x => x.TheatreId,
                        principalTable: "TheatreInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TheatreSeats_ScreenRowId",
                table: "TheatreSeats",
                column: "ScreenRowId");

            migrationBuilder.CreateIndex(
                name: "IX_Screens_TheatreInfoId",
                table: "Screens",
                column: "TheatreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenRows_ScreenId",
                table: "ScreenRows",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenRows_TheatreId",
                table: "ScreenRows",
                column: "TheatreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_TheatreInfos_TheatreInfoId",
                table: "Screens",
                column: "TheatreInfoId",
                principalTable: "TheatreInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TheatreSeats_ScreenRows_ScreenRowId",
                table: "TheatreSeats",
                column: "ScreenRowId",
                principalTable: "ScreenRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_TheatreInfos_TheatreInfoId",
                table: "Screens");

            migrationBuilder.DropForeignKey(
                name: "FK_TheatreSeats_ScreenRows_ScreenRowId",
                table: "TheatreSeats");

            migrationBuilder.DropTable(
                name: "ScreenRows");

            migrationBuilder.DropIndex(
                name: "IX_TheatreSeats_ScreenRowId",
                table: "TheatreSeats");

            migrationBuilder.DropIndex(
                name: "IX_Screens_TheatreInfoId",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "RowId",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "ScreenRowId",
                table: "TheatreSeats");

            migrationBuilder.DropColumn(
                name: "SeatTicketPrice",
                table: "TheatreSeats");

            migrationBuilder.AddColumn<string>(
                name: "RowValue",
                table: "TheatreSeats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeatType",
                table: "TheatreSeats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TheatreInfoId",
                table: "Screens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheatreInfoId1",
                table: "Screens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Screens_TheatreInfoId1",
                table: "Screens",
                column: "TheatreInfoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_TheatreInfos_TheatreInfoId1",
                table: "Screens",
                column: "TheatreInfoId1",
                principalTable: "TheatreInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
