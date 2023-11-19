using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PlacesAddressRelationChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Place_Address_AddressId",
                table: "Place");

            migrationBuilder.DropIndex(
                name: "IX_Place_AddressId",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "DistanceFromAddress",
                table: "Place");

            migrationBuilder.CreateTable(
                name: "PlaceAddress",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistanceFromAddress = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceAddress", x => new { x.AddressId, x.PlaceId });
                    table.ForeignKey(
                        name: "FK_PlaceAddress_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceAddress_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceAddress_PlaceId",
                table: "PlaceAddress",
                column: "PlaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceAddress");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Place",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "DistanceFromAddress",
                table: "Place",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Place_AddressId",
                table: "Place",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Address_AddressId",
                table: "Place",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }
    }
}
