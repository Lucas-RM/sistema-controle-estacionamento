import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatioAgoraComponent } from './features/patio/patio-agora.component';
import { VeiculosListComponent } from './features/veiculos/veiculos-list.component';
import { RelatoriosComponent } from './features/relatorios/relatorios.component';
import { RegistrarEntradaComponent } from './features/movimentacao/registrar-entrada.component';

const routes: Routes = [
  { path: '', redirectTo: '/patio', pathMatch: 'full' },
  { path: 'patio', component: PatioAgoraComponent },
  { path: 'entrada', component: RegistrarEntradaComponent },
  { path: 'veiculos', component: VeiculosListComponent },
  { path: 'relatorios', component: RelatoriosComponent },
  { path: '**', redirectTo: '/patio' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
