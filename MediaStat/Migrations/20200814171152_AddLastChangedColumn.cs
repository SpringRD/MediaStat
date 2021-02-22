using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaStat.Migrations
{
    public partial class AddLastChangedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChanged",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChanged",
                table: "Accounts");
        }
    }
}
