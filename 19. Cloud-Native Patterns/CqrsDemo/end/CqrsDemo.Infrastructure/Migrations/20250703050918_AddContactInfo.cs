using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CqrsDemo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContactInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "ContactEmail", "ContactPhone", "DueDate", "InvoiceDate" },
                values: new object[] { "", "", new DateTimeOffset(new DateTime(2025, 7, 23, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8596), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 6, 23, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8577), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "ContactEmail", "ContactPhone", "DueDate", "InvoiceDate" },
                values: new object[] { "", "", new DateTimeOffset(new DateTime(2025, 7, 18, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8600), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 6, 28, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8599), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "ContactEmail", "ContactPhone", "DueDate", "InvoiceDate" },
                values: new object[] { "", "", new DateTimeOffset(new DateTime(2025, 7, 13, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8604), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 7, 1, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8604), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                columns: new[] { "ContactEmail", "ContactPhone", "DueDate", "InvoiceDate" },
                values: new object[] { "", "", new DateTimeOffset(new DateTime(2025, 7, 8, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8608), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 7, 2, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8607), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                columns: new[] { "ContactEmail", "ContactPhone", "DueDate", "InvoiceDate" },
                values: new object[] { "", "", new DateTimeOffset(new DateTime(2025, 7, 5, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8612), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 7, 3, 5, 9, 18, 78, DateTimeKind.Unspecified).AddTicks(8611), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Invoices");

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DueDate", "InvoiceDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 7, 21, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6093), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 6, 21, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6067), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "DueDate", "InvoiceDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 7, 16, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6100), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 6, 26, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6099), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "DueDate", "InvoiceDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 7, 11, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6104), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 6, 29, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6103), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                columns: new[] { "DueDate", "InvoiceDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 7, 6, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6108), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 6, 30, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6107), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                columns: new[] { "DueDate", "InvoiceDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 7, 3, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6112), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 7, 1, 9, 33, 51, 160, DateTimeKind.Unspecified).AddTicks(6112), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
