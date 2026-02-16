export interface FaturamentoItem {
  data: string;
  totalSessoes: number;
  valorTotal: number;
}

export interface TopVeiculoItem {
  placa: string;
  modelo: string;
  tempoTotalMinutos: number;
  quantidadeSessoes: number;
}

export interface OcupacaoItem {
  periodo: string;
  hora: number;
  quantidadeVeiculos: number;
}

export interface RelatorioFiltro {
  dataInicio?: Date;
  dataFim?: Date;
  periodo?: number;
}

export type TipoRelatorio = 'faturamento' | 'top-veiculos' | 'ocupacao-hora';

export const TipoRelatorioEnum = {
  Faturamento: 'faturamento' as TipoRelatorio,
  TopVeiculos: 'top-veiculos' as TipoRelatorio,
  Ocupacao: 'ocupacao-hora' as TipoRelatorio
};
