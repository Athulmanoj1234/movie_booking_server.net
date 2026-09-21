using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class changedfrommanualtoautomaticrelationshipfrommoviedirectoractorandwriter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActorInfoMovieInfo",
                columns: table => new
                {
                    ActorInfoId = table.Column<int>(type: "int", nullable: false),
                    MovieInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorInfoMovieInfo", x => new { x.ActorInfoId, x.MovieInfoId });
                    table.ForeignKey(
                        name: "FK_ActorInfoMovieInfo_ActorInfos_ActorInfoId",
                        column: x => x.ActorInfoId,
                        principalTable: "ActorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorInfoMovieInfo_MovieInfos_MovieInfoId",
                        column: x => x.MovieInfoId,
                        principalTable: "MovieInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieInfoWriterInfo",
                columns: table => new
                {
                    MovieInfoId = table.Column<int>(type: "int", nullable: false),
                    WriterInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieInfoWriterInfo", x => new { x.MovieInfoId, x.WriterInfoId });
                    table.ForeignKey(
                        name: "FK_MovieInfoWriterInfo_MovieInfos_MovieInfoId",
                        column: x => x.MovieInfoId,
                        principalTable: "MovieInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieInfoWriterInfo_WriterInfos_WriterInfoId",
                        column: x => x.WriterInfoId,
                        principalTable: "WriterInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorInfoMovieInfo_MovieInfoId",
                table: "ActorInfoMovieInfo",
                column: "MovieInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieInfoWriterInfo_WriterInfoId",
                table: "MovieInfoWriterInfo",
                column: "WriterInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorInfoMovieInfo");

            migrationBuilder.DropTable(
                name: "MovieInfoWriterInfo");
        }
    }
}
