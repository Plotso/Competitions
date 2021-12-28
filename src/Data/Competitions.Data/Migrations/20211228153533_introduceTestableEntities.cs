using Microsoft.EntityFrameworkCore.Migrations;

namespace Competitions.Data.Migrations
{
    public partial class introduceTestableEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Sports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTestEntity",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTestEntity",
                table: "Referees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTestEntity",
                table: "Partners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTestEntity",
                table: "Grounds",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "IsTestEntity",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "IsTestEntity",
                table: "Referees");

            migrationBuilder.DropColumn(
                name: "IsTestEntity",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "IsTestEntity",
                table: "Grounds");
        }
    }
}
