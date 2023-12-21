using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiPlaceId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    BusinessStatus = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    UserRatingsTotal = table.Column<int>(type: "int", nullable: false),
                    Vicinity = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CompoundCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    GlobalCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level1ShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Level1LongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Level2ShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Level2LongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdministrativeArea_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    PhotoReference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlacePlaceTypeRelation",
                columns: table => new
                {
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacePlaceTypeRelation", x => new { x.PlaceId, x.PlaceTypeId });
                    table.ForeignKey(
                        name: "FK_PlacePlaceTypeRelation_PlaceType_PlaceTypeId",
                        column: x => x.PlaceTypeId,
                        principalTable: "PlaceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlacePlaceTypeRelation_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plan_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleRelation",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleRelation", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoleRelation_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleRelation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiPlaceId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    LocalityShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LocalityLongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    AdministrativeAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_AdministrativeArea_AdministrativeAreaId",
                        column: x => x.AdministrativeAreaId,
                        principalTable: "AdministrativeArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlacePlanRelation",
                columns: table => new
                {
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndingDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacePlanRelation", x => new { x.PlaceId, x.PlanId });
                    table.ForeignKey(
                        name: "FK_PlacePlanRelation_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlacePlanRelation_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaceAddressRelation",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistanceFromAddress = table.Column<double>(type: "float(8)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceAddressRelation", x => new { x.AddressId, x.PlaceId });
                    table.ForeignKey(
                        name: "FK_PlaceAddressRelation_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceAddressRelation_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportPlaceRelation",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPlaceRelation", x => new { x.ReportId, x.PlaceId });
                    table.ForeignKey(
                        name: "FK_ReportPlaceRelation_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportPlaceRelation_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportPlaceTypeRelation",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPlaceTypeRelation", x => new { x.ReportId, x.PlaceTypeId });
                    table.ForeignKey(
                        name: "FK_ReportPlaceTypeRelation_PlaceType_PlaceTypeId",
                        column: x => x.PlaceTypeId,
                        principalTable: "PlaceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportPlaceTypeRelation_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_AdministrativeAreaId",
                table: "Address",
                column: "AdministrativeAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_ApiPlaceId",
                table: "Address",
                column: "ApiPlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeArea_CountryId",
                table: "AdministrativeArea",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_PlaceId",
                table: "Photo",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Place_ApiPlaceId",
                table: "Place",
                column: "ApiPlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceAddressRelation_PlaceId",
                table: "PlaceAddressRelation",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacePlaceTypeRelation_PlaceTypeId",
                table: "PlacePlaceTypeRelation",
                column: "PlaceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacePlanRelation_PlanId",
                table: "PlacePlanRelation",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceType_ApiName",
                table: "PlaceType",
                column: "ApiName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceType_Name",
                table: "PlaceType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plan_UserId",
                table: "Plan",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_AddressId",
                table: "Report",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_UserId",
                table: "Report",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPlaceRelation_PlaceId",
                table: "ReportPlaceRelation",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPlaceTypeRelation_PlaceTypeId",
                table: "ReportPlaceTypeRelation",
                column: "PlaceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleRelation_RoleId",
                table: "UserRoleRelation",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "PlaceAddressRelation");

            migrationBuilder.DropTable(
                name: "PlacePlaceTypeRelation");

            migrationBuilder.DropTable(
                name: "PlacePlanRelation");

            migrationBuilder.DropTable(
                name: "ReportPlaceRelation");

            migrationBuilder.DropTable(
                name: "ReportPlaceTypeRelation");

            migrationBuilder.DropTable(
                name: "UserRoleRelation");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "PlaceType");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AdministrativeArea");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
