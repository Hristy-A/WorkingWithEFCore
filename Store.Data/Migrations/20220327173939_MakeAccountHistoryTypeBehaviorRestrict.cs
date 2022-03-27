using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class MakeAccountHistoryTypeBehaviorRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_accountHistories_users_userId",
                table: "accountHistories");

            migrationBuilder.UpdateData(
                table: "eventTypeInfo",
                keyColumn: "typeId",
                keyValue: 0,
                column: "description",
                value: "Successfull login");

            migrationBuilder.AddForeignKey(
                name: "fK_accountHistories_users_userId",
                table: "accountHistories",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_accountHistories_users_userId",
                table: "accountHistories");

            migrationBuilder.UpdateData(
                table: "eventTypeInfo",
                keyColumn: "typeId",
                keyValue: 0,
                column: "description",
                value: "Successful login");

            migrationBuilder.AddForeignKey(
                name: "fK_accountHistories_users_userId",
                table: "accountHistories",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
