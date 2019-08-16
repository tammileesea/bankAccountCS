using Microsoft.EntityFrameworkCore.Migrations;

namespace bankAccount.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_users_UserId",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_UserId",
                table: "transactions",
                newName: "IX_transactions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_users_UserId",
                table: "transactions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_users_UserId",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_UserId",
                table: "Transaction",
                newName: "IX_Transaction_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_users_UserId",
                table: "Transaction",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
