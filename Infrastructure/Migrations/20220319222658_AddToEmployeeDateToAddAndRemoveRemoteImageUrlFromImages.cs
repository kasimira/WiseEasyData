using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddToEmployeeDateToAddAndRemoveRemoteImageUrlFromImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemoteImageUrl",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "DataToAdd",
                table: "Images",
                newName: "DateToAdd");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateToAdd",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateToAdd",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "DateToAdd",
                table: "Images",
                newName: "DataToAdd");

            migrationBuilder.AddColumn<string>(
                name: "RemoteImageUrl",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
