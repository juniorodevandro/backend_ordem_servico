using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class att : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCodigo",
                table: "OrdemServico");

            migrationBuilder.DropColumn(
                name: "OrdemCodigo",
                table: "OrdemServico");

            migrationBuilder.DropColumn(
                name: "ItemCodigo",
                table: "OrdemItem");

            migrationBuilder.DropColumn(
                name: "OrdemCodigo",
                table: "OrdemItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemCodigo",
                table: "OrdemServico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrdemCodigo",
                table: "OrdemServico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemCodigo",
                table: "OrdemItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrdemCodigo",
                table: "OrdemItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
