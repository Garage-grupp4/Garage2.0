using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage2._0.Migrations
{
    /// <inheritdoc />
    public partial class _20260810 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_VehicleType_VehicleTypeId",
                table: "ParkedVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_ParkedVehicle_VehicleId",
                table: "ParkingSession");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_ParkingSpot_ParkingSpotId",
                table: "ParkingSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkedVehicle",
                table: "ParkedVehicle");

            migrationBuilder.RenameTable(
                name: "VehicleType",
                newName: "VehicleTypes");

            migrationBuilder.RenameTable(
                name: "ParkingSpot",
                newName: "ParkingSpots");

            migrationBuilder.RenameTable(
                name: "ParkedVehicle",
                newName: "Vehicles");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleType_Name",
                table: "VehicleTypes",
                newName: "IX_VehicleTypes_Name");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpot_Number",
                table: "ParkingSpots",
                newName: "IX_ParkingSpots_Number");

            migrationBuilder.RenameIndex(
                name: "IX_ParkedVehicle_VehicleTypeId",
                table: "Vehicles",
                newName: "IX_Vehicles_VehicleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkedVehicle_RegistrationNumber",
                table: "Vehicles",
                newName: "IX_Vehicles_RegistrationNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSession_ParkingSpots_ParkingSpotId",
                table: "ParkingSession",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSession_Vehicles_VehicleId",
                table: "ParkingSession",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_ParkingSpots_ParkingSpotId",
                table: "ParkingSession");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_Vehicles_VehicleId",
                table: "ParkingSession");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots");

            migrationBuilder.RenameTable(
                name: "VehicleTypes",
                newName: "VehicleType");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "ParkedVehicle");

            migrationBuilder.RenameTable(
                name: "ParkingSpots",
                newName: "ParkingSpot");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleTypes_Name",
                table: "VehicleType",
                newName: "IX_VehicleType_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "ParkedVehicle",
                newName: "IX_ParkedVehicle_VehicleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "ParkedVehicle",
                newName: "IX_ParkedVehicle_RegistrationNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpots_Number",
                table: "ParkingSpot",
                newName: "IX_ParkingSpot_Number");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkedVehicle",
                table: "ParkedVehicle",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_VehicleType_VehicleTypeId",
                table: "ParkedVehicle",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSession_ParkedVehicle_VehicleId",
                table: "ParkingSession",
                column: "VehicleId",
                principalTable: "ParkedVehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSession_ParkingSpot_ParkingSpotId",
                table: "ParkingSession",
                column: "ParkingSpotId",
                principalTable: "ParkingSpot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
