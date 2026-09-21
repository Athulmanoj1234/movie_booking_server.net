using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class adddedmovieinfoactorinfodirectorinfoandwriterinfomodelentityandaddeditsrelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActorInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DirectorInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Audiolanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubtitleLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    ImdbRating = table.Column<float>(type: "real", nullable: false),
                    AppRating = table.Column<float>(type: "real", nullable: false),
                    MovieDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassificationAge = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WriterInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WriterName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriterInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoviesInfoActors",
                columns: table => new
                {
                    MovieInfoId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesInfoActors", x => new { x.MovieInfoId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MoviesInfoActors_ActorInfos_ActorId",
                        column: x => x.ActorId,
                        principalTable: "ActorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesInfoActors_MovieInfos_MovieInfoId",
                        column: x => x.MovieInfoId,
                        principalTable: "MovieInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviesInfoDirectors",
                columns: table => new
                {
                    MovieInfoId = table.Column<int>(type: "int", nullable: false),
                    DirectorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesInfoDirectors", x => new { x.MovieInfoId, x.DirectorId });
                    table.ForeignKey(
                        name: "FK_MoviesInfoDirectors_DirectorInfos_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "DirectorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesInfoDirectors_MovieInfos_MovieInfoId",
                        column: x => x.MovieInfoId,
                        principalTable: "MovieInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoviesInfoWriters",
                columns: table => new
                {
                    MovieInfoId = table.Column<int>(type: "int", nullable: false),
                    WriterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesInfoWriters", x => new { x.MovieInfoId, x.WriterId });
                    table.ForeignKey(
                        name: "FK_MoviesInfoWriters_MovieInfos_MovieInfoId",
                        column: x => x.MovieInfoId,
                        principalTable: "MovieInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesInfoWriters_WriterInfos_WriterId",
                        column: x => x.WriterId,
                        principalTable: "WriterInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesInfoActors_ActorId",
                table: "MoviesInfoActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesInfoDirectors_DirectorId",
                table: "MoviesInfoDirectors",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesInfoWriters_WriterId",
                table: "MoviesInfoWriters",
                column: "WriterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesInfoActors");

            migrationBuilder.DropTable(
                name: "MoviesInfoDirectors");

            migrationBuilder.DropTable(
                name: "MoviesInfoWriters");

            migrationBuilder.DropTable(
                name: "ActorInfos");

            migrationBuilder.DropTable(
                name: "DirectorInfos");

            migrationBuilder.DropTable(
                name: "MovieInfos");

            migrationBuilder.DropTable(
                name: "WriterInfos");
        }
    }
}
