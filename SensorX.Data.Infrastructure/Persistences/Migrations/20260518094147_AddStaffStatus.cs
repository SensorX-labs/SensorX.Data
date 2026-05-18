using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorX.Data.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Staffs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "Staffs",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Staffs");
        }
    }
}
