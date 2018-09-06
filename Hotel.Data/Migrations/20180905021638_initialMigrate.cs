using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel.Data.Migrations
{
    public partial class initialMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Roles",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 25, nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Roles", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    InstanceId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 200, nullable: false),
                    Password = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordExpires = table.Column<DateTime>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    LastLoginIp = table.Column<string>(maxLength: 20, nullable: true),
                    Locked = table.Column<bool>(nullable: false),
                    LoginAttempts = table.Column<int>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    ActivationToken = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        "FK_Users_Users_CreatedBy",
                        x => x.CreatedBy,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Users_Roles_RoleId",
                        x => x.RoleId,
                        "Roles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Users_Users_UpdatedBy",
                        x => x.UpdatedBy,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Users_CreatedBy",
                "Users",
                "CreatedBy");

            migrationBuilder.CreateIndex(
                "IX_Users_RoleId",
                "Users",
                "RoleId");

            migrationBuilder.CreateIndex(
                "IX_Users_UpdatedBy",
                "Users",
                "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Users");

            migrationBuilder.DropTable(
                "Roles");
        }
    }
}