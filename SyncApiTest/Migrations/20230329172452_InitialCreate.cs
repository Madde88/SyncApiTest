using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SyncApiTest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: true),
                    LocalDateUpdate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ServerDateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: true),
                    Breed = table.Column<int>(type: "INTEGER", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LocalDateUpdate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ServerDateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dogs_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Address", "Age", "Deleted", "LastSyncedAt", "LocalDateUpdate", "Name", "ServerDateUpdated" },
                values: new object[,]
                {
                    { new Guid("288bf7c6-163a-4fd4-a8a3-1e8b7ebf089c"), null, 35, false, null, null, "John Doe", null },
                    { new Guid("2ebbad77-97e1-4a8c-8684-e0fa3ae6d2df"), null, 30, false, null, null, "Jane Doe", null }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Age", "Breed", "Color", "Deleted", "LastSyncedAt", "LocalDateUpdate", "Name", "OwnerId", "ServerDateUpdated" },
                values: new object[,]
                {
                    { new Guid("416c7350-d994-47ae-aa9d-a9a6eb04bd8a"), 5, 2, "Brown", false, null, null, "Max", new Guid("2ebbad77-97e1-4a8c-8684-e0fa3ae6d2df"), null },
                    { new Guid("517a542a-e59f-49f8-b75a-43ebed7e9e79"), 3, 1, "Golden", false, null, null, "Ruby", new Guid("288bf7c6-163a-4fd4-a8a3-1e8b7ebf089c"), null },
                    { new Guid("761bc29f-eb54-4cb5-a437-470898c17738"), 2, 3, "White", false, null, null, "Buddy", new Guid("2ebbad77-97e1-4a8c-8684-e0fa3ae6d2df"), null },
                    { new Guid("90b7b98c-505b-48b4-abcc-506d47fbbe61"), 9, 0, "Black", false, null, null, "Lucy", new Guid("288bf7c6-163a-4fd4-a8a3-1e8b7ebf089c"), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_OwnerId",
                table: "Dogs",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dogs");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
