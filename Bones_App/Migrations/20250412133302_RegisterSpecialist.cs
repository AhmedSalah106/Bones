using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bones_App.Migrations
{
    /// <inheritdoc />
    public partial class RegisterSpecialist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientSpecialist_Specialist_SpecialistsId",
                table: "PatientSpecialist");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialist_AspNetUsers_UserId",
                table: "Specialist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialist",
                table: "Specialist");

            migrationBuilder.RenameTable(
                name: "Specialist",
                newName: "Specialists");

            migrationBuilder.RenameIndex(
                name: "IX_Specialist_UserId",
                table: "Specialists",
                newName: "IX_Specialists_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specialists",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CertificateUrl",
                table: "Specialists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Specialists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialists",
                table: "Specialists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientSpecialist_Specialists_SpecialistsId",
                table: "PatientSpecialist",
                column: "SpecialistsId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialists_AspNetUsers_UserId",
                table: "Specialists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientSpecialist_Specialists_SpecialistsId",
                table: "PatientSpecialist");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialists_AspNetUsers_UserId",
                table: "Specialists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialists",
                table: "Specialists");

            migrationBuilder.DropColumn(
                name: "CertificateUrl",
                table: "Specialists");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Specialists");

            migrationBuilder.RenameTable(
                name: "Specialists",
                newName: "Specialist");

            migrationBuilder.RenameIndex(
                name: "IX_Specialists_UserId",
                table: "Specialist",
                newName: "IX_Specialist_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specialist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialist",
                table: "Specialist",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientSpecialist_Specialist_SpecialistsId",
                table: "PatientSpecialist",
                column: "SpecialistsId",
                principalTable: "Specialist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialist_AspNetUsers_UserId",
                table: "Specialist",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
