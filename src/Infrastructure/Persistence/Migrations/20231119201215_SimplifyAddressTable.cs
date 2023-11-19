using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministrativeAreaLevel1_LongName",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "AdministrativeAreaLevel1_ShortName",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "AdministrativeAreaLevel2_LongName",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "AdministrativeAreaLevel2_ShortName",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Country_LongName",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "PostalCode_ShortName",
                table: "Address",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "PostalCode_LongName",
                table: "Address",
                newName: "Locality");

            migrationBuilder.RenameColumn(
                name: "Locality_ShortName",
                table: "Address",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "Locality_LongName",
                table: "Address",
                newName: "AdministrativeAreaLevel2");

            migrationBuilder.RenameColumn(
                name: "Country_ShortName",
                table: "Address",
                newName: "AdministrativeAreaLevel1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Address",
                newName: "PostalCode_ShortName");

            migrationBuilder.RenameColumn(
                name: "Locality",
                table: "Address",
                newName: "PostalCode_LongName");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Address",
                newName: "Locality_ShortName");

            migrationBuilder.RenameColumn(
                name: "AdministrativeAreaLevel2",
                table: "Address",
                newName: "Locality_LongName");

            migrationBuilder.RenameColumn(
                name: "AdministrativeAreaLevel1",
                table: "Address",
                newName: "Country_ShortName");

            migrationBuilder.AddColumn<string>(
                name: "AdministrativeAreaLevel1_LongName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdministrativeAreaLevel1_ShortName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdministrativeAreaLevel2_LongName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdministrativeAreaLevel2_ShortName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country_LongName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
