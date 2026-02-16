import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApiService } from './api.service';
import { Sessao, AbrirSessaoRequest, CalcularValorResponse, SessaoQueryParams } from '../models/sessao.model';
import { PagedResult } from '../models/pagination.model';

@Injectable()
export class SessaoService {
  constructor(private api: ApiService) {}

  listarSessoes(params?: SessaoQueryParams): Observable<PagedResult<Sessao>> {
    return this.api.get<PagedResult<Sessao>>('/movimentacao', params);
  }

  listarSessoesAtivas(params?: any): Observable<PagedResult<Sessao>> {
    const queryParams = { ...params, status: 'ativas' };
    return this.api.get<PagedResult<Sessao>>('/patio/agora', queryParams);
  }

  abrirSessao(request: AbrirSessaoRequest): Observable<Sessao> {
    return this.api.post<Sessao>('/movimentacao/entrada', request);
  }

  calcularValor(sessaoId: string): Observable<CalcularValorResponse> {
    return this.api.get<CalcularValorResponse>(`/movimentacao/calcular-valor/${sessaoId}`);
  }

  fecharSessao(sessaoId: string, rowVersion: string): Observable<Sessao> {
    return this.api.post<Sessao>('/movimentacao/saida', { sessaoId, rowVersion });
  }
}
