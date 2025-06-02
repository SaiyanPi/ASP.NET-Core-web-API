using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutputCachingDemo.Migrations
{
    /// <inheritdoc />
    public partial class TenantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Domain", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "tenant1.example.com", "Tenant 1" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "tenant2.example.com", "Tenant 2" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "tenant3.example.com", "Tenant 3" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "tenant4.example.com", "Tenant 4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
