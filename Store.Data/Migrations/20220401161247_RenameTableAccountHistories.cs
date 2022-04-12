using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Store.Data.Migrations
{
    public partial class RenameTableAccountHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "accountHistories",
                newName: "accountHistory");

            migrationBuilder.DropPrimaryKey(
                name: "pK_accountHistories",
                table: "accountHistory");

            migrationBuilder.DropForeignKey(
                name: "fK_accountHistories_users_userId",
                table: "accountHistory");

            migrationBuilder.AddPrimaryKey(
                name: "pK_accountHistory",
                table: "accountHistory",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fK_accountHistory_users_userId",
                table: "accountHistory",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "iX_accountHistories_userId",
                newName: "iX_accountHistory_userId",
                table: "accountHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "accountHistory",
                newName: "accountHistories");

            migrationBuilder.DropPrimaryKey(
                name: "pK_accountHistory",
                table: "accountHistories");

            migrationBuilder.DropForeignKey(
                name: "fK_accountHistory_users_userId",
                table: "accountHistories");

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "iX_accountHistory_userId",
                newName: "iX_accountHistories_userId",
                table: "accountHistories");
        }
    }
}
