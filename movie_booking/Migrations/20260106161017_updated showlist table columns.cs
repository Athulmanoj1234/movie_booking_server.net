using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_booking.Migrations
{
    /// <inheritdoc />
    public partial class updatedshowlisttablecolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_showsLists_MovieInfos_MovieId",
                table: "showsLists");

            migrationBuilder.DropForeignKey(
                name: "FK_showsLists_Screens_Screenid",
                table: "showsLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_showsLists",
                table: "showsLists");

            migrationBuilder.RenameTable(
                name: "showsLists",
                newName: "ShowsLists");

            migrationBuilder.RenameColumn(
                name: "Screenid",
                table: "ShowsLists",
                newName: "ScreenId");

            migrationBuilder.RenameColumn(
                name: "ShowTime",
                table: "ShowsLists",
                newName: "ShowStart");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "ShowsLists",
                newName: "MovieInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_showsLists_Screenid",
                table: "ShowsLists",
                newName: "IX_ShowsLists_ScreenId");

            migrationBuilder.RenameIndex(
                name: "IX_showsLists_MovieId",
                table: "ShowsLists",
                newName: "IX_ShowsLists_MovieInfoId");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ShowEnd",
                table: "ShowsLists",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowsLists",
                table: "ShowsLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowsLists_MovieInfos_MovieInfoId",
                table: "ShowsLists",
                column: "MovieInfoId",
                principalTable: "MovieInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowsLists_Screens_ScreenId",
                table: "ShowsLists",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowsLists_MovieInfos_MovieInfoId",
                table: "ShowsLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShowsLists_Screens_ScreenId",
                table: "ShowsLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowsLists",
                table: "ShowsLists");

            migrationBuilder.DropColumn(
                name: "ShowEnd",
                table: "ShowsLists");

            migrationBuilder.RenameTable(
                name: "ShowsLists",
                newName: "showsLists");

            migrationBuilder.RenameColumn(
                name: "ScreenId",
                table: "showsLists",
                newName: "Screenid");

            migrationBuilder.RenameColumn(
                name: "ShowStart",
                table: "showsLists",
                newName: "ShowTime");

            migrationBuilder.RenameColumn(
                name: "MovieInfoId",
                table: "showsLists",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_ShowsLists_ScreenId",
                table: "showsLists",
                newName: "IX_showsLists_Screenid");

            migrationBuilder.RenameIndex(
                name: "IX_ShowsLists_MovieInfoId",
                table: "showsLists",
                newName: "IX_showsLists_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_showsLists",
                table: "showsLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_showsLists_MovieInfos_MovieId",
                table: "showsLists",
                column: "MovieId",
                principalTable: "MovieInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_showsLists_Screens_Screenid",
                table: "showsLists",
                column: "Screenid",
                principalTable: "Screens",
                principalColumn: "Id");
        }
    }
}
