using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ClientInvoicesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ClientId",
                table: "Transactions",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Clients_ClientId",
                table: "Transactions",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Clients_ClientId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ClientId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Transactions");
        }
    }
}
