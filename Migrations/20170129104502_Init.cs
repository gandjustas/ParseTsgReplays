using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParseTsgReplays.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Replays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsFinished = table.Column<bool>(nullable: false),
                    Island = table.Column<string>(maxLength: 50, nullable: true),
                    Mission = table.Column<string>(maxLength: 50, nullable: true),
                    Server = table.Column<string>(maxLength: 2, nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnterExitEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnter = table.Column<bool>(nullable: false),
                    ReplayId = table.Column<int>(nullable: false),
                    Time = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    User = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterExitEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterExitEvents_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kills",
                columns: table => new
                {
                    KillId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ammo = table.Column<string>(maxLength: 50, nullable: true),
                    Distance = table.Column<double>(nullable: false),
                    KillerId = table.Column<int>(nullable: false),
                    KillerVehicleId = table.Column<int>(nullable: true),
                    ReplayId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    Time = table.Column<int>(nullable: false),
                    Weapon = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kills", x => x.KillId);
                    table.ForeignKey(
                        name: "FK_Kills_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    ReplayId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Class = table.Column<string>(maxLength: 50, nullable: true),
                    Damage = table.Column<double>(nullable: false),
                    Icon = table.Column<string>(maxLength: 50, nullable: true),
                    IsVehicle = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Side = table.Column<int>(nullable: false),
                    Squad = table.Column<string>(maxLength: 50, nullable: true),
                    TimeOfDeath = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    VehicleOrDriverId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => new { x.ReplayId, x.Id });
                    table.ForeignKey(
                        name: "FK_Units_Replays_ReplayId",
                        column: x => x.ReplayId,
                        principalTable: "Replays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnterExitEvents_ReplayId",
                table: "EnterExitEvents",
                column: "ReplayId");

            migrationBuilder.CreateIndex(
                name: "IX_Kills_ReplayId",
                table: "Kills",
                column: "ReplayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnterExitEvents");

            migrationBuilder.DropTable(
                name: "Kills");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Replays");
        }
    }
}
