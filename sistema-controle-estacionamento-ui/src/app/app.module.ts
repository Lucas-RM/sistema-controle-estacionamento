import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// PrimeNG
import { DialogModule } from 'primeng/primeng';
import { ButtonModule } from 'primeng/primeng';

// Routing
import { AppRoutingModule } from './app-routing.module';

// Components
import { AppComponent } from './app.component';
import { PatioAgoraComponent } from './features/patio/patio-agora.component';
import { VeiculosListComponent } from './features/veiculos/veiculos-list.component';
import { RelatoriosComponent } from './features/relatorios/relatorios.component';
import { RegistrarEntradaComponent } from './features/movimentacao/registrar-entrada.component';

// Services
import { ApiService } from './core/services/api.service';
import { VeiculoService } from './core/services/veiculo.service';
import { SessaoService } from './core/services/sessao.service';
import { RelatorioService } from './core/services/relatorio.service';

// Pipes
import { PlacaFormatPipe } from './shared/pipes/placa-format.pipe';
import { TempoPermanenciaPipe } from './shared/pipes/tempo-permanencia.pipe';
import { PeriodoFormatPipe } from './shared/pipes/periodo-format.pipe';
import { HoraFromPeriodoPipe } from './shared/pipes/hora-from-periodo.pipe';

@NgModule({
  declarations: [
    AppComponent,
    PatioAgoraComponent,
    VeiculosListComponent,
    RelatoriosComponent,
    RegistrarEntradaComponent,
    PlacaFormatPipe,
    TempoPermanenciaPipe,
    PeriodoFormatPipe,
    HoraFromPeriodoPipe
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    DialogModule,
    ButtonModule
  ],
  providers: [
    ApiService,
    VeiculoService,
    SessaoService,
    RelatorioService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
