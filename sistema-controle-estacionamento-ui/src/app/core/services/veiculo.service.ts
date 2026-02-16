import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApiService } from './api.service';
import { Veiculo, CreateVeiculoDto, UpdateVeiculoDto, VeiculoQueryParams } from '../models/veiculo.model';
import { PagedResult } from '../models/pagination.model';

@Injectable()
export class VeiculoService {
  constructor(private api: ApiService) {}

  listarVeiculos(params?: VeiculoQueryParams): Observable<PagedResult<Veiculo>> {
    return this.api.get<PagedResult<Veiculo>>('/veiculos', params);
  }

  buscarVeiculo(id: string): Observable<Veiculo> {
    return this.api.get<Veiculo>(`/veiculos/${id}`);
  }

  buscarPorPlaca(placa: string): Observable<Veiculo> {
    return this.api.get<Veiculo>(`/veiculos/placa/${placa}`);
  }

  criarVeiculo(veiculo: CreateVeiculoDto): Observable<Veiculo> {
    return this.api.post<Veiculo>('/veiculos', veiculo);
  }

  atualizarVeiculo(id: string, veiculo: UpdateVeiculoDto): Observable<Veiculo> {
    return this.api.put<Veiculo>(`/veiculos/${id}`, veiculo);
  }
}
