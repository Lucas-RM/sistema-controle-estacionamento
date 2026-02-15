using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaControleEstacionamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesForFiltering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sessoes_Ativa",
                table: "Sessoes",
                column: "Ativa");

            migrationBuilder.CreateIndex(
                name: "IX_Sessoes_DataHoraEntrada",
                table: "Sessoes",
                column: "DataHoraEntrada");

            migrationBuilder.CreateIndex(
                name: "IX_Sessoes_DataHoraSaida",
                table: "Sessoes",
                column: "DataHoraSaida");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Tipo",
                table: "Veiculos",
                column: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessoes_Ativa",
                table: "Sessoes");

            migrationBuilder.DropIndex(
                name: "IX_Sessoes_DataHoraEntrada",
                table: "Sessoes");

            migrationBuilder.DropIndex(
                name: "IX_Sessoes_DataHoraSaida",
                table: "Sessoes");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Tipo",
                table: "Veiculos");
        }
    }
}
