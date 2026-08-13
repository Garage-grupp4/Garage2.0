using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage2._0.Migrations
{
    /// <inheritdoc />
    public partial class Park : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_ParkingSpots_ParkingSpotId",
                table: "ParkingSession");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_Vehicles_VehicleId",
                table: "ParkingSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSession",
                table: "ParkingSession");

            migrationBuilder.RenameTable(
                name: "ParkingSession",
                newName: "ParkingSessions");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSession_VehicleId",
                table: "ParkingSessions",
                newName: "IX_ParkingSessions_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSession_ParkingSpotId",
                table: "ParkingSessions",
                newName: "IX_ParkingSessions_ParkingSpotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSessions",
                table: "ParkingSessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSessions_ParkingSpots_ParkingSpotId",
                table: "ParkingSessions",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSessions_Vehicles_VehicleId",
                table: "ParkingSessions",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSessions_ParkingSpots_ParkingSpotId",
                table: "ParkingSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSessions_Vehicles_VehicleId",
                table: "ParkingSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSessions",
                table: "ParkingSessions");

            migrationBuilder.RenameTable(
                name: "ParkingSessions",
                newName: "ParkingSession");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSessions_VehicleId",
                table: "ParkingSession",
                newName: "IX_ParkingSession_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSessions_ParkingSpotId",
                table: "ParkingSession",
                newName: "IX_ParkingSession_ParkingSpotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSession",
                table: "ParkingSession",
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
        }
    }
}
