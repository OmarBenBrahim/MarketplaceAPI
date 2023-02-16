using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketplaceAPI.Migrations
{
    /// <inheritdoc />
    public partial class changecountrytostate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "AspNetUsers",
                newName: "State");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "AspNetUsers",
                newName: "Country");
        }
    }
}
