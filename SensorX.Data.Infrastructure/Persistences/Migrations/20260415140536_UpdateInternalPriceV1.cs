using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorX.Data.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInternalPriceV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InternalPrices_ProductId",
                table: "InternalPrices",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InternalPrices_Products_ProductId",
                table: "InternalPrices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalPrices_Products_ProductId",
                table: "InternalPrices");

            migrationBuilder.DropIndex(
                name: "IX_InternalPrices_ProductId",
                table: "InternalPrices");
        }
    }
}
