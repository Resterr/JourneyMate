using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PlaceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaceId",
                table: "Address",
                newName: "ApiPlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_PlaceId",
                table: "Address",
                newName: "IX_Address_ApiPlaceId");

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiPlaceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    UserRatingsTotal = table.Column<int>(type: "int", nullable: false),
                    Vicinity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistanceFromAddress = table.Column<double>(type: "float", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: false),
                    Location_Longitude = table.Column<double>(type: "float", nullable: false),
                    PlusCode_CompoundCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlusCode_GlobalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photo_Height = table.Column<int>(type: "int", nullable: true),
                    Photo_Width = table.Column<int>(type: "int", nullable: true),
                    Photo_PhotoReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Place_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlaceType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceTypeRelation",
                columns: table => new
                {
                    PlacesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceTypeRelation", x => new { x.PlacesId, x.TypesId });
                    table.ForeignKey(
                        name: "FK_PlaceTypeRelation_PlaceType_TypesId",
                        column: x => x.TypesId,
                        principalTable: "PlaceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceTypeRelation_Place_PlacesId",
                        column: x => x.PlacesId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Place_AddressId",
                table: "Place",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Place_ApiPlaceId",
                table: "Place",
                column: "ApiPlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceType_Name",
                table: "PlaceType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceTypeRelation_TypesId",
                table: "PlaceTypeRelation",
                column: "TypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceTypeRelation");

            migrationBuilder.DropTable(
                name: "PlaceType");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                table: "Role");

            migrationBuilder.RenameColumn(
                name: "ApiPlaceId",
                table: "Address",
                newName: "PlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_ApiPlaceId",
                table: "Address",
                newName: "IX_Address_PlaceId");
        }
    }
}
