using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaControleEstacionamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionAndUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessoes_VeiculoId_Ativa",
                table: "Sessoes");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Sessoes",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            // SQLite suporta partial index com WHERE
            migrationBuilder.Sql(
                "CREATE UNIQUE INDEX IX_Sessoes_VeiculoId_Ativa_Unique ON Sessoes(VeiculoId) WHERE Ativa = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Sessoes_VeiculoId_Ativa_Unique;");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Sessoes");

            migrationBuilder.CreateIndex(
                name: "IX_Sessoes_VeiculoId_Ativa",
                table: "Sessoes",
                columns: new[] { "VeiculoId", "Ativa" },
                filter: "[Ativa] = 1");
        }
    }
}
