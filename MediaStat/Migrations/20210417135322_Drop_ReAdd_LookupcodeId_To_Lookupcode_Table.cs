using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaStat.Migrations
{
    public partial class Drop_ReAdd_LookupcodeId_To_Lookupcode_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LookupDescriptions_LookupCodes_LookupCodeId",
                table: "LookupDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LookupCodes",
                table: "LookupCodes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LookupCodes");

            migrationBuilder.AddColumn<int>(
                name: "LookupCodeId",
                table: "LookupCodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LookupCodes",
                table: "LookupCodes",
                column: "LookupCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LookupDescriptions_LookupCodes_LookupCodeId",
                table: "LookupDescriptions",
                column: "LookupCodeId",
                principalTable: "LookupCodes",
                principalColumn: "LookupCodeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LookupDescriptions_LookupCodes_LookupCodeId",
                table: "LookupDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LookupCodes",
                table: "LookupCodes");

            migrationBuilder.DropColumn(
                name: "LookupCodeId",
                table: "LookupCodes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LookupCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LookupCodes",
                table: "LookupCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LookupDescriptions_LookupCodes_LookupCodeId",
                table: "LookupDescriptions",
                column: "LookupCodeId",
                principalTable: "LookupCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
