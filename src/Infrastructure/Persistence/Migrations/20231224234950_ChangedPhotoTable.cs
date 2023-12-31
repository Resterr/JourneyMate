using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPhotoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Width",
                table: "Photo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "Photo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Photo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Photo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Photo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Photo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Address",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Photo");

            migrationBuilder.AlterColumn<int>(
                name: "Width",
                table: "Photo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "Photo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Address",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);
        }
    }
}
