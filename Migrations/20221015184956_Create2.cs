using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class Create2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordem_Pessoa_ClienteId",
                table: "Ordem");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordem_Pessoa_ResponsavelId",
                table: "Ordem");

            migrationBuilder.RenameColumn(
                name: "ResponsavelId",
                table: "Ordem",
                newName: "ResponsavelPessoaId");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Ordem",
                newName: "ClientePessoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Ordem_ResponsavelId",
                table: "Ordem",
                newName: "IX_Ordem_ResponsavelPessoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Ordem_ClienteId",
                table: "Ordem",
                newName: "IX_Ordem_ClientePessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordem_Pessoa_ClientePessoaId",
                table: "Ordem",
                column: "ClientePessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordem_Pessoa_ResponsavelPessoaId",
                table: "Ordem",
                column: "ResponsavelPessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordem_Pessoa_ClientePessoaId",
                table: "Ordem");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordem_Pessoa_ResponsavelPessoaId",
                table: "Ordem");

            migrationBuilder.RenameColumn(
                name: "ResponsavelPessoaId",
                table: "Ordem",
                newName: "ResponsavelId");

            migrationBuilder.RenameColumn(
                name: "ClientePessoaId",
                table: "Ordem",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Ordem_ResponsavelPessoaId",
                table: "Ordem",
                newName: "IX_Ordem_ResponsavelId");

            migrationBuilder.RenameIndex(
                name: "IX_Ordem_ClientePessoaId",
                table: "Ordem",
                newName: "IX_Ordem_ClienteId");

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
    }
}
