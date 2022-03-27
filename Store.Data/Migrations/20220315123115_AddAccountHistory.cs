using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Store.Data.Migrations
{
    public partial class AddAccountHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "users",
                newName: "disabled");

            migrationBuilder.CreateTable(
                name: "accountHistorys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    eventType = table.Column<int>(type: "integer", nullable: false),
                    dateTimeOffset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    errorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_accountHistorys", x => x.id);
                    table.ForeignKey(
                        name: "fK_accountHistorys_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eventTypeInfo",
                columns: table => new
                {
                    typeId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_eventTypeInfo", x => x.typeId);
                });

            migrationBuilder.InsertData(
                table: "eventTypeInfo",
                columns: new[] { "typeId", "description", "name" },
                values: new object[,]
                {
                    { 0, "Successful login", "SuccessfullLogin" },
                    { 1, "Successfull logout", "SuccessfullLogout" },
                    { 2, "Failed login attempt", "LoginAttempt" },
                    { 3, "Failed logout attempt", "LogoutAttempt" },
                    { 4, "Account disabled", "Disabled" }
                });

            migrationBuilder.CreateIndex(
                name: "iX_accountHistorys_userId",
                table: "accountHistorys",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountHistorys");

            migrationBuilder.DropTable(
                name: "eventTypeInfo");

            migrationBuilder.RenameColumn(
                name: "disabled",
                table: "users",
                newName: "isActive");
        }
    }
}
