using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SplittedAdministativeArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_AdministrativeArea_AdministrativeAreaId",
                table: "Address");

            migrationBuilder.DropTable(
                name: "AdministrativeArea");

            migrationBuilder.RenameColumn(
                name: "AdministrativeAreaId",
                table: "Address",
                newName: "AdministrativeAreaLevel2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Address_AdministrativeAreaId",
                table: "Address",
                newName: "IX_Address_AdministrativeAreaLevel2Id");

            migrationBuilder.CreateTable(
                name: "AdministrativeAreaLevel1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeAreaLevel1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdministrativeAreaLevel1_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeAreaLevel2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AdministrativeAreaLevel1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeAreaLevel2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdministrativeAreaLevel2_AdministrativeAreaLevel1_AdministrativeAreaLevel1Id",
                        column: x => x.AdministrativeAreaLevel1Id,
                        principalTable: "AdministrativeAreaLevel1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeAreaLevel1_CountryId",
                table: "AdministrativeAreaLevel1",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeAreaLevel2_AdministrativeAreaLevel1Id",
                table: "AdministrativeAreaLevel2",
                column: "AdministrativeAreaLevel1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_AdministrativeAreaLevel2_AdministrativeAreaLevel2Id",
                table: "Address",
                column: "AdministrativeAreaLevel2Id",
                principalTable: "AdministrativeAreaLevel2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_AdministrativeAreaLevel2_AdministrativeAreaLevel2Id",
                table: "Address");

            migrationBuilder.DropTable(
                name: "AdministrativeAreaLevel2");

            migrationBuilder.DropTable(
                name: "AdministrativeAreaLevel1");

            migrationBuilder.RenameColumn(
                name: "AdministrativeAreaLevel2Id",
                table: "Address",
                newName: "AdministrativeAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_AdministrativeAreaLevel2Id",
                table: "Address",
                newName: "IX_Address_AdministrativeAreaId");

            migrationBuilder.CreateTable(
                name: "AdministrativeArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level1LongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Level1ShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Level2LongName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Level2ShortName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeArea_CountryId",
                table: "AdministrativeArea",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_AdministrativeArea_AdministrativeAreaId",
                table: "Address",
                column: "AdministrativeAreaId",
                principalTable: "AdministrativeArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
