using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class RenameAccountHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_accountHistorys_users_userId",
                table: "accountHistorys");

            migrationBuilder.DropPrimaryKey(
                name: "pK_accountHistorys",
                table: "accountHistorys");

            migrationBuilder.RenameTable(
                name: "accountHistorys",
                newName: "accountHistories");

            migrationBuilder.RenameIndex(
                name: "iX_accountHistorys_userId",
                table: "accountHistories",
                newName: "iX_accountHistories_userId");

            migrationBuilder.AddPrimaryKey(
                name: "pK_accountHistories",
                table: "accountHistories",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fK_accountHistories_users_userId",
                table: "accountHistories",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_accountHistories_users_userId",
                table: "accountHistories");

            migrationBuilder.DropPrimaryKey(
                name: "pK_accountHistories",
                table: "accountHistories");

            migrationBuilder.RenameTable(
                name: "accountHistories",
                newName: "accountHistorys");

            migrationBuilder.RenameIndex(
                name: "iX_accountHistories_userId",
                table: "accountHistorys",
                newName: "iX_accountHistorys_userId");

            migrationBuilder.AddPrimaryKey(
                name: "pK_accountHistorys",
                table: "accountHistorys",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fK_accountHistorys_users_userId",
                table: "accountHistorys",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
