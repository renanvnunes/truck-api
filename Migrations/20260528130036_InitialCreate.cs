using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TruckApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "checklist_templates",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    machine_type = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklist_templates", x => x.id);
                    table.ForeignKey(
                        name: "FK_checklist_templates_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "geofences",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<string>(type: "text", nullable: false),
                    polygon = table.Column<string>(type: "jsonb", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geofences", x => x.id);
                    table.ForeignKey(
                        name: "FK_geofences_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "machines",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    brand = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    plate = table.Column<string>(type: "text", nullable: true),
                    current_hourmeter = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machines", x => x.id);
                    table.ForeignKey(
                        name: "FK_machines_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    whatsapp = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    document = table.Column<string>(type: "text", nullable: true),
                    age = table.Column<int>(type: "integer", nullable: true),
                    role = table.Column<string>(type: "text", nullable: false),
                    picture = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    timezone = table.Column<string>(type: "text", nullable: false),
                    accepted_terms_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "checklist_template_items",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    template_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklist_template_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_checklist_template_items_checklist_templates_template_id",
                        column: x => x.template_id,
                        principalTable: "checklist_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "geofence_events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geofence_id = table.Column<string>(type: "text", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geofence_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_geofence_events_geofences_geofence_id",
                        column: x => x.geofence_id,
                        principalTable: "geofences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_geofence_events_machines_machine_id",
                        column: x => x.machine_id,
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "machine_locations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    recorded_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    speed = table.Column<decimal>(type: "numeric", nullable: true),
                    heading = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machine_locations", x => x.id);
                    table.ForeignKey(
                        name: "FK_machine_locations_machines_machine_id",
                        column: x => x.machine_id,
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checklists",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    operator_id = table.Column<string>(type: "text", nullable: false),
                    template_id = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklists", x => x.id);
                    table.ForeignKey(
                        name: "FK_checklists_checklist_templates_template_id",
                        column: x => x.template_id,
                        principalTable: "checklist_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_checklists_machines_machine_id",
                        column: x => x.machine_id,
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checklists_users_operator_id",
                        column: x => x.operator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "hour_records",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    operator_id = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ended_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    total_hours = table.Column<decimal>(type: "numeric", nullable: true),
                    hourmeter_start = table.Column<decimal>(type: "numeric", nullable: true),
                    hourmeter_end = table.Column<decimal>(type: "numeric", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hour_records", x => x.id);
                    table.ForeignKey(
                        name: "FK_hour_records_machines_machine_id",
                        column: x => x.machine_id,
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_hour_records_users_operator_id",
                        column: x => x.operator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "maintenance_records",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    machine_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    performed_by_id = table.Column<string>(type: "text", nullable: true),
                    performed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    hourmeter_at_service = table.Column<decimal>(type: "numeric", nullable: true),
                    next_service_hourmeter = table.Column<decimal>(type: "numeric", nullable: true),
                    cost = table.Column<decimal>(type: "numeric", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_records", x => x.id);
                    table.ForeignKey(
                        name: "FK_maintenance_records_machines_machine_id",
                        column: x => x.machine_id,
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_maintenance_records_users_performed_by_id",
                        column: x => x.performed_by_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "checklist_item_responses",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    checklist_id = table.Column<string>(type: "text", nullable: false),
                    template_item_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    observation = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklist_item_responses", x => x.id);
                    table.ForeignKey(
                        name: "FK_checklist_item_responses_checklist_template_items_template_~",
                        column: x => x.template_item_id,
                        principalTable: "checklist_template_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checklist_item_responses_checklists_checklist_id",
                        column: x => x.checklist_id,
                        principalTable: "checklists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_checklist_item_responses_checklist_id",
                table: "checklist_item_responses",
                column: "checklist_id");

            migrationBuilder.CreateIndex(
                name: "IX_checklist_item_responses_template_item_id",
                table: "checklist_item_responses",
                column: "template_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_checklist_template_items_template_id",
                table: "checklist_template_items",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "IX_checklist_templates_company_id",
                table: "checklist_templates",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "checklists_machine_date_idx",
                table: "checklists",
                columns: new[] { "machine_id", "started_at" });

            migrationBuilder.CreateIndex(
                name: "checklists_machine_id_idx",
                table: "checklists",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "checklists_operator_id_idx",
                table: "checklists",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_checklists_template_id",
                table: "checklists",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "geofence_events_geofence_id_idx",
                table: "geofence_events",
                column: "geofence_id");

            migrationBuilder.CreateIndex(
                name: "geofence_events_machine_id_idx",
                table: "geofence_events",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "geofence_events_machine_time_idx",
                table: "geofence_events",
                columns: new[] { "machine_id", "occurred_at" });

            migrationBuilder.CreateIndex(
                name: "geofences_company_id_idx",
                table: "geofences",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "hour_records_machine_date_idx",
                table: "hour_records",
                columns: new[] { "machine_id", "date" });

            migrationBuilder.CreateIndex(
                name: "hour_records_machine_id_idx",
                table: "hour_records",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "hour_records_operator_id_idx",
                table: "hour_records",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "machine_locations_machine_time_idx",
                table: "machine_locations",
                columns: new[] { "machine_id", "recorded_at" });

            migrationBuilder.CreateIndex(
                name: "IX_machines_serial_number",
                table: "machines",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "machines_company_id_idx",
                table: "machines",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "machines_status_idx",
                table: "machines",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_records_performed_by_id",
                table: "maintenance_records",
                column: "performed_by_id");

            migrationBuilder.CreateIndex(
                name: "maintenance_records_machine_id_idx",
                table: "maintenance_records",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_document",
                table: "users",
                column: "document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_whatsapp",
                table: "users",
                column: "whatsapp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_company_id_idx",
                table: "users",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "users_company_id_role_idx",
                table: "users",
                columns: new[] { "company_id", "role" });

            migrationBuilder.CreateIndex(
                name: "users_is_active_idx",
                table: "users",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "users_role_idx",
                table: "users",
                column: "role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "checklist_item_responses");

            migrationBuilder.DropTable(
                name: "geofence_events");

            migrationBuilder.DropTable(
                name: "hour_records");

            migrationBuilder.DropTable(
                name: "machine_locations");

            migrationBuilder.DropTable(
                name: "maintenance_records");

            migrationBuilder.DropTable(
                name: "checklist_template_items");

            migrationBuilder.DropTable(
                name: "checklists");

            migrationBuilder.DropTable(
                name: "geofences");

            migrationBuilder.DropTable(
                name: "checklist_templates");

            migrationBuilder.DropTable(
                name: "machines");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
