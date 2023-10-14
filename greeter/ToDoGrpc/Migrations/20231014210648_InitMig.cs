using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDoGrpc.Migrations
{
    /// <inheritdoc />
    public partial class InitMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ToDtoStatus = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ToDoItems",
                columns: new[] { "Id", "Desc", "Title", "ToDtoStatus" },
                values: new object[,]
                {
                    { 1, "This is the first ToDo item", "First ToDo", "NEW" },
                    { 2, "This is the second ToDo item", "Second ToDo", "NEW" },
                    { 3, "This is the third ToDo item", "Third ToDo", "NEW" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoItems");
        }
    }
}
