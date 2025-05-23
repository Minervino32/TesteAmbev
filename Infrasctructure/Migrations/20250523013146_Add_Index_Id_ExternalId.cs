using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasctructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Index_Id_ExternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ExternalId",
                table: "Orders",
                column: "ExternalId",
                unique: true,
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Id",
                table: "Orders",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Order_ExternalId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Order_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Orders");
        }
    }
}
