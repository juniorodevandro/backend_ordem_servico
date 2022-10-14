using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class TentarAtualizarOrdem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClienteId",
                table: "Ordem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsavelId",
                table: "Ordem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ordem_ClienteId",
                table: "Ordem",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordem_ResponsavelId",
                table: "Ordem",
                column: "ResponsavelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordem_Pessoa_ClienteId",
                table: "Ordem",
                column: "ClienteId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordem_Pessoa_ResponsavelId",
                table: "Ordem",
                column: "ResponsavelId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordem_Pessoa_ClienteId",
                table: "Ordem");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordem_Pessoa_ResponsavelId",
                table: "Ordem");

            migrationBuilder.DropIndex(
                name: "IX_Ordem_ClienteId",
                table: "Ordem");

            migrationBuilder.DropIndex(
                name: "IX_Ordem_ResponsavelId",
                table: "Ordem");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Ordem");

            migrationBuilder.DropColumn(
                name: "ResponsavelId",
                table: "Ordem");
        }
    }
}
