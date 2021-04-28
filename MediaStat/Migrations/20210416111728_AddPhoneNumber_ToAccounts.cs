using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaStat.Migrations
{
    public partial class AddPhoneNumber_ToAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountPhone",
                table: "Accounts",
                type: "nvarchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountPhone",
                table: "Accounts");
        }
    }
}
