using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class AddLoginIndexUniqueAndFixNamingInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Manufacturers_ManufacturerId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Role_RolesId",
                table: "RoleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Users_UsersId",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleUser",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "RoleUser",
                newName: "roleUser");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "role");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "Manufacturers",
                newName: "manufacturers");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Login",
                table: "users",
                newName: "login");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "users",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "users",
                newName: "createdOn");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "roleUser",
                newName: "usersId");

            migrationBuilder.RenameColumn(
                name: "RolesId",
                table: "roleUser",
                newName: "rolesId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UsersId",
                table: "roleUser",
                newName: "iX_roleUser_usersId");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "role",
                newName: "shortName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "role",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "ManufacturerId",
                table: "products",
                newName: "manufacturerId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "products",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "products",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ManufacturerId",
                table: "products",
                newName: "iX_products_manufacturerId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "manufacturers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "manufacturers",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "manufacturers",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "login",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_roleUser",
                table: "roleUser",
                columns: new[] { "rolesId", "usersId" });

            migrationBuilder.AddPrimaryKey(
                name: "pK_role",
                table: "role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_products",
                table: "products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_manufacturers",
                table: "manufacturers",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "iX_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fK_products_manufacturers_manufacturerId",
                table: "products",
                column: "manufacturerId",
                principalTable: "manufacturers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fK_roleUser_role_rolesId",
                table: "roleUser",
                column: "rolesId",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_roleUser_users_usersId",
                table: "roleUser",
                column: "usersId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_products_manufacturers_manufacturerId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fK_roleUser_role_rolesId",
                table: "roleUser");

            migrationBuilder.DropForeignKey(
                name: "fK_roleUser_users_usersId",
                table: "roleUser");

            migrationBuilder.DropPrimaryKey(
                name: "pK_users",
                table: "users");

            migrationBuilder.DropIndex(
                name: "iX_users_login",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pK_roleUser",
                table: "roleUser");

            migrationBuilder.DropPrimaryKey(
                name: "pK_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "pK_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "pK_manufacturers",
                table: "manufacturers");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "roleUser",
                newName: "RoleUser");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "manufacturers",
                newName: "Manufacturers");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "login",
                table: "Users",
                newName: "Login");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "createdOn",
                table: "Users",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "usersId",
                table: "RoleUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "rolesId",
                table: "RoleUser",
                newName: "RolesId");

            migrationBuilder.RenameIndex(
                name: "iX_roleUser_usersId",
                table: "RoleUser",
                newName: "IX_RoleUser_UsersId");

            migrationBuilder.RenameColumn(
                name: "shortName",
                table: "Role",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Role",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "manufacturerId",
                table: "Products",
                newName: "ManufacturerId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "iX_products_manufacturerId",
                table: "Products",
                newName: "IX_Products_ManufacturerId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Manufacturers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Manufacturers",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Manufacturers",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleUser",
                table: "RoleUser",
                columns: new[] { "RolesId", "UsersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Manufacturers_ManufacturerId",
                table: "Products",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Role_RolesId",
                table: "RoleUser",
                column: "RolesId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Users_UsersId",
                table: "RoleUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
