import { Veiculo } from './veiculo.model';

export interface Sessao {
  id: string;
  veiculoId: string;
  veiculo: Veiculo;
  dataHoraEntrada: string;
  dataHoraSaida?: string;
  valorCobrado?: number;
  ativa: boolean;
  rowVersion: string;
  tempoPermanencia: string;
}

export interface AbrirSessaoRequest {
  veiculoId: string;
}

export interface FecharSessaoRequest {
  sessaoId: string;
  rowVersion: string;
}

export interface CalcularValorResponse {
  valor: number;
}

export interface SessaoQueryParams {
  status?: string;
  placa?: string;
  veiculoId?: string;
  dataInicio?: string;
  dataFim?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortOrder?: string;
}
