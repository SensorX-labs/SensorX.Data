using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorX.Data.Infrastructure.Persistences.Migrations;

public partial class ProductUseSupplierAndUnitIds : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "SupplierId",
            table: "Products",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UnitOfQuantityId",
            table: "Products",
            type: "uuid",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Products_SupplierId",
            table: "Products",
            column: "SupplierId");

        migrationBuilder.CreateIndex(
            name: "IX_Products_UnitOfQuantityId",
            table: "Products",
            column: "UnitOfQuantityId");

        migrationBuilder.AddForeignKey(
            name: "FK_Products_Suppliers_SupplierId",
            table: "Products",
            column: "SupplierId",
            principalTable: "Suppliers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Products_UnitOfQuantities_UnitOfQuantityId",
            table: "Products",
            column: "UnitOfQuantityId",
            principalTable: "UnitOfQuantities",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Products_Suppliers_SupplierId",
            table: "Products");

        migrationBuilder.DropForeignKey(
            name: "FK_Products_UnitOfQuantities_UnitOfQuantityId",
            table: "Products");

        migrationBuilder.DropIndex(
            name: "IX_Products_SupplierId",
            table: "Products");

        migrationBuilder.DropIndex(
            name: "IX_Products_UnitOfQuantityId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "SupplierId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "UnitOfQuantityId",
            table: "Products");
    }
}
