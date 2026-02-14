using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Domain.Entities;

public class Veiculo
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Modelo { get; set; }
    public string? Cor { get; set; }
    public TipoVeiculo Tipo { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // Navegação
    public virtual ICollection<Sessao> Sessoes { get; set; } = new List<Sessao>();
}

