using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFormManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFormSchemaRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormComponents_FormVersions_FormVersionId1",
                table: "FormComponents");

            migrationBuilder.DropIndex(
                name: "IX_FormComponents_FormVersionId_ComponentKey",
                table: "FormComponents");

            migrationBuilder.DropIndex(
                name: "IX_FormComponents_FormVersionId1",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "ComponentKey",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "FormVersionId1",
                table: "FormComponents");

            migrationBuilder.RenameColumn(
                name: "Required",
                table: "FormComponents",
                newName: "IsRequired");

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "FormComponents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupKey",
                table: "FormComponents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "FormComponents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "FormComponents",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "FormComponents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "FormComponents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "FormComponents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prompt",
                table: "FormComponents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComponentOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayText = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentOptions_FormComponents_FormComponentId",
                        column: x => x.FormComponentId,
                        principalTable: "FormComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RuleName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Logic = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentRules_FormVersions_FormVersionId",
                        column: x => x.FormVersionId,
                        principalTable: "FormVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ComponentRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<string>(type: "text", nullable: false),
                    TargetComponentId = table.Column<Guid>(type: "uuid", nullable: true),
                    TargetGroupKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleActions_ComponentRules_ComponentRuleId",
                        column: x => x.ComponentRuleId,
                        principalTable: "ComponentRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RuleActions_FormComponents_TargetComponentId",
                        column: x => x.TargetComponentId,
                        principalTable: "FormComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuleConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ComponentRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Operator = table.Column<string>(type: "text", nullable: false),
                    ExpectedOptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExpectedValue = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleConditions_ComponentOptions_ExpectedOptionId",
                        column: x => x.ExpectedOptionId,
                        principalTable: "ComponentOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuleConditions_ComponentRules_ComponentRuleId",
                        column: x => x.ComponentRuleId,
                        principalTable: "ComponentRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RuleConditions_FormComponents_SourceComponentId",
                        column: x => x.SourceComponentId,
                        principalTable: "FormComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormComponents_FormVersionId",
                table: "FormComponents",
                column: "FormVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentOptions_FormComponentId",
                table: "ComponentOptions",
                column: "FormComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentRules_FormVersionId",
                table: "ComponentRules",
                column: "FormVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleActions_ComponentRuleId",
                table: "RuleActions",
                column: "ComponentRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleActions_TargetComponentId",
                table: "RuleActions",
                column: "TargetComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleConditions_ComponentRuleId",
                table: "RuleConditions",
                column: "ComponentRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleConditions_ExpectedOptionId",
                table: "RuleConditions",
                column: "ExpectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleConditions_SourceComponentId",
                table: "RuleConditions",
                column: "SourceComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuleActions");

            migrationBuilder.DropTable(
                name: "RuleConditions");

            migrationBuilder.DropTable(
                name: "ComponentOptions");

            migrationBuilder.DropTable(
                name: "ComponentRules");

            migrationBuilder.DropIndex(
                name: "IX_FormComponents_FormVersionId",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "GroupKey",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "FormComponents");

            migrationBuilder.DropColumn(
                name: "Prompt",
                table: "FormComponents");

            migrationBuilder.RenameColumn(
                name: "IsRequired",
                table: "FormComponents",
                newName: "Required");

            migrationBuilder.AddColumn<string>(
                name: "ComponentKey",
                table: "FormComponents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "FormVersionId1",
                table: "FormComponents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormComponents_FormVersionId_ComponentKey",
                table: "FormComponents",
                columns: new[] { "FormVersionId", "ComponentKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormComponents_FormVersionId1",
                table: "FormComponents",
                column: "FormVersionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FormComponents_FormVersions_FormVersionId1",
                table: "FormComponents",
                column: "FormVersionId1",
                principalTable: "FormVersions",
                principalColumn: "Id");
        }
    }
}
