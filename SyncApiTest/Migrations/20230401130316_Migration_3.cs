using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SyncApiTest.Migrations
{
    /// <inheritdoc />
    public partial class Migration_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("309cd235-feb1-4059-a713-7d1d78de22b9"));

            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("5eae9102-e649-4c41-b6f6-2010e5628cea"));

            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("c3ea1c74-b3a4-4ff1-bd49-7c778197b45f"));

            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("ec4edfea-e812-48be-9463-23d25039068c"));

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("03c40583-4162-4bac-bf88-94f466da94f4"));

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("be015344-fdb6-42aa-b8f8-a0a2c9d178d7"));

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Address", "Age", "Deleted", "LastSyncedAt", "LocalDateUpdate", "Name", "ServerDateUpdated" },
                values: new object[,]
                {
                    { new Guid("18126184-f3a0-4a5d-826c-75318f3a0053"), null, 30, false, null, null, "Jane Doe", null },
                    { new Guid("27b051cc-9147-475e-ae7b-386d2386e5d7"), null, 35, false, null, null, "John Doe", null }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Age", "Breed", "Color", "Deleted", "LastSyncedAt", "LocalDateUpdate", "Name", "OwnerId", "ServerDateUpdated" },
                values: new object[,]
                {
                    { new Guid("49a83292-4f5d-41a3-b3c6-2dddf00b02f4"), 2, 3, "White", false, null, null, "Buddy", new Guid("18126184-f3a0-4a5d-826c-75318f3a0053"), null },
                    { new Guid("6d8a44eb-0a51-46d9-ac72-80ffdd39cc5a"), 9, 0, "Black", false, null, null, "Lucy", new Guid("27b051cc-9147-475e-ae7b-386d2386e5d7"), null },
                    { new Guid("b7fda981-c976-4d02-8fd2-dcf74966f269"), 3, 1, "Golden", false, null, null, "Ruby", new Guid("27b051cc-9147-475e-ae7b-386d2386e5d7"), null },
                    { new Guid("f9b51f07-182b-4760-8bd4-55661ad19fd3"), 5, 2, "Brown", false, null, null, "Max", new Guid("18126184-f3a0-4a5d-826c-75318f3a0053"), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("49a83292-4f5d-41a3-b3c6-2dddf00b02f4"));

            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("6d8a44eb-0a51-46d9-ac72-80ffdd39cc5a"));

            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("b7fda981-c976-4d02-8fd2-dcf74966f269"));

            migrationBuilder.DeleteData(
                table: "Dogs",
                keyColumn: "Id",
                keyValue: new Guid("f9b51f07-182b-4760-8bd4-55661ad19fd3"));

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("18126184-f3a0-4a5d-826c-75318f3a0053"));

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("27b051cc-9147-475e-ae7b-386d2386e5d7"));

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Address", "Age", "Deleted", "LastSyncedAt", "LocalDateUpdate", "Name", "ServerDateUpdated" },
                values: new object[,]
                {
                    { new Guid("03c40583-4162-4bac-bf88-94f466da94f4"), null, 35, false, null, null, "John Doe", null },
                    { new Guid("be015344-fdb6-42aa-b8f8-a0a2c9d178d7"), null, 30, false, null, null, "Jane Doe", null }
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Age", "Breed", "Color", "Deleted", "LastSyncedAt", "LocalDateUpdate", "Name", "OwnerId", "ServerDateUpdated" },
                values: new object[,]
                {
                    { new Guid("309cd235-feb1-4059-a713-7d1d78de22b9"), 5, 2, "Brown", false, null, null, "Max", new Guid("be015344-fdb6-42aa-b8f8-a0a2c9d178d7"), null },
                    { new Guid("5eae9102-e649-4c41-b6f6-2010e5628cea"), 2, 3, "White", false, null, null, "Buddy", new Guid("be015344-fdb6-42aa-b8f8-a0a2c9d178d7"), null },
                    { new Guid("c3ea1c74-b3a4-4ff1-bd49-7c778197b45f"), 3, 1, "Golden", false, null, null, "Ruby", new Guid("03c40583-4162-4bac-bf88-94f466da94f4"), null },
                    { new Guid("ec4edfea-e812-48be-9463-23d25039068c"), 9, 0, "Black", false, null, null, "Lucy", new Guid("03c40583-4162-4bac-bf88-94f466da94f4"), null }
                });
        }
    }
}
