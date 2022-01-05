using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Ver2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommitmentNumber",
                table: "Servers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<ulong>(
                name: "UserId",
                table: "Servers",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitmentNumber",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Servers");
        }
    }
}
