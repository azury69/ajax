using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace makingticket.Migrations
{
    /// <inheritdoc />
    public partial class addiingassignedby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedById",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AssignedById",
                table: "AspNetUsers",
                column: "AssignedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AssignedById",
                table: "AspNetUsers",
                column: "AssignedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AssignedById",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AssignedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AssignedById",
                table: "AspNetUsers");
        }
    }
}
