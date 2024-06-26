﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickAdmin.DAO.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateUserId = table.Column<string>(maxLength: 100, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastModifyUserId = table.Column<string>(maxLength: 100, nullable: false),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleteUserId = table.Column<string>(maxLength: 100, nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    RequestUrl = table.Column<string>(maxLength: 1000, nullable: false),
                    RequestParam = table.Column<string>(maxLength: 4000, nullable: false),
                    ResponseParam = table.Column<string>(maxLength: 4000, nullable: false),
                    ServiceName = table.Column<string>(maxLength: 255, nullable: false),
                    ActionName = table.Column<string>(maxLength: 255, nullable: false),
                    Ip = table.Column<string>(maxLength: 255, nullable: false),
                    UserAgent = table.Column<string>(maxLength: 1000, nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    ExceptionContent = table.Column<string>(maxLength: 4000, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateUserId = table.Column<string>(maxLength: 100, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastModifyUserId = table.Column<string>(maxLength: 100, nullable: false),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleteUserId = table.Column<string>(maxLength: 100, nullable: false),
                    DeleteTime = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    EnUserName = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 255, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Phone = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
            migrationBuilder.Sql("INSERT [dbo].[User] ([Id], [CreateUserId], [CreateTime], [LastModifyUserId], [LastModifyTime], [IsDeleted], [DeleteUserId], [DeleteTime], [UserName], [EnUserName], [Password], [Email], [Phone]) VALUES (N'6aa1530e-e37b-4333-b788-e09494b50ab1', N'', CAST(N'2019-07-21T21:23:38.5314047' AS DateTime2), N'', CAST(N'2019-07-21T21:23:38.5314061' AS DateTime2), 0, N'', CAST(N'1970-01-01T00:00:00.0000000' AS DateTime2), N'admin', N'', N'vo0BIJ3kkSSI4Gc6plAvfw==', N'', N'18888888888')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
