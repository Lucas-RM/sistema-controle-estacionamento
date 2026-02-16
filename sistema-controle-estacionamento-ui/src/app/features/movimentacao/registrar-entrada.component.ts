import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { VeiculoService } from '../../core/services/veiculo.service';
import { SessaoService } from '../../core/services/sessao.service';
import { Veiculo } from '../../core/models/veiculo.model';

@Component({
  selector: 'app-registrar-entrada',
  templateUrl: './registrar-entrada.component.html',
  styleUrls: ['./registrar-entrada.component.css']
})
export class RegistrarEntradaComponent {
  form: FormGroup;
  loading: boolean = false;
  veiculoEncontrado: Veiculo | null = null;
  mensagem: string = '';
  erro: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private veiculoService: VeiculoService,
    private sessaoService: SessaoService
  ) {
    this.form = this.fb.group({
      placa: ['', [Validators.required, Validators.pattern(/^[A-Z]{3}[0-9]{4}$|^[A-Z]{3}[0-9][A-Z][0-9]{2}$|^[A-Z]{2}[0-9]{3}[A-Z]{2}$|^[A-Z]{3}[0-9]{3}$/)]]
    });
  }

  normalizarPlaca(value: string): string {
    return (value || '').toUpperCase().replace(/[\s-]/g, '');
  }

  buscarVeiculo() {
    this.mensagem = '';
    this.erro = '';
    this.veiculoEncontrado = null;

    if (this.form.invalid) {
      this.erro = 'Informe uma placa válida.';
      return;
    }

    const placa = this.normalizarPlaca(this.form.value.placa);
    this.loading = true;
    this.veiculoService.buscarPorPlaca(placa).subscribe(
      (veiculo: Veiculo) => {
        this.veiculoEncontrado = veiculo;
        this.loading = false;
      },
      (error: any) => {
        this.loading = false;
        if (error.status === 404) {
          this.erro = 'Veículo não encontrado. Cadastre-o na tela de Veículos.';
        } else {
          this.erro = error.message || 'Erro ao buscar veículo.';
        }
      }
    );
  }

  registrarEntrada() {
    if (!this.veiculoEncontrado) return;

    this.mensagem = '';
    this.erro = '';
    this.loading = true;

    this.sessaoService.abrirSessao({ veiculoId: this.veiculoEncontrado.id }).subscribe(
      () => {
        this.loading = false;
        this.mensagem = 'Entrada registrada com sucesso.';
        // Redireciona para o pátio após breve intervalo
        setTimeout(() => this.router.navigate(['/patio']), 800);
      },
      (error: any) => {
        this.loading = false;
        this.erro = error.message || 'Erro ao registrar entrada.';
      }
    );
  }

  irParaCadastroVeiculo() {
    this.router.navigate(['/veiculos']);
  }
}
