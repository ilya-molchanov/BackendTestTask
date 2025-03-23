using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendTest.Migrations
{
    /// <inheritdoc />
    public partial class InitializeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    NodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentNodeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.NodeId);
                });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "NodeId", "Name", "ParentNodeId" },
                values: new object[,]
                {
                    { 1, "First", null },
                    { 2, "Second", 1 },
                    { 3, "Third", 1 },
                    { 4, "Fourth", 2 },
                    { 5, "Fifth", 2 },
                    { 6, "Sixth", 4 },
                    { 7, "Seventh", 6 },
                    { 8, "Eighth", 7 },
                    { 9, "Nineth", 8 },
                    { 10, "Tenth", 3 },
                    { 11, "Eleventh", 10 },
                    { 12, "Twelfth", 11 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nodes");
        }
    }
}
