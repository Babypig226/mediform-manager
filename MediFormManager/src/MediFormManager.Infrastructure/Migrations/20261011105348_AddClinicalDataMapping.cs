using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFormManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicalDataMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataDictionaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataKey = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DataType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDictionaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentDataMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDictionaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDataMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentDataMappings_DataDictionaries_DataDictionaryId",
                        column: x => x.DataDictionaryId,
                        principalTable: "DataDictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentDataMappings_FormComponents_FormComponentId",
                        column: x => x.FormComponentId,
                        principalTable: "FormComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDataMappings_DataDictionaryId",
                table: "ComponentDataMappings",
                column: "DataDictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDataMappings_FormComponentId",
                table: "ComponentDataMappings",
                column: "FormComponentId",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_DataDictionaries_DataKey",
                table: "DataDictionaries",
                column: "DataKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentDataMappings");

            migrationBuilder.DropTable(
                name: "DataDictionaries");
        }
    }
}
