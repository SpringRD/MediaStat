using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaStat.Migrations
{
    public partial class AddHashtagFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Classification",
                table: "TweetHashtagDim",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TweetHashtagDim",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeClassification",
                table: "TweetHashtagDim",
                maxLength: 4000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classification",
                table: "TweetHashtagDim");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TweetHashtagDim");

            migrationBuilder.DropColumn(
                name: "FreeClassification",
                table: "TweetHashtagDim");
        }
    }
}
