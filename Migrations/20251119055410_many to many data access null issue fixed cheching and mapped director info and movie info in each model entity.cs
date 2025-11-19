        using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class manytomanydataaccessnullissuefixedchechingandmappeddirectorinfoandmovieinfoineachmodelentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectorInfoMovieInfo",
                columns: table => new
                {
                    DirectorInfoId = table.Column<int>(type: "int", nullable: false),
                    MovieInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorInfoMovieInfo", x => new { x.DirectorInfoId, x.MovieInfoId });
                    table.ForeignKey(
                        name: "FK_DirectorInfoMovieInfo_DirectorInfos_DirectorInfoId",
                        column: x => x.DirectorInfoId,
                        principalTable: "DirectorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectorInfoMovieInfo_MovieInfos_MovieInfoId",
                        column: x => x.MovieInfoId,
                        principalTable: "MovieInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectorInfoMovieInfo_MovieInfoId",
                table: "DirectorInfoMovieInfo",
                column: "MovieInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectorInfoMovieInfo");
        }
    }
}
