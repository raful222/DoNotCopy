using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DoNotCopy.Core.Data.Migrations
{
    public partial class updateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "ExerciseFiles");

            migrationBuilder.AddColumn<int>(
                name: "ExeciseTime",
                table: "Exercises",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SolutionTemplte",
                table: "ExerciseFiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StudentFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    ExerciseId = table.Column<int>(nullable: false),
                    FileId = table.Column<Guid>(nullable: true),
                    Alt = table.Column<string>(nullable: true),
                    Grade = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFiles_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentFiles_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4d850546-933e-4783-98f6-cab99433ed8a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "UpdatedAt" },
                values: new object[] { "fee4a906-21d7-4537-829a-e1fab9f3d4c0", new DateTime(2023, 6, 6, 11, 46, 45, 372, DateTimeKind.Local).AddTicks(9302), "AQAAAAEAACcQAAAAECw+bPHNpdtdA2DXZ++d2veuDTBRYUlw7PD+O5MMdUyUWDEElorgG7aqeTugqslLQQ==", "508c90b6-23f1-4360-80fd-ba40006d2da5", new DateTime(2023, 6, 6, 11, 46, 45, 374, DateTimeKind.Local).AddTicks(8820) });

            migrationBuilder.CreateIndex(
                name: "IX_StudentFiles_ExerciseId",
                table: "StudentFiles",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFiles_FileId",
                table: "StudentFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFiles_StudentId",
                table: "StudentFiles",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentFiles");

            migrationBuilder.DropColumn(
                name: "ExeciseTime",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "SolutionTemplte",
                table: "ExerciseFiles");

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "ExerciseFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b30c8406-e859-42e7-9818-59ea416ecd8b");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "UpdatedAt" },
                values: new object[] { "4f63f366-b0de-4848-aa4d-2848100ae8ba", new DateTime(2023, 4, 14, 0, 56, 7, 920, DateTimeKind.Local).AddTicks(223), "AQAAAAEAACcQAAAAEPnmcS06lb7/rEZuKpFNwQDZQZC1ftNR/NVpnTuS8SwpbAULXp01HClMiLg4LGVrdA==", "cb49a0a2-f805-4f6a-b590-4a68d533e5b7", new DateTime(2023, 4, 14, 0, 56, 7, 922, DateTimeKind.Local).AddTicks(840) });
        }
    }
}
