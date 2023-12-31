using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedScheduleStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlacePlanRelation",
                table: "PlacePlanRelation");

            migrationBuilder.DropIndex(
                name: "IX_PlacePlanRelation_PlanId",
                table: "PlacePlanRelation");

            migrationBuilder.DropColumn(
                name: "EndingDate",
                table: "PlacePlanRelation");

            migrationBuilder.DropColumn(
                name: "StartingDate",
                table: "PlacePlanRelation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlacePlanRelation",
                table: "PlacePlanRelation",
                columns: new[] { "PlanId", "PlaceId" });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedule_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlacePlanRelation_PlaceId",
                table: "PlacePlanRelation",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_PlaceId",
                table: "Schedule",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_PlanId",
                table: "Schedule",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlacePlanRelation",
                table: "PlacePlanRelation");

            migrationBuilder.DropIndex(
                name: "IX_PlacePlanRelation_PlaceId",
                table: "PlacePlanRelation");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndingDate",
                table: "PlacePlanRelation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingDate",
                table: "PlacePlanRelation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlacePlanRelation",
                table: "PlacePlanRelation",
                columns: new[] { "PlaceId", "PlanId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlacePlanRelation_PlanId",
                table: "PlacePlanRelation",
                column: "PlanId");
        }
    }
}
