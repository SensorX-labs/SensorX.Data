using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorX.Data.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShowCaseForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showcase_Body",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Showcase_Summary",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Showcase",
                table: "Products",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showcase",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Showcase_Body",
                table: "Products",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Showcase_Summary",
                table: "Products",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
