using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class theatreandrelatedentityadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WriterName",
                table: "WriterInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DirectorName",
                table: "DirectorInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ActorName",
                table: "ActorInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "TheatreLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    TheatreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheatreLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TheatreInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheatreTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheatreLocationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheatreInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TheatreInfos_TheatreLocations_TheatreLocationId",
                        column: x => x.TheatreLocationId,
                        principalTable: "TheatreLocations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Screens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenCapacity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheatreInfoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheatreInfoId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screens_TheatreInfos_TheatreInfoId1",
                        column: x => x.TheatreInfoId1,
                        principalTable: "TheatreInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "showsLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ShowTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Screenid = table.Column<int>(type: "int", nullable: true),
                    MovieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_showsLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_showsLists_MovieInfos_MovieId",
                        column: x => x.MovieId,
                        principalTable: "MovieInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_showsLists_Screens_Screenid",
                        column: x => x.Screenid,
                        principalTable: "Screens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TheatreSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatAvailabilityStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheatreSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TheatreSeats_Screens_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screens_TheatreInfoId1",
                table: "Screens",
                column: "TheatreInfoId1");

            migrationBuilder.CreateIndex(
                name: "IX_showsLists_MovieId",
                table: "showsLists",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_showsLists_Screenid",
                table: "showsLists",
                column: "Screenid");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreInfos_TheatreLocationId",
                table: "TheatreInfos",
                column: "TheatreLocationId",
                unique: true,
                filter: "[TheatreLocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TheatreSeats_ScreenId",
                table: "TheatreSeats",
                column: "ScreenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "showsLists");

            migrationBuilder.DropTable(
                name: "TheatreSeats");

            migrationBuilder.DropTable(
                name: "Screens");

            migrationBuilder.DropTable(
                name: "TheatreInfos");

            migrationBuilder.DropTable(
                name: "TheatreLocations");

            migrationBuilder.AlterColumn<string>(
                name: "WriterName",
                table: "WriterInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DirectorName",
                table: "DirectorInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActorName",
                table: "ActorInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
