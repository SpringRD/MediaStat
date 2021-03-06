﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaStat.Migrations
{
    public partial class CreateInitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenName = table.Column<string>(nullable: true),
                    ProfileName = table.Column<string>(nullable: true),
                    Joined = table.Column<DateTime>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Location = table.Column<int>(nullable: true),
                    LocationDescription = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Party = table.Column<int>(nullable: true),
                    AccountType = table.Column<int>(nullable: true),
                    AccountUrl = table.Column<string>(nullable: true),
                    Classification1 = table.Column<int>(nullable: true),
                    Classification2 = table.Column<int>(nullable: true),
                    Followers = table.Column<int>(nullable: true),
                    Following = table.Column<int>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    AllLinks = table.Column<string>(nullable: true),
                    ProfileImageURL = table.Column<string>(nullable: true),
                    SpecialAccountId = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    LastChanged = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "LoginRequests",
                columns: table => new
                {
                    LoginRequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Username = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    AccessToken = table.Column<string>(maxLength: 4000, nullable: true),
                    Id = table.Column<string>(maxLength: 4000, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginRequests", x => x.LoginRequestId);
                });

            migrationBuilder.CreateTable(
                name: "LookupCodes",
                columns: table => new
                {
                    LookupCodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookupCodeDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupCodes", x => x.LookupCodeId);
                });

            migrationBuilder.CreateTable(
                name: "TweetHashtagDim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HashtagText = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetHashtagDim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountLinks",
                columns: table => new
                {
                    AccountLinkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkDescription = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    AccountInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLinks", x => x.AccountLinkId);
                    table.ForeignKey(
                        name: "FK_AccountLinks_Accounts_AccountInfoId",
                        column: x => x.AccountInfoId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LookupDescriptions",
                columns: table => new
                {
                    LookupDescriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookupDescriptionDetail = table.Column<string>(nullable: false),
                    LookupCodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupDescriptions", x => x.LookupDescriptionId);
                    table.ForeignKey(
                        name: "FK_LookupDescriptions_LookupCodes_LookupCodeId",
                        column: x => x.LookupCodeId,
                        principalTable: "LookupCodes",
                        principalColumn: "LookupCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLinks_AccountInfoId",
                table: "AccountLinks",
                column: "AccountInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupDescriptions_LookupCodeId",
                table: "LookupDescriptions",
                column: "LookupCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLinks");

            migrationBuilder.DropTable(
                name: "LoginRequests");

            migrationBuilder.DropTable(
                name: "LookupDescriptions");

            migrationBuilder.DropTable(
                name: "TweetHashtagDim");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "LookupCodes");
        }
    }
}
