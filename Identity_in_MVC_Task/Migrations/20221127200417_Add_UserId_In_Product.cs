using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_in_MVC_Task.Migrations
{
    public partial class Add_UserId_In_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Product",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Product");
        }
    }
}
