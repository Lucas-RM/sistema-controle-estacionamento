import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import { SessaoService } from '../../core/services/sessao.service';
import { Sessao } from '../../core/models/sessao.model';
import { PagedResult } from '../../core/models/pagination.model';

@Component({
  selector: 'app-patio-agora',
  templateUrl: './patio-agora.component.html',
  styleUrls: ['./patio-agora.component.css']
})
export class PatioAgoraComponent implements OnInit, OnDestroy {
  sessoes: Sessao[] = [];
  totalRecords: number = 0;
  loading: boolean = false;
  placa: string = '';
  
  sessaoSelecionada: Sessao | null = null;
  mostrarModalSaida: boolean = false;
  valorCalculado: number = 0;
  
  private intervalSubscription: any;

  constructor(private sessaoService: SessaoService) {}

  ngOnInit() {
    this.carregarSessoes();
    
    // Atualiza a cada 60 segundos
    this.intervalSubscription = Observable.interval(60000).subscribe(() => {
      this.carregarSessoes();
    });
  }

  ngOnDestroy() {
    if (this.intervalSubscription) {
      this.intervalSubscription.unsubscribe();
    }
  }

  carregarSessoes() {
    this.loading = true;
    const params = this.placa ? { placa: this.placa } : {};
    
    this.sessaoService.listarSessoesAtivas(params).subscribe(
      (result: PagedResult<Sessao>) => {
        this.sessoes = result.data;
        this.totalRecords = result.pagination.totalItems;
        this.loading = false;
      },
      error => {
        console.error('Erro ao carregar sessões:', error);
        this.loading = false;
      }
    );
  }

  buscar() {
    this.carregarSessoes();
  }

  abrirModalSaida(sessao: Sessao) {
    this.sessaoSelecionada = sessao;
    this.loading = true;
    
    this.sessaoService.calcularValor(sessao.id).subscribe(
      response => {
        this.valorCalculado = response.valor;
        this.mostrarModalSaida = true;
        this.loading = false;
      },
      error => {
        console.error('Erro ao calcular valor:', error);
        this.loading = false;
      }
    );
  }

  confirmarSaida() {
    if (!this.sessaoSelecionada) return;
    
    this.loading = true;
    this.sessaoService.fecharSessao(this.sessaoSelecionada.id, this.sessaoSelecionada.rowVersion).subscribe(
      () => {
        this.mostrarModalSaida = false;
        this.sessaoSelecionada = null;
        this.carregarSessoes();
        this.loading = false;
      },
      error => {
        console.error('Erro ao registrar saída:', error);
        if (error.status === 409) {
          // Conflito de concorrência - recarregar dados
          this.carregarSessoes();
        }
        this.loading = false;
      }
    );
  }

  cancelarSaida() {
    this.mostrarModalSaida = false;
    this.sessaoSelecionada = null;
  }
}
