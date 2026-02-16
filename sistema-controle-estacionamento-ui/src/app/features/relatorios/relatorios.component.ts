import { Component, OnInit } from '@angular/core';
import { RelatorioService } from '../../core/services/relatorio.service';
import { FaturamentoItem, TopVeiculoItem, OcupacaoItem, TipoRelatorio, TipoRelatorioEnum } from '../../core/models/relatorio.model';

@Component({
  selector: 'app-relatorios',
  templateUrl: './relatorios.component.html',
  styleUrls: ['./relatorios.component.css']
})
export class RelatoriosComponent implements OnInit {
  loading: boolean = false;
  
  tipoRelatorioSelecionado: TipoRelatorio = TipoRelatorioEnum.Faturamento;
  tiposRelatorio = [
    { label: 'Faturamento por Dia', value: TipoRelatorioEnum.Faturamento },
    { label: 'Top 10 Veículos por Tempo', value: TipoRelatorioEnum.TopVeiculos },
    { label: 'Ocupação por Hora', value: TipoRelatorioEnum.Ocupacao }
  ];
  
  periodo: number = 1;
  periodos = [
    { label: 'Últimos 7 dias', value: 1 },
    { label: 'Últimos 30 dias', value: 2 }
  ];
  
  dataInicio: string | null = null;
  dataFim: string | null = null;
  
  // Dados dos relatórios
  faturamentoData: FaturamentoItem[] = [];
  topVeiculosData: TopVeiculoItem[] = [];
  ocupacaoData: OcupacaoItem[] = [];
  
  relatorioGerado: boolean = false;

  constructor(private relatorioService: RelatorioService) {}

  ngOnInit() {
    // Inicializa datas padrão (formato datetime-local: YYYY-MM-DDTHH:mm)
    const dataFim = new Date();
    const dataInicio = new Date();
    dataInicio.setDate(dataInicio.getDate() - 30);
    
    this.dataFim = this.formatDateTimeLocal(dataFim);
    this.dataInicio = this.formatDateTimeLocal(dataInicio);
  }

  /**
   * Preenche uma string com zeros à esquerda
   */
  private padStart(value: number, length: number): string {
    const str = String(value);
    if (str.length >= length) {
      return str;
    }
    // Cria uma string de zeros do tamanho necessário
    let zeros = '';
    for (let i = 0; i < length - str.length; i++) {
      zeros += '0';
    }
    return zeros + str;
  }

  /**
   * Converte um objeto Date para o formato datetime-local (YYYY-MM-DDTHH:mm)
   */
  private formatDateTimeLocal(date: Date): string {
    const year = date.getFullYear();
    const month = this.padStart(date.getMonth() + 1, 2);
    const day = this.padStart(date.getDate(), 2);
    const hours = this.padStart(date.getHours(), 2);
    const minutes = this.padStart(date.getMinutes(), 2);
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  /**
   * Obtém o offset de timezone do navegador em horas
   * Exemplo: -3 para GMT-3, 5 para GMT+5
   */
  private getTimezoneOffset(): number {
    return -new Date().getTimezoneOffset() / 60;
  }

  /**
   * Formata o offset de timezone para o padrão ISO 8601
   * Exemplo: -03:00 para GMT-3, +05:00 para GMT+5
   */
  private formatTimezoneOffset(): string {
    const offset = this.getTimezoneOffset();
    const sign = offset >= 0 ? '+' : '-';
    const absOffset = Math.abs(offset);
    const hours = this.padStart(Math.floor(absOffset), 2);
    const minutes = this.padStart((absOffset % 1) * 60, 2);
    return `${sign}${hours}:${minutes}`;
  }

  /**
   * Converte uma string datetime-local para formato ISO 8601 com timezone
   * Formato: YYYY-MM-DDTHH:mm:ss-03:00
   */
  private toISOStringWithTimezone(dateTimeLocal: string): string {
    // datetime-local retorna "YYYY-MM-DDTHH:mm"
    // Adiciona segundos (:00) e o offset de timezone
    if (dateTimeLocal.includes('T')) {
      const parts = dateTimeLocal.split('T');
      const date = parts[0];
      const time = parts[1] || '00:00';
      const timeParts = time.split(':');
      const hours = timeParts[0] || '00';
      const minutes = timeParts[1] || '00';
      
      const timezone = this.formatTimezoneOffset();
      return `${date}T${hours}:${minutes}:00${timezone}`;
    }
    return dateTimeLocal;
  }

  /**
   * Converte uma string datetime-local para formato de data apenas (YYYY-MM-DD)
   */
  private toDateString(dateTimeLocal: string): string {
    return dateTimeLocal.split('T')[0];
  }

  gerarRelatorio() {
    this.loading = true;
    this.relatorioGerado = false;
    
    if (this.tipoRelatorioSelecionado === TipoRelatorioEnum.Faturamento) {
      this.gerarFaturamento();
    } else if (this.tipoRelatorioSelecionado === TipoRelatorioEnum.TopVeiculos) {
      this.gerarTopVeiculos();
    } else if (this.tipoRelatorioSelecionado === TipoRelatorioEnum.Ocupacao) {
      this.gerarOcupacao();
    }
  }

  private gerarFaturamento() {
    this.relatorioService.faturamento(this.periodo).subscribe(
      data => {
        this.faturamentoData = data;
        this.relatorioGerado = true;
        this.loading = false;
      },
      error => {
        console.error('Erro ao gerar relatório:', error);
        alert(error.message);
        this.loading = false;
      }
    );
  }

  private gerarTopVeiculos() {
    if (!this.dataInicio || !this.dataFim) {
      alert('Selecione as datas de início e fim');
      this.loading = false;
      return;
    }
    
    const dataInicioStr = this.toISOStringWithTimezone(this.dataInicio);
    const dataFimStr = this.toISOStringWithTimezone(this.dataFim);
    
    this.relatorioService.topVeiculos(dataInicioStr, dataFimStr).subscribe(
      data => {
        this.topVeiculosData = data;
        this.relatorioGerado = true;
        this.loading = false;
      },
      error => {
        console.error('Erro ao gerar relatório:', error);
        alert(error.message);
        this.loading = false;
      }
    );
  }

  private gerarOcupacao() {
    if (!this.dataInicio || !this.dataFim) {
      alert('Selecione as datas de início e fim');
      this.loading = false;
      return;
    }
    
    if (this.dataInicio > this.dataFim) {
      alert('A data de início deve ser anterior à data fim');
      this.loading = false;
      return;
    }
    
    const dataInicioStr = this.toISOStringWithTimezone(this.dataInicio);
    const dataFimStr = this.toISOStringWithTimezone(this.dataFim);
    
    this.relatorioService.ocupacaoPorHora(dataInicioStr, dataFimStr).subscribe(
      data => {
        this.ocupacaoData = data;
        this.relatorioGerado = true;
        this.loading = false;
      },
      error => {
        console.error('Erro ao gerar relatório:', error);
        alert(error.message);
        this.loading = false;
      }
    );
  }

  getTotalFaturamento(): number {
    return this.faturamentoData.reduce((sum, item) => sum + item.valorTotal, 0);
  }

  getTotalSessoes(): number {
    return this.faturamentoData.reduce((sum, item) => sum + item.totalSessoes, 0);
  }
}
