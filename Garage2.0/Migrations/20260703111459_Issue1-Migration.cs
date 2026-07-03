using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage2._0.Migrations
{
    /// <inheritdoc />
    public partial class Issue1Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "wheels",
                table: "ParkedVehicle",
                newName: "Wheels");

            migrationBuilder.RenameColumn(
                name: "vehicleType",
                table: "ParkedVehicle",
                newName: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "vehicleModel",
                table: "ParkedVehicle",
                newName: "VehicleModel");

            migrationBuilder.RenameColumn(
                name: "vehicleBrand",
                table: "ParkedVehicle",
                newName: "VehicleBrand");

            migrationBuilder.RenameColumn(
                name: "registrationNumber",
                table: "ParkedVehicle",
                newName: "RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "departureTime",
                table: "ParkedVehicle",
                newName: "DepartureTime");

            migrationBuilder.RenameColumn(
                name: "color",
                table: "ParkedVehicle",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "arrivalTime",
                table: "ParkedVehicle",
                newName: "ArrivalTime");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "ParkedVehicle",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_RegistrationNumber",
                table: "ParkedVehicle",
                column: "RegistrationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_RegistrationNumber",
                table: "ParkedVehicle");

            migrationBuilder.RenameColumn(
                name: "Wheels",
                table: "ParkedVehicle",
                newName: "wheels");

            migrationBuilder.RenameColumn(
                name: "VehicleType",
                table: "ParkedVehicle",
                newName: "vehicleType");

            migrationBuilder.RenameColumn(
                name: "VehicleModel",
                table: "ParkedVehicle",
                newName: "vehicleModel");

            migrationBuilder.RenameColumn(
                name: "VehicleBrand",
                table: "ParkedVehicle",
                newName: "vehicleBrand");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "ParkedVehicle",
                newName: "registrationNumber");

            migrationBuilder.RenameColumn(
                name: "DepartureTime",
                table: "ParkedVehicle",
                newName: "departureTime");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ParkedVehicle",
                newName: "color");

            migrationBuilder.RenameColumn(
                name: "ArrivalTime",
                table: "ParkedVehicle",
                newName: "arrivalTime");

            migrationBuilder.AlterColumn<short>(
                name: "color",
                table: "ParkedVehicle",
                type: "INTEGER",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
