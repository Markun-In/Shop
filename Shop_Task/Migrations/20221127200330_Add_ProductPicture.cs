using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop_Task.Migrations
{
    public partial class Add_ProductPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProductPicture",
                table: "Product",
                type: "longblob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPicture",
                table: "Product");
        }
    }
}
