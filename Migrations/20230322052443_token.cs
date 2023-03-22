using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLoginApi.Migrations
{
    public partial class token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenHash",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "TokenSalt",
                table: "clients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TokenHash",
                table: "clients",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TokenSalt",
                table: "clients",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
