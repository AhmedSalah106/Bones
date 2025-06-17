using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bones_App.Migrations
{
    /// <inheritdoc />
    public partial class addEmailInPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PaymentTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "PaymentTransactions");
        }
    }
}
