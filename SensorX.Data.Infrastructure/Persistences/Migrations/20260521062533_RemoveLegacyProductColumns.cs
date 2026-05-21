using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorX.Data.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyProductColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "Products" p
                SET "SupplierId" = s."Id"
                FROM "Suppliers" s
                WHERE p."SupplierId" IS NULL
                  AND p."Manufacture" IS NOT NULL
                  AND TRIM(p."Manufacture") <> ''
                  AND s."Name" = p."Manufacture";
                """);

            migrationBuilder.Sql(
                """
                UPDATE "Products" p
                SET "UnitOfQuantityId" = u."Id"
                FROM "UnitOfQuantities" u
                WHERE p."UnitOfQuantityId" IS NULL
                  AND p."Unit" IS NOT NULL
                  AND TRIM(p."Unit") <> ''
                  AND u."Name" = p."Unit";
                """);

            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM "Products"
                        WHERE "SupplierId" IS NULL OR "UnitOfQuantityId" IS NULL
                    ) THEN
                        RAISE EXCEPTION 'Cannot remove legacy product columns because some Products could not be mapped to SupplierId/UnitOfQuantityId';
                    END IF;
                END $$;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitOfQuantityId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "Manufacture",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Manufacture",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitOfQuantityId",
                table: "Products",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SupplierId",
                table: "Products",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
