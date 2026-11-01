using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonServicesLibrary.Migrations
{
    /// <inheritdoc />
    public partial class readdedaddforiegnkeyconstraintforshowseatiebothshowlistidandtheatreseatid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowListId",
                table: "ShowSeat");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowSeatid",
                table: "ShowSeat",
                column: "ShowSeatid",
                principalTable: "ShowsLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowSeatid",
                table: "ShowSeat");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeat_ShowsLists_ShowListId",
                table: "ShowSeat",
                column: "ShowListId",
                principalTable: "ShowsLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
