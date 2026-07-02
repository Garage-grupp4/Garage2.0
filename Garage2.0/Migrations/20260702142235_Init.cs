using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage2._0.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkedVehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    registrationNumber = table.Column<string>(type: "TEXT", nullable: false),
                    arrivalTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    vehicleType = table.Column<int>(type: "INTEGER", nullable: false),
                    vehicleModel = table.Column<string>(type: "TEXT", nullable: false),
                    vehicleBrand = table.Column<string>(type: "TEXT", nullable: false),
                    departureTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    color = table.Column<short>(type: "INTEGER", nullable: false),
                    wheels = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicle", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicle");
        }
    }
}
