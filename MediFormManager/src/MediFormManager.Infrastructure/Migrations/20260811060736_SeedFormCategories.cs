using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediFormManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedFormCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormComponents_Forms_FormId",
                table: "FormComponents");

            migrationBuilder.RenameColumn(
                name: "FormId",
                table: "FormComponents",
                newName: "FormVersionId1");

            migrationBuilder.RenameIndex(
                name: "IX_FormComponents_FormId",
                table: "FormComponents",
                newName: "IX_FormComponents_FormVersionId1");

            migrationBuilder.InsertData(
                table: "FormCategories",
                columns: new[] { "Id", "CategoryCode", "CategoryName", "CreatedAt", "CreatedBy", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "CONSENT", "Consent Form", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "", false, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "QUESTIONNAIRE", "Questionnaire", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "", false, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "ADMINISTRATIVE", "Administrative Form", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "", false, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "GUIDE", "Patient Guide", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "", false, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "OTHER", "Other", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "", false, null, null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FormComponents_FormVersions_FormVersionId1",
                table: "FormComponents",
                column: "FormVersionId1",
                principalTable: "FormVersions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormComponents_FormVersions_FormVersionId1",
                table: "FormComponents");

            migrationBuilder.DeleteData(
                table: "FormCategories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "FormCategories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "FormCategories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "FormCategories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "FormCategories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.RenameColumn(
                name: "FormVersionId1",
                table: "FormComponents",
                newName: "FormId");

            migrationBuilder.RenameIndex(
                name: "IX_FormComponents_FormVersionId1",
                table: "FormComponents",
                newName: "IX_FormComponents_FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormComponents_Forms_FormId",
                table: "FormComponents",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id");
        }
    }
}
