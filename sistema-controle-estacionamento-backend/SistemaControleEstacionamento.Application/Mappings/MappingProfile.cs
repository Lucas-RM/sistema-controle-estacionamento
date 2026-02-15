using AutoMapper;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;
using SistemaControleEstacionamento.Domain.Entities;

namespace SistemaControleEstacionamento.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Veiculo
        CreateMap<Veiculo, VeiculoDto>();
        CreateMap<CreateVeiculoDto, Veiculo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => NormalizarPlaca(src.Placa)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Sessoes, opt => opt.Ignore());

        // Sessao
        CreateMap<Sessao, SessaoDto>()
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.TempoPermanencia, opt => opt.MapFrom(src =>
                src.DataHoraSaida.HasValue
                    ? src.DataHoraSaida.Value - src.DataHoraEntrada
                    : (TimeSpan?)(DateTimeOffset.UtcNow - src.DataHoraEntrada)));
    }

    private string NormalizarPlaca(string placa)
    {
        return placa.Trim().ToUpper().Replace("-", "").Replace(" ", "");
    }
}

