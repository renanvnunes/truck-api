using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCompanyIdCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_users_companies_company_id", table: "users");

            migrationBuilder.AddCheckConstraint(
                name: "ck_users_company_id_by_role",
                table: "users",
                sql: "(role = 'Admin' AND company_id IS NULL) OR (role <> 'Admin' AND company_id IS NOT NULL)"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_users_companies_company_id",
                table: "users",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_users_companies_company_id", table: "users");

            migrationBuilder.DropCheckConstraint(
                name: "ck_users_company_id_by_role",
                table: "users"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_users_companies_company_id",
                table: "users",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull
            );
        }
    }
}
