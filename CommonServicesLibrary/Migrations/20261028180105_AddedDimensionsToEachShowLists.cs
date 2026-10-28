using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonServicesLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedDimensionsToEachShowLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShowDimension",
                table: "ShowsLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowDimension",
                table: "ShowsLists");
        }
    }
}
