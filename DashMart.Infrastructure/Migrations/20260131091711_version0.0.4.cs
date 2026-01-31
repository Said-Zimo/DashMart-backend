using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashMart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class version004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Customers",
                newName: "Balance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Customers",
                newName: "Amount");
        }
    }
}
