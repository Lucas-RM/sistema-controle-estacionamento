import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApiService } from './api.service';
import { FaturamentoItem, TopVeiculoItem, OcupacaoItem } from '../models/relatorio.model';

@Injectable()
export class RelatorioService {
  constructor(private api: ApiService) {}

  faturamento(periodo?: number): Observable<FaturamentoItem[]> {
    const params = periodo ? { periodo } : {};
    return this.api.get<FaturamentoItem[]>('/relatorios/faturamento', params);
  }

  topVeiculos(dataInicio?: string, dataFim?: string): Observable<TopVeiculoItem[]> {
    const params: any = {};
    if (dataInicio) params.dataInicio = dataInicio;
    if (dataFim) params.dataFim = dataFim;
    return this.api.get<TopVeiculoItem[]>('/relatorios/top-veiculos', params);
  }

  ocupacaoPorHora(dataInicio: string, dataFim: string): Observable<OcupacaoItem[]> {
    return this.api.get<OcupacaoItem[]>('/relatorios/ocupacao-hora', { dataInicio, dataFim });
  }
}
