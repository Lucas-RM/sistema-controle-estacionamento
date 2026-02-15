using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaControleEstacionamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRowVersionForSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Recriar tabela Sessoes com RowVersion como TEXT
            migrationBuilder.Sql(@"
                CREATE TABLE Sessoes_New (
                    Id TEXT NOT NULL PRIMARY KEY,
                    VeiculoId TEXT NOT NULL,
                    DataHoraEntrada TEXT NOT NULL,
                    DataHoraSaida TEXT,
                    ValorCobrado TEXT,
                    Ativa INTEGER NOT NULL,
                    RowVersion TEXT NOT NULL DEFAULT '',
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT,
                    FOREIGN KEY (VeiculoId) REFERENCES Veiculos(Id) ON DELETE RESTRICT
                );
            ");

            // Copiar dados existentes gerando novo RowVersion
            migrationBuilder.Sql(@"
                INSERT INTO Sessoes_New (Id, VeiculoId, DataHoraEntrada, DataHoraSaida, ValorCobrado, Ativa, RowVersion, CreatedAt, UpdatedAt)
                SELECT Id, VeiculoId, DataHoraEntrada, DataHoraSaida, ValorCobrado, Ativa, 
                       lower(hex(randomblob(4)) || '-' || hex(randomblob(2)) || '-' || hex(randomblob(2)) || '-' || hex(randomblob(2)) || '-' || hex(randomblob(6))),
                       CreatedAt, UpdatedAt
                FROM Sessoes;
            ");

            // Remover tabela antiga
            migrationBuilder.Sql("DROP TABLE Sessoes;");

            // Renomear nova tabela
            migrationBuilder.Sql("ALTER TABLE Sessoes_New RENAME TO Sessoes;");

            // Recriar índices
            migrationBuilder.Sql("CREATE UNIQUE INDEX IX_Sessoes_VeiculoId_Ativa_Unique ON Sessoes(VeiculoId) WHERE Ativa = 1;");
            migrationBuilder.Sql("CREATE INDEX IX_Sessoes_Ativa ON Sessoes(Ativa);");
            migrationBuilder.Sql("CREATE INDEX IX_Sessoes_DataHoraEntrada ON Sessoes(DataHoraEntrada);");
            migrationBuilder.Sql("CREATE INDEX IX_Sessoes_DataHoraSaida ON Sessoes(DataHoraSaida);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "Sessoes",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 36);
        }
    }
}
