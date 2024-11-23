using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsImportant = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDateUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RemindUserUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserTaskId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => new { x.UserTaskId, x.Id });
                    table.ForeignKey(
                        name: "FK_Step_UserTasks_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Step");

            migrationBuilder.DropTable(
                name: "UserTasks");
        }
    }
}