export interface Veiculo {
  id: string;
  placa: string;
  modelo?: string;
  cor?: string;
  tipo: TipoVeiculo;
  createdAt: string;
  updatedAt?: string;
}

export enum TipoVeiculo {
  Carro = 1,
  Moto = 2,
  Caminhonete = 3
}

export interface CreateVeiculoDto {
  placa: string;
  modelo?: string;
  cor?: string;
  tipo: TipoVeiculo;
}

export interface UpdateVeiculoDto {
  modelo?: string;
  cor?: string;
  tipo?: TipoVeiculo;
}

export interface VeiculoQueryParams {
  placa?: string;
  modelo?: string;
  cor?: string;
  tipo?: string;
  comSessaoAtiva?: boolean;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortOrder?: string;
}
