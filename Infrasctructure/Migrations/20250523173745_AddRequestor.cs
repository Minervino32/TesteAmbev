using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasctructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Requestor",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Requestor",
                table: "Orders");
        }
    }
}
