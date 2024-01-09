using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class b4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "Receipts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Receipts");
        }
    }
}
