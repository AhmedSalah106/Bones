using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bones_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_Specialists_SpecialistId",
                table: "Emails");

            migrationBuilder.DropIndex(
                name: "IX_Emails_SpecialistId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "SpecialistId",
                table: "Emails");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Emails");

            migrationBuilder.AddColumn<int>(
                name: "SpecialistId",
                table: "Emails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Emails_SpecialistId",
                table: "Emails",
                column: "SpecialistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Specialists_SpecialistId",
                table: "Emails",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
