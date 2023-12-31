using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFollowTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFollowerPlanRelation");

            migrationBuilder.DropTable(
                name: "UserFollower");

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follow_User_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Follow_User_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FollowPlanRelation",
                columns: table => new
                {
                    FollowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowPlanRelation", x => new { x.FollowId, x.PlanId });
                    table.ForeignKey(
                        name: "FK_FollowPlanRelation_Follow_FollowId",
                        column: x => x.FollowId,
                        principalTable: "Follow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowPlanRelation_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FollowedId",
                table: "Follow",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FollowerId",
                table: "Follow",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowPlanRelation_PlanId",
                table: "FollowPlanRelation",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowPlanRelation");

            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.CreateTable(
                name: "UserFollower",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollower", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollower_User_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollower_User_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFollowerPlanRelation",
                columns: table => new
                {
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowerPlanRelation", x => new { x.FollowerId, x.PlanId });
                    table.ForeignKey(
                        name: "FK_UserFollowerPlanRelation_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFollowerPlanRelation_UserFollower_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "UserFollower",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFollower_FollowedId",
                table: "UserFollower",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollower_FollowerId",
                table: "UserFollower",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowerPlanRelation_PlanId",
                table: "UserFollowerPlanRelation",
                column: "PlanId");
        }
    }
}
