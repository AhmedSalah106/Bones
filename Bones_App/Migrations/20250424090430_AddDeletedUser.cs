using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bones_App.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeletedUser",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeletedUser",
                table: "Patients");
        }
    }
}
