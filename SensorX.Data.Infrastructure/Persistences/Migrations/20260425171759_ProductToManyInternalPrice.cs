using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorX.Data.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class ProductToManyInternalPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InternalPrices_ProductId",
                table: "InternalPrices");

            migrationBuilder.CreateIndex(
                name: "IX_InternalPrices_ProductId_CreatedAt",
                table: "InternalPrices",
                columns: new[] { "ProductId", "CreatedAt" },
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InternalPrices_ProductId_CreatedAt",
                table: "InternalPrices");

            migrationBuilder.CreateIndex(
                name: "IX_InternalPrices_ProductId",
                table: "InternalPrices",
                column: "ProductId",
                unique: true);
        }
    }
}
